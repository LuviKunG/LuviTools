using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameUIManager
{
    private static List<string> _uikey = new List<string>();

    public static void Show(string key) { if (!_uikey.Contains(key)) _uikey.Add(key); }
    public static void Hide(string key) { if (_uikey.Contains(key)) _uikey.Remove(key); }
    public static bool IsShowing(string key) { return _uikey.Contains(key); }

    private static int _uianim = 0;

    public static void StartAnimate() { _uianim++; }
    public static void StopAnimate() { _uianim--; if (_uianim < 0) _uianim = 0; }
    public static bool IsAnimating { get { return _uianim > 0; } }

    /// <summary>
    /// UI Key for check UI is active or deactive
    /// </summary>
    //public static List<string> UIKey = new List<string>();

    /// <summary>
    /// Return what UI is animating
    /// </summary>
    //public static bool IsUIAnimating { get { return UIAnimate != 0; } }

    /// <summary>
    /// Increase this param when UI begin animate
    /// Decrease this param when UI animate end
    /// </summary>
    //public static int UIAnimate = 0;
}