using UnityEngine;
using LuviKunG;

public class LuviVungleAds : CacheBehaviour
{
    public LuviVungleAdsData data;

    private bool isCachePlayableAds;
    public bool IsCachePlayableAds
    {
        get { return isCachePlayableAds; }
    }

    public delegate void VungleAdsCallback(bool isComplete, bool isAdsClicked);
    private VungleAdsCallback onAdsCallback;

    [SerializeField]
    bool isInitOnStart = true;

    void Awake()
    {
        Singleton<LuviVungleAds>.Instance = this;
    }

    void Start()
    {
        if (isInitOnStart)
            Initialize();
    }

    public void Initialize()
    {
        if (string.IsNullOrEmpty(data.appID_Android))
        {
            Debug.LogError("'appIDAndroid' is null or empty");
            return;
        }
        if (string.IsNullOrEmpty(data.appID_IOS))
        {
            Debug.LogError("'appIDiOS' is null or empty");
            return;
        }
        if (!enabled)
        {
            Debug.LogWarning("'LuviVungleAds' still disable, please enable to register Vungle's callback.");
        }
        Vungle.init(data.appID_Android, data.appID_IOS, data.appID_Win);
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            Vungle.onPause();
        }
        else
        {
            Vungle.onResume();
        }
    }

    void OnEnable()
    {
        Vungle.adPlayableEvent += PlayableAdsCallback;
        Vungle.onAdStartedEvent += AdsStartCallback;
        Vungle.onAdFinishedEvent += AdsFinishCallback;
#if LUVI_VUNGLE_LOG
        Vungle.onLogEvent += Log;
#endif
    }

    void OnDisable()
    {
        Vungle.adPlayableEvent -= PlayableAdsCallback;
        Vungle.onAdStartedEvent -= AdsStartCallback;
        Vungle.onAdFinishedEvent -= AdsFinishCallback;
#if LUVI_VUNGLE_LOG
        Vungle.onLogEvent -= Log;
#endif
    }

    public bool IsAdsAvailable
    {
        get { return Vungle.isAdvertAvailable(); }
    }

    public void ShowAdvertisment(VungleAdsCallback callback)
    {
        onAdsCallback = callback;
#if UNITY_EDITOR
        Log("Cannot play ads on editor.");
        return;
#elif UNITY_ANDROID
        Log("ShowAdvertisment");
        VungleAndroid.playAdEx(true, (int)VungleAdOrientation.MatchVideo, true);
#endif
    }

    public void ClearCacheAdvertisment()
    {
        Vungle.clearCache();
        Log("ClearCacheAdvertisment");
    }

    public void AdsStartCallback()
    {
        Log("Ads Start");
    }

    public void AdsFinishCallback(AdFinishedEventArgs _event)
    {
        bool isComplete = _event.IsCompletedView;
        bool isAdsClicked = _event.WasCallToActionClicked;
        if (onAdsCallback != null)
            onAdsCallback(isComplete, isAdsClicked);
        Log("Ads Finish|isComplete: " + isComplete + "|isAdsClicked: " + isAdsClicked);
    }

    public void PlayableAdsCallback(bool isPlayable)
    {
        isCachePlayableAds = isPlayable;
        Log("isPlayable: " + isPlayable);
    }

    void Log(object message)
    {
#if LUVI_VUNGLE_LOG
        Debug.Log("LuviVungleAds|" + message.ToString());
#endif
    }

#if LUVI_VUNGLE_TESTGUI
    void OnGUI()
    {
        GUILayout.BeginVertical(GUILayout.Width(Screen.width / 2));
        Label("IsAdsAvailable: " + IsCachePlayableAds);
        if (Button("Play Ads", IsAdsAvailable))
        {
            ShowAdvertisment((isComplete, isAdsClicked) =>
            {
                Log("Play Ads Callbacked!|isComplete: " + isComplete + "|isAdsClicked: " + isAdsClicked);
            });
        }
        if (Button("Clean Cache Ads", IsAdsAvailable))
        {
            ClearCacheAdvertisment();
        }
        GUILayout.EndVertical();
    }

    bool Button(string _label)
    {
        return GUILayout.Button(_label);
    }

    bool Button(string _label, bool _enable)
    {
        if (_enable)
        {
            return Button(_label);
        }
        else
        {
            GUI.enabled = false;
            bool button = GUILayout.Button(_label);
            GUI.enabled = true;
            return button;
        }
    }

    void Label(string _text)
    {
        GUILayout.Label(_text);
    }

    string TextField(string _text)
    {
        return GUILayout.TextField(_text);
    }
#endif

#if UNITY_EDITOR
    void Reset()
    {
        name = "LuviVungleAds";
    }
#endif
}
