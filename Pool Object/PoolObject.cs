using UnityEngine;
using System.Collections.Generic;

namespace LuviKunG
{
    public class PoolObject : CacheBehaviour
    {
        [SerializeField]
        List<GameObject> list = new LoopList<GameObject>();
        public LoopList<GameObject> List
        {
            get { return (LoopList<GameObject>)list; }
            set { list = value; }
        }

        public GameObject Pick()
        {
            return List.GetNext();
        }

        public T Pick<T>() where T : MonoBehaviour
        {
            return Pick().GetComponent<T>();
        }
    }
}