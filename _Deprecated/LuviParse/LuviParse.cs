using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Text;
using System.Text.RegularExpressions;
using Boomlagoon.JSON;

/* LuviParse
 * version 1.0.0
 * by LuviKunG
 * http://www.facebook.com/LuviKunG
 * http://www.4dbox.in.th
 * 
 * This script use with HTTP Request and only for Parse Cloud
 * every callback that have boolean for check it's cache exception or not
 * and string JSON object
 * (recommend to use with Boomlagoon.JSON)
 * No require Parse Library
 */
public class LuviParse : LuviBehavior
{
    private static LuviParse _instance;
    public static LuviParse Instance
    {
        get
        {
            if (!_instance) RequestInstance();
            return _instance;
        }
    }
    public string parseApplicationID;
    public string parseRestAPIKey;

    public static void RequestInstance()
    {
        GameObject obj = new GameObject("LuviParse");
        _instance = obj.AddComponent<LuviParse>();
        DontDestroyOnLoad(obj);
    }

    void Awake()
    {
        if (_instance)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
    }

    private void RequestFunction(string function, JSONObject data, Action<bool, string> callback)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.parse.com/1/functions/" + function);
        string postData = data.ToString();
        byte[] byteData = Encoding.ASCII.GetBytes(postData);
        request.ContentType = "application/json";
        request.ContentLength = byteData.Length;
        request.Method = "POST";
        request.Headers.Add("X-Parse-Application-Id", parseApplicationID);
        request.Headers.Add("X-Parse-REST-API-Key", parseRestAPIKey);
        request.Credentials = CredentialCache.DefaultCredentials;
        using (Stream stream = request.GetRequestStream()) { stream.Write(byteData, 0, byteData.Length); }
        try
        {
            request.BeginGetResponse(new AsyncCallback(delegate(IAsyncResult result)
            {
                HttpWebResponse response = (result.AsyncState as HttpWebRequest).EndGetResponse(result) as HttpWebResponse;
                using (Stream stream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(stream);
                    if (callback != null) callback(true, reader.ReadToEnd());
                    reader.Close();
                    response.Close();
                }
            }), request);
        }
        catch (Exception e)
        {
            Debug.Log(string.Format("Exception Cached: {0}|{1}", e.Source, e.Message));
            if (callback != null) callback(false, e.Message);
        }
    }

    public static string EscapeURL(string str)
    {
        return Regex.Escape(WWW.EscapeURL(str, Encoding.UTF8));
    }

    public static string UnescapeURL(string str)
    {
        return WWW.UnEscapeURL(Regex.Unescape(str), Encoding.UTF8);
    }
}
