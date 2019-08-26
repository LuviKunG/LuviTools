using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using WebSocketSharp;
using LitJson;

namespace LuviKunG.Web.Socket
{
    public sealed class WebSocketIO
    {
        public const int EIO = 3;
        public const string TRANSPORT = "websocket";
        public const float PING_INTERVAL = 10.0f;

        private WebSocket m_webSocket = default;
        public WebSocket webSocket => m_webSocket;

        private string m_sid = null;
        public string sid => m_sid;

        private float m_pingInterval = 0;
        public float pingInterval => m_pingInterval;

        private float m_pingTimeout = 0;
        public float pingTimeout => m_pingTimeout;

        private Dictionary<string, WebSocketEvent> events;
        private WebSocketBehaviour behaviour;
        private Coroutine routinePing;

        private WebSocketIO()
        {
            events = new Dictionary<string, WebSocketEvent>();
            behaviour = WebSocketBehaviour.Instance;
        }

        public WebSocketIO(string uri, int eio = EIO, string transport = TRANSPORT) : this()
        {
            UriBuilder webSocketUri = new UriBuilder(uri);
            if (!string.Equals(webSocketUri.Scheme, "ws"))
                throw new Exception("Not a web socket url scheme.");
            UriQuery uriQuery = new UriQuery(webSocketUri.Query);
            uriQuery.SetKey("EIO", eio);
            uriQuery.SetKey("transport", transport);
            webSocketUri.Query = uriQuery.ToQueryString();
            webSocketUri.Path += "socket.io/";
            m_webSocket = new WebSocket(webSocketUri.ToString());
        }

        public WebSocketIO(string uri, UriQuery query, int eio = EIO, string transport = TRANSPORT) : this()
        {
            UriBuilder webSocketUri = new UriBuilder(uri);
            if (!string.Equals(webSocketUri.Scheme, "ws"))
                throw new Exception("Not a web socket url scheme.");
            UriQuery uriQuery = new UriQuery(query);
            uriQuery.SetKey("EIO", eio);
            uriQuery.SetKey("transport", transport);
            webSocketUri.Query = uriQuery.ToQueryString();
            webSocketUri.Path += "socket.io/";
            m_webSocket = new WebSocket(webSocketUri.ToString());
        }

        private void EventHandlerOnOpen(object sender, EventArgs e)
        {
            Debug.Log("OnOpen");
            routinePing = behaviour.StartCoroutine(RoutinePing());
        }

        private void EventHandlerOnClose(object sender, CloseEventArgs e)
        {
            Debug.LogWarning($"OnClose\nCode: {e.Code}\nReason: {e.Reason}\nWasClean: {e.WasClean}");
            if (routinePing != null)
                behaviour.StopCoroutine(routinePing);
            Disconnect();
        }

        private void EventHandlerOnMessage(object sender, MessageEventArgs e)
        {
            if (e.IsPing)
                return;
            Debug.Log($"OnMessage\nData: {e.Data}");
            var eventData = new WebSocketEventData(e.Data);
            switch (eventData.code)
            {
                //Handshake event.
                case "0":
                    {
                        var data = eventData.data;
                        if (data != null && data.IsObject)
                        {
                            if (data.ContainsKey("sid") && data["sid"].IsString)
                            {
                                m_sid = (string)data["sid"];
                                Debug.Log($"SID: {m_sid}");
                            }
                            if (data.ContainsKey("pingInterval") && data["pingInterval"].IsInt)
                            {
                                var cPingInterval = (int)data["pingInterval"];
                                m_pingInterval = cPingInterval / 1000.0f;
                                Debug.Log($"PingInterval: {m_pingInterval.ToString("N2")}");
                            }
                            if (data.ContainsKey("pingTimeout") && data["pingTimeout"].IsInt)
                            {
                                var cPingTimeout = (int)data["pingTimeout"];
                                m_pingTimeout = cPingTimeout / 1000.0f;
                                Debug.Log($"PingTimeout: {m_pingTimeout.ToString("N2")}");
                            }
                        }
                    }
                    break;
                //Ping-pong event.
                case "3":
                    {
                        
                    }
                    break;
                case "42":
                    {
                        if (events.ContainsKey(eventData.eventName))
                            behaviour.AddAction(() => { events[eventData.eventName]?.Invoke(eventData.data); });
                    }
                    break;
                default:
                    break;
            }
            Debug.Log(eventData.ToString());
        }

        private void EventHandlerOnError(object sender, ErrorEventArgs e)
        {
            Debug.LogError($"OnError\nMessage: {e.Message}\nException: {e.Exception}");
        }

        public WebSocketIO Connect(ConnectionCallback onSuccess, ConnectionCallbackError onError)
        {
            m_webSocket.OnOpen += EventHandlerOnOpen;
            m_webSocket.OnClose += EventHandlerOnClose;
            m_webSocket.OnMessage += EventHandlerOnMessage;
            m_webSocket.OnError += EventHandlerOnError;

            m_webSocket.OnOpen += OnOpen;
            m_webSocket.OnClose += OnClose;
            m_webSocket.Connect();
            return this;

            void OnOpen(object sender, EventArgs e)
            {
                m_webSocket.OnOpen -= OnOpen;
                m_webSocket.OnClose -= OnClose;
                onSuccess?.Invoke();
            }
            void OnClose(object sender, CloseEventArgs e)
            {
                m_webSocket.OnOpen -= OnOpen;
                m_webSocket.OnClose -= OnClose;
                onError?.Invoke(e.Code, e.Reason, e.WasClean);
            }
        }

        public void Disconnect()
        {
            m_webSocket.Close();
            m_webSocket.OnOpen -= EventHandlerOnOpen;
            m_webSocket.OnClose -= EventHandlerOnClose;
            m_webSocket.OnMessage -= EventHandlerOnMessage;
            m_webSocket.OnError -= EventHandlerOnError;
        }

        public WebSocketIO On(string eventName, WebSocketEvent data)
        {
            events.Add(eventName, data);
            return this;
        }

        public void Emit(string eventName, JsonData data)
        {
            var wsData = new WebSocketEventData("42", eventName, data);
            string d = wsData.ToSocketData();
            Debug.Log(d);
            m_webSocket.Send(d);
        }

        private IEnumerator RoutinePing()
        {
            while (m_webSocket.IsAlive)
            {
                m_webSocket.Send("2");
                yield return new WaitForSecondsRealtime(PING_INTERVAL);
            }
        }
    }
}