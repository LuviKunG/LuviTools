using UnityEngine;
using System.Collections.Generic;

namespace LuviKunG.Pool
{
    public class PoolObjectFlexibleClass<TObject> where TObject : Object, IPoolableObjectFlexibleClass<TObject>
    {
        List<TObject> activeList = new List<TObject>();
        List<TObject> deactiveList = new List<TObject>();

        public void Add(TObject obj, bool isActive = false)
        {
            obj.PoolRegister(this);
            if (isActive)
            {
                obj.PoolActive();
                activeList.Add(obj);
            }
            else
            {
                obj.PoolDeactive();
                deactiveList.Add(obj);
            }
        }

        public bool Remove(TObject obj)
        {
            if (activeList.Remove(obj))
            {
                obj.PoolRegister(null);
                return true;
            }
            else if (deactiveList.Remove(obj))
            {
                obj.PoolRegister(null);
                return true;
            }
            else
                return false;
        }

        public TObject Pick(bool isActive = true)
        {
            if (deactiveList.Count > 0)
            {
                TObject obj = deactiveList[0];
                if (isActive)
                {
                    deactiveList.RemoveAt(0);
                    activeList.Add(obj);
                    obj.PoolActive();
                }
                return obj;
            }
            else
                return null;
        }

        public TObject Peek()
        {
            if (deactiveList.Count > 0)
                return deactiveList[0];
            else
                return null;
        }

        public bool Return(TObject obj)
        {
            if (obj.GetPool == this)
            {
                if (activeList.Remove(obj))
                {
                    deactiveList.Add(obj);
                    obj.PoolDeactive();
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }
    }
}