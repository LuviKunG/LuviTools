using UnityEngine;

[DisallowMultipleComponent]
public class AndroidTargetFramerate : AndroidSetting
{
    public int targetFramerate;

    public override void Execute()
    {
        Application.targetFrameRate = targetFramerate;
    }
}