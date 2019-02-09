using UnityEngine;
using System.Collections;

public class LuviBehavior : MonoBehaviour
{
    private Transform cacheTrans = null;
    /// <summary>
    /// Cache old transform to fast pick-up.
    /// </summary>
    public new Transform transform
    {
        get
        {
            if (cacheTrans == null)
            {
                cacheTrans = base.transform;
            }
            return cacheTrans;
        }
    }

    private GameObject cacheGameObj = null;
    /// <summary>
    /// Cache old gameObject to fast pick-up.
    /// </summary>
    public new GameObject gameObject
    {
        get
        {
            if (cacheGameObj == null)
            {
                cacheGameObj = base.gameObject;
            }
            return cacheGameObj;
        }
    }
    
	private Rigidbody2D cacheRigid2D = null;
	/// <summary>
	/// Cache old gameObject to fast pick-up.
	/// </summary>
	public new Rigidbody2D rigidbody2D
	{
		get
		{
			if (cacheRigid2D == null)
			{
				cacheRigid2D = base.rigidbody2D;
			}
			return cacheRigid2D;
		}
	}

    //Coroutine hookUpdate;
    //public new bool enabled
    //{
    //    get
    //    {
    //        return hookUpdate != null;
    //    }
    //    set
    //    {
    //        if (value)
    //        {
    //            if (hookUpdate != null) StopCoroutine(hookUpdate);
    //            hookUpdate = StartCoroutine(HookUpdate());
    //        }
    //        else
    //        {

    //        }
    //    }
    //}
    //IEnumerator HookUpdate()
    //{

    //}
}

public interface ILuviUpdate
{
    void HookUpdate();
}