using UnityEngine;

[DisallowMultipleComponent]
public class AndroidKeyboardInput : AndroidSetting
{
    [SerializeField]
    private bool hideInput = true;

    public override void Execute()
    {
        TouchScreenKeyboard.hideInput = hideInput;
    }
}