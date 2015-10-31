using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestMainUpdate : MonoBehaviour
{
    List<ILuviUpdate> listUpdate = new List<ILuviUpdate>();

    void Awake()
    {
        Singleton<TestMainUpdate>.Instance = this;
    }

    int index;
    IEnumerator Start()
    {
        while (true)
        {
            for (index = 0; index < listUpdate.Count; index++)
            {
                listUpdate[index].HookUpdate();
            }
            yield return null;
        }
    }

    public void AddUpdate(ILuviUpdate u)
    {
        listUpdate.Add(u);
    }
}
