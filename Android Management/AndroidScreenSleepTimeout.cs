using UnityEngine;

[DisallowMultipleComponent]
public class AndroidScreenSleepTimeout : AndroidSetting
{
    public enum Type : int { SystemSetting = -2, NeverSleep = -1, Custom = 0 }
    [SerializeField]
    private Type type = Type.SystemSetting;
    [SerializeField]
    private int customSleepTimeout = default;

    public override void Execute()
    {
        switch (type)
        {
            case Type.SystemSetting:
                Screen.sleepTimeout = SleepTimeout.SystemSetting;
                break;
            case Type.NeverSleep:
                Screen.sleepTimeout = SleepTimeout.NeverSleep;
                break;
            default:
                Screen.sleepTimeout = customSleepTimeout;
                break;
        }
    }
}