using LitJson;
using System;
using System.Text;

namespace LuviKunG.Web.Socket
{
    public struct WebSocketEventData
    {
        public string code;
        public string eventName;
        public JsonData data;

        public WebSocketEventData(string code, string eventName, JsonData data)
        {
            this.code = code;
            this.eventName = eventName;
            this.data = data;
        }

        public WebSocketEventData(string parse) : this()
        {
            for (int i = 0; i < parse.Length; i++)
            {
                if (parse[i] == '[' || parse[i] == '{')
                {
                    code = parse.Substring(0, i);
                    string rEventData = parse.Substring(i);
                    JsonData json = JsonMapper.ToObject(rEventData);
                    if (json.IsArray && json.Count > 1)
                    {
                        if (json[0].IsString)
                            eventName = (string)json[0];
                        else
                            throw new Exception($"Cannot parse {nameof(eventName)} because it's wrong type.");
                        data = json[1];
                    }
                    else if (json.IsObject)
                    {
                        data = json;
                    }
                    break;
                }
                else continue;
            }
            if (string.IsNullOrEmpty(code))
            {
                code = parse;
            }
        }

        public string ToSocketData()
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(code))
                sb.Append(code);
            if (string.IsNullOrEmpty(eventName))
            {
                if (data == null)
                    throw new Exception($"Cannot create string socket data from null data.");
                else
                    sb.Append(data.ToJson());
            }
            else
            {
                JsonData json = new JsonData();
                json.SetJsonType(JsonType.Array);
                json.Add(eventName);
                json.Add(data);
                sb.Append(json.ToJson());
            }
            return sb.ToString();
        }

        public override string ToString()
        {
            JsonData json = new JsonData();
            json.SetJsonType(JsonType.Object);
            json["code"] = code;
            json["eventName"] = eventName;
            json["data"] = data;
            return json.ToJson();
        }
    }
}