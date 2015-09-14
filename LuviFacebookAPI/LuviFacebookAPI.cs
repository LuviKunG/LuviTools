using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Facebook;
using Facebook.MiniJSON;

public class LuviFacebookAPI : LuviBehavior
{
    private static LuviFacebookAPI _instance = null;
    public static LuviFacebookAPI Instance
    {
        get
        {
            if (!_instance) RequestInstance();
            return _instance;
        }
    }

    private static void RequestInstance()
    {
        GameObject obj = new GameObject("LuviFacebookAPI");
        _instance = obj.AddComponent<LuviFacebookAPI>();
    }

    public string defaultPermission = "public_profile,email,user_friends";
    [HideInInspector]
    public string userProfileID;
    [HideInInspector]
    public string userProfileName;
    [HideInInspector]
    public Texture2D userProfilePicture;

    public delegate void ActionCallback();
    private ActionCallback actionInitCallback;
    public delegate void ActionSuccessCallback(bool isSuccess);
    private ActionSuccessCallback actionLoginCallback;
    private ActionSuccessCallback actionGetBasicUserProfileCallback;
    private ActionSuccessCallback actionGetPictureProfileCallback;
    private ActionSuccessCallback actionFeedDialogCallback;
    private ActionSuccessCallback actionFeedAPICallback;

    private LuviFacebookFeedAPI openFeed;

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

    public void Init(ActionCallback callback = null)
    {
        if (FB.IsInitialized)
        {
            if (callback != null) callback();
            return;
        }
        Debug.Log("Initialize");
        actionInitCallback = callback;
        FB.Init(InitCallBack);
    }

    private void InitCallBack()
    {
        Debug.Log("Initialize Complete|isLoggedIn: " + FB.IsLoggedIn);
        if (FB.IsLoggedIn)
        {
            GetBasicUserProfile();
            GetPictureProfile();
        }
        if (actionInitCallback != null) actionInitCallback();
    }

    public void Login(ActionSuccessCallback callback = null)
    {
        if (FB.IsLoggedIn)
        {
            if (callback != null) callback(false);
            return;
        }
        Debug.Log("Login|permission: " + defaultPermission);
        actionLoginCallback = callback;
        FB.Login(defaultPermission, LoginCallBack);
    }

    public void Login(string permission, ActionSuccessCallback callback = null)
    {
        defaultPermission = permission;
        Login(callback);
    }

    private void LoginCallBack(FBResult result)
    {
        if (!string.IsNullOrEmpty(result.Error))
        {
            Debug.Log(string.Format("Login Error Response: {0}", result.Error));
            if (actionLoginCallback != null) actionLoginCallback(false);
        }
        else if (!FB.IsLoggedIn)
        {
            Debug.Log(string.Format("Login was cancelled by user: {0}", result.Text));
            if (actionLoginCallback != null) actionLoginCallback(false);
        }
        else
        {
            Debug.Log(string.Format("Login was successful: {0}", result.Text));
            if (actionLoginCallback != null) actionLoginCallback(true);
        }
        result.Dispose();
    }

    public void Logout()
    {
        if (!FB.IsLoggedIn) return;
        Debug.Log("Logout from Facebook");
        FB.Logout();
    }

    public void GetBasicUserProfile(ActionSuccessCallback callback = null)
    {
        Debug.Log("Get Basic User Profile");
        if (FB.IsInitialized && FB.IsLoggedIn)
        {
            actionGetBasicUserProfileCallback = callback;
            FB.API("me?fields=id,name", HttpMethod.GET, GetBasicUserProfileCallback);
        }
    }

    public void GetBasicUserProfileCallback(FBResult result)
    {
        if (!string.IsNullOrEmpty(result.Error))
        {
            Debug.Log(string.Format("Failed to get basic user profile: {0}", result.Error));
            if (actionGetBasicUserProfileCallback != null) actionGetBasicUserProfileCallback(false);
        }
        else
        {
            Dictionary<string, object> resultAPIData = Json.Deserialize(result.Text) as Dictionary<string, object>;
            userProfileID = resultAPIData["id"] as string;
            userProfileName = resultAPIData["name"] as string;
            Debug.Log(string.Format("id: {0}|name: {1}", userProfileID, userProfileName));
            if (actionGetBasicUserProfileCallback != null) actionGetBasicUserProfileCallback(true);
        }
        result.Dispose();
    }

    public void GetPictureProfile(ActionSuccessCallback callback = null)
    {
        Debug.Log("Get Picture Profile");
        if (FB.IsInitialized && FB.IsLoggedIn)
        {
            actionGetPictureProfileCallback = callback;
            FB.API("me/picture?type=large&fields=url&redirect=false", HttpMethod.GET, GetPictureProfileCallback);
        }
    }

    public void GetPictureProfileCallback(FBResult result)
    {
        if (!string.IsNullOrEmpty(result.Error))
        {
            Debug.Log(string.Format("Failed to get picture profile: {0}", result.Error));
            if (actionGetBasicUserProfileCallback != null) actionGetBasicUserProfileCallback(false);
        }
        else
        {
            Dictionary<string, object> resultAPIData = Json.Deserialize(result.Text) as Dictionary<string, object>;
            resultAPIData = resultAPIData["data"] as Dictionary<string, object>;
            string pictureURL = resultAPIData["url"] as string;
            StartCoroutine(GetPictureProfileFromURL(pictureURL, actionGetPictureProfileCallback));
            Debug.Log(string.Format("url: {0}", pictureURL));
        }
        result.Dispose();
    }

    public IEnumerator GetPictureProfileFromURL(string url, ActionSuccessCallback callback = null)
    {
        WWW geturl = new WWW(url);
        yield return geturl;
        if (!string.IsNullOrEmpty(geturl.error))
        {
            Debug.Log("Error to get picture URL");
            if (callback != null) callback(false);
            yield break;
        }
        userProfilePicture = new Texture2D(128, 128, TextureFormat.DXT1, false);
        geturl.LoadImageIntoTexture(userProfilePicture);
        Debug.Log("Done to get picture");
        if (callback != null) callback(true);
    }

    public void OpenFeedDialog(LuviFacebookFeedAPI feed, ActionSuccessCallback callback = null)
    {
        Debug.Log("Open Feed Dialog");
        if (FB.IsInitialized && FB.IsLoggedIn)
        {
            actionFeedDialogCallback = callback;
            openFeed = feed;
            FB.Feed(
                link: openFeed.url,
                linkName: openFeed.header,
                linkCaption: openFeed.caption,
                linkDescription: openFeed.description,
                picture: openFeed.picture,
                callback: OpenFeedCallback
            );
        }
    }

    private void OpenFeedCallback(FBResult result)
    {
        if (!string.IsNullOrEmpty(result.Error))
        {
            Debug.Log("Failed to feed: " + result.Error);
            if (actionFeedDialogCallback != null) actionFeedDialogCallback(false);
        }
        else
        {
            Debug.Log("Feed success");
            if (actionFeedDialogCallback != null) actionFeedDialogCallback(true);
        }
        result.Dispose();
    }

    public void FeedAPI(LuviFacebookFeedAPI feed, ActionSuccessCallback callback = null)
    {
        Debug.Log("FeedAPI");
        actionFeedAPICallback = callback;
        openFeed = feed;
        WWWForm form = new WWWForm();
        form.AddField("link", openFeed.url);
        form.AddField("name", openFeed.header);
        form.AddField("caption", openFeed.caption);
        form.AddField("description", openFeed.description);
        form.AddField("picture", openFeed.picture);
        form.AddField("actions", "[{'name':'picture','link':'" + openFeed.picture + "'}]");
        FB.API("/me/feed", HttpMethod.POST, FeedAPICallback, form);
    }

    private void FeedAPICallback(FBResult result)
    {
        if (!string.IsNullOrEmpty(result.Error))
        {
            Debug.Log("Failed to feed: " + result.Error);
            if (actionFeedAPICallback != null) actionFeedAPICallback(false);
        }
        else
        {
            Debug.Log("Feed success");
            if (actionFeedAPICallback != null) actionFeedAPICallback(true);
        }
        result.Dispose();
    }
}

public struct LuviFacebookFeedAPI
{
    public string url;
    public string header;
    public string caption;
    public string description;
    public string picture;
}