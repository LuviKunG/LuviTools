using UnityEngine;
using System.Collections.Generic;

namespace LuviKunG
{
    public class Pool<T> where T : Object, IPool
    {
        public delegate T PoolEvent(T item);

        public Pool(T prefab)
        {
            this.prefab = prefab;
            this.transform = null;
            list = new List<T>();
        }

        public Pool(T prefab, Transform transform)
        {
            this.prefab = prefab;
            this.transform = transform;
            list = new List<T>();
        }

        public PoolEvent onInstantiate;

        private T prefab;
        private Transform transform;
        private List<T> list;

        public T Pick()
        {
            for (int i = 0; i < list.Count; i++)
                if (!list[i].isActive)
                    return list[i];
            return Instantiate();
        }

        public List<T> Instantiate(int amount)
        {
            if (!(amount > 0))
                throw new System.InvalidOperationException("Cannot instantiate member because your amount is invalid.");
            List<T> list = new List<T>();
            for (int i = 0; i < amount; i++)
                list.Add(Instantiate());
            return list;
        }

        public T Instantiate()
        {
            T instant = Object.Instantiate(prefab, transform);
            if (onInstantiate != null)
                instant = onInstantiate(instant);
            list.Add(instant);
            return instant;
        }

        public void SetAll(bool isActive)
        {
            for (int i = 0; i < list.Count; i++)
                list[i].isActive = isActive;
        }

        public List<T> GetAll(bool isActive)
        {
            List<T> activeList = new List<T>();
            for (int i = 0; i < list.Count; i++)
                if (list[i].isActive == isActive)
                    activeList.Add(list[i]);
            return activeList;
        }
    }

    public interface IPool
    {
        bool isActive { get; set; }
    }
}