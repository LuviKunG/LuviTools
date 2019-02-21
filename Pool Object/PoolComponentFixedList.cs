using UnityEngine;
using System.Collections.Generic;

namespace LuviKunG.Pool
{
    public class PoolComponentFixedList : MonoBehaviour
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
        public T Pick<T>() where T : IPoolableComponentFixedList
        {
            T obj = Pick().GetComponent<T>();
            obj.PoolRegister(this);
            return obj;
        }
    }
}