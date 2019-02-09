using UnityEngine;

public class GameConfiguration : MonoBehaviour
{
    public int targetFrameRate = 60;
    public bool runInBackground = true;
    public bool multiTouchEnabled = false;
    public bool keyboardHideInput = true;

    void Awake()
    {
        Application.targetFrameRate = targetFrameRate;
        Application.runInBackground = runInBackground;
        Input.multiTouchEnabled = multiTouchEnabled;
        TouchScreenKeyboard.hideInput = keyboardHideInput;
    }
}
