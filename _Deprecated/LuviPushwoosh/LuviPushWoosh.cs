using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class LuviPushWoosh : MonoBehaviour
{
    [SerializeField]
    private bool enableMultiNotification = true;
    [SerializeField]
    private bool clearPushHistoryOnStart = false;
    [SerializeField]
    private bool clearNotificationOnStart = false;
    [SerializeField]
    private bool debugOutputPushHistory = false;

    public Action<bool> RegisteredForPushNotificationsCallback = null;
    public Action PushNotificationsReceivedCallback = null;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public bool SetNotification
    {
        get { return PlayerPrefs.GetInt("Notification", 1) > 0; }
        set
        {
            if (value)
            {
                PlayerPrefs.SetInt("Notification", 1);
                RegisterForPushNotifications();
            }
            else
            {
                PlayerPrefs.SetInt("Notification", 0);
                UnregisterForPushNotifications();
            }
        }
    }
    public enum SoundNotificationType { Default = 0, NoSound = 1, Always = 2 }
    public SoundNotificationType SetSoundNotificationType { set { pushwoosh.Call("setSoundNotificationType", value); } }
    public enum VibrateNotificationType { Default = 0, NoSound = 1, Always = 2 }
    public VibrateNotificationType VibrateNotification { set { pushwoosh.Call("setVibrateNotificationType", value); } }

    public bool LightUpScreenNotification { set { pushwoosh.Call("setLightScreenOnNotification", value); } }
    public bool EnableLED { set { pushwoosh.Call("setEnableLED", value); } }

    public string PushToken { get { return pushwoosh.Call<string>("getPushToken"); } }
    public string PushwooshHWID { get { return pushwoosh.Call<string>("getPushwooshHWID"); } }
    public string[] PushHistory
    {
        get
        {
            AndroidJavaObject history = pushwoosh.Call<AndroidJavaObject>("getPushHistory");
            if (history.GetRawObject().ToInt32() == 0)
            {
                return new String[0];
            }
            string[] result = AndroidJNIHelper.ConvertFromJNIArray<String[]>(history.GetRawObject());
            history.Dispose();
            return result;
        }
    }

    private static AndroidJavaObject pushwoosh = null;

    void Start()
    {
        InitPushwoosh();
        if (SetNotification) SetNotification = true;
        else SetNotification = false;
        Debug.Log("LuviPushWoosh|GetPushToken: " + PushToken);
        if (clearNotificationOnStart) ClearNotificationCenter();
        if (debugOutputPushHistory)
        {
            string[] result = PushHistory;
            foreach (string str in result)
            {
                Debug.Log("LuviPushWoosh|\'history result\': " + str);
            }
        }
        if (clearPushHistoryOnStart) ClearPushHistory();
        if (enableMultiNotification) SetMultiNotificationMode();
        else SetSimpleNotificationMode();
    }

    void InitPushwoosh()
    {
        if (pushwoosh != null) return;
        using (var pluginClass = new AndroidJavaClass("com.arellomobile.android.push.PushwooshProxy")) pushwoosh = pluginClass.CallStatic<AndroidJavaObject>("instance");
        pushwoosh.Call("setListenerName", this.gameObject.name);
    }
    public void RegisterForPushNotifications() { pushwoosh.Call("registerForPushNotifications"); }
    public void UnregisterForPushNotifications() { pushwoosh.Call("unregisterFromPushNotifications"); }

    public void SetIntTag(string tagName, int tagValue) { pushwoosh.Call("setIntTag", tagName, tagValue); }
    public void SetStringTag(string tagName, string tagValue) { pushwoosh.Call("setStringTag", tagName, tagValue); }
    public void SetListTag(string tagName, List<object> tagValues)
    {
        AndroidJavaObject tags = new AndroidJavaObject("com.arellomobile.android.push.TagValues");
        foreach (var tagValue in tagValues) { tags.Call("addValue", tagValue); }
        pushwoosh.Call("setListTag", tagName, tags);
    }

    public void ClearPushHistory() { pushwoosh.Call("clearPushHistory"); }

    public void sendLocation(double lat, double lon) { pushwoosh.Call("sendLocation", lat, lon); }
    public void startTrackingGeoPushes() { pushwoosh.Call("startTrackingGeoPushes"); }
    public void stopTrackingGeoPushes() { pushwoosh.Call("stopTrackingGeoPushes"); }
    public void startTrackingBeaconPushes() { pushwoosh.Call("startTrackingBeaconPushes"); }
    public void stopTrackingBeaconPushes() { pushwoosh.Call("stopTrackingBeaconPushes"); }
    public void setBeaconBackgroundMode(bool backgroundMode) { pushwoosh.Call("setBeaconBackgroundMode", backgroundMode); }


    public void ClearLocalNotifications() { pushwoosh.Call("clearLocalNotifications"); }
    public void ClearNotificationCenter() { pushwoosh.Call("clearNotificationCenter"); }

    public void ScheduleLocalNotification(string message, int seconds) { pushwoosh.Call("scheduleLocalNotification", message, seconds); }
    public void ScheduleLocalNotification(string message, int seconds, string userdata) { pushwoosh.Call("scheduleLocalNotification", message, seconds, userdata); }

    public void SetMultiNotificationMode() { pushwoosh.Call("setMultiNotificationMode"); }
    public void SetSimpleNotificationMode() { pushwoosh.Call("setSimpleNotificationMode"); }



    void onRegisteredForPushNotifications(string token)
    {
        Debug.Log("LuviPushWoosh|onRegisteredForPushNotifications: " + token);
        if (RegisteredForPushNotificationsCallback != null) RegisteredForPushNotificationsCallback(true);
    }
    void onFailedToRegisteredForPushNotifications(string error)
    {
        Debug.Log("LuviPushWoosh|onFailedToRegisteredForPushNotifications: " + error);
        if (RegisteredForPushNotificationsCallback != null) RegisteredForPushNotificationsCallback(false);
    }
    void onPushNotificationsReceived(string payload)
    {
        Debug.Log("LuviPushWoosh|onPushNotificationsReceived: " + payload);
        if (PushNotificationsReceivedCallback != null) PushNotificationsReceivedCallback();
    }
    void OnApplicationPause(bool paused)
    {
        if (pushwoosh == null) InitPushwoosh();
        if (paused)
        {
            pushwoosh.Call("onPause");
        }
        else
        {
            pushwoosh.Call("onResume");
        }
    }
}
