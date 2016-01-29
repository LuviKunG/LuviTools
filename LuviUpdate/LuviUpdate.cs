using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace LuviKunG.Update
{
    public class LuviUpdate : LuviBehaviour
    {
        public static LuviUpdate Instance;

        List<LuviUpdateElement> updateOrder = new List<LuviUpdateElement>();

        void Awake()
        {
            LuviUpdate.Instance = this;
        }

        int index;
        IEnumerator Start()
        {
            while (true)
            {
                for (index = 0; index < updateOrder.Count; index++)
                {
                    updateOrder[index].component.LuviUpdateHandler();
                }
                yield return null;
            }
        }

        #if UNITY_EDITOR
        void Reset()
        {
            name = "LuviUpdate";
        }
        #endif

        public void AddUpdate(int order, ILuviUpdate component)
        {
            updateOrder.Add(new LuviUpdateElement(order, component));
            SortUpdate();
        }

        public void SortUpdate()
        {
            updateOrder.Sort(SortUpdateComparer);
        }

        int SortUpdateComparer(LuviUpdateElement a, LuviUpdateElement b)
        {
            if (a.order < b.order)
                return -1;
            else if (a.order > b.order)
                return 1;
            else
                return 0;
        }
    }

    public class LuviUpdateElement
    {
        public int order;
        public ILuviUpdate component;

        public LuviUpdateElement(int order, ILuviUpdate component)
        {
            this.order = order;
            this.component = component;
        }
    }
}