using UnityEngine;

[DisallowMultipleComponent]
public class AndroidScreenSleepTimeout : AndroidSetting
{
    public enum Type : int { SystemSetting = -2, NeverSleep = -1, Custom = 0 }
    public Type type;
    public int customSleepTimeout;

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