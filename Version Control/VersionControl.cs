using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using MiniJSONDecode;

public class VersionControl : MonoBehaviour
{
    private static VersionControl _instance = null;
    public static VersionControl Instance
    {
        get { return RequestInstance(); }
        set { _instance = value; }
    }

    private static VersionControl RequestInstance()
    {
        if (_instance == null)
        {
            GameObject obj = new GameObject();
            obj.name = "VersionControl";
            _instance = obj.AddComponent<VersionControl>();
        }
        return _instance;
    }

    public static Dictionary<string, object> dict = new Dictionary<string, object>();
    public static bool isLatest;

    [SerializeField]
    private float waitingTime = 5.0f;

    private Action<bool> actionCallback;
    private bool isLoadSuccess;

    void Awake()
    {
        if (!_instance) _instance = this;
        isLoadSuccess = false;
        isLatest = false;
    }

    public void Load(string url, Action<bool> callback)
    {
#if Debug
        isLoadSuccess = true;
        isLatest = true;
        callback(true);
        return;
#endif
        if (!_instance) RequestInstance();
        if (isLoadSuccess)
        {
            callback(true);
            return;
        }
        actionCallback = callback;
        StartCoroutine(Download(url));
    }

    IEnumerator Download(string url)
    {
        WWW www = new WWW(url);
        float waitStartTime = Time.time;
        while (!www.isDone)
        {
            if (Time.time - waitStartTime > waitingTime)
            {
                actionCallback(false);
                yield break;
            }
            yield return www;
        }
        if (string.IsNullOrEmpty(www.error) && !string.IsNullOrEmpty(www.text))
        {
            dict = Json.Deserialize(www.text) as Dictionary<string, object>;
            actionCallback(true);
            isLoadSuccess = true;
        }
        else
        {
            actionCallback(false);
            isLoadSuccess = false;
        }
    }

    void OnDestroy()
    {
        _instance = null;
    }
}
