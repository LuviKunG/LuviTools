using UnityEngine;

[DisallowMultipleComponent]
public class AndroidMultiTouch : AndroidSetting
{
    [SerializeField]
    private bool enableMultiTouch = true;

    public override void Execute()
    {
        Input.multiTouchEnabled = enableMultiTouch;
    }
}