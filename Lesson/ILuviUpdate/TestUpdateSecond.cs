using UnityEngine;
using System.Collections;

public class TestUpdateSecond : MonoBehaviour, ILuviUpdate
{
    public void Start()
    {
        Singleton<TestMainUpdate>.Instance.AddUpdate(this);
    }

    public void HookUpdate()
    {
        
    }
}
