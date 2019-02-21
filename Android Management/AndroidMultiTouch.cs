using UnityEngine;

[DisallowMultipleComponent]
public class AndroidMultiTouch : AndroidSetting
{
    public bool enableMultiTouch;

    public override void Execute()
    {
        Input.multiTouchEnabled = enableMultiTouch;
    }
}