using UnityEngine;

[DisallowMultipleComponent]
public class AndroidTargetFramerate : AndroidSetting
{
    [SerializeField]
    private int targetFramerate = 60;

    public override void Execute()
    {
        Application.targetFrameRate = targetFramerate;
    }
}