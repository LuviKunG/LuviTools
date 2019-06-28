using System;
using System.Collections.Generic;

namespace LuviKunG
{
    [Serializable]
    public class LimitList<T> : List<T>
    {
        protected int limit;
        int Limit
        {
            get { return limit; }
            set { limit = value; }
        }
        public LimitList(int _limit) : base()
        {
            limit = _limit;
        }
        public LimitList(int _limit, IEnumerable<T> _list) : base(_list)
        {
            limit = _limit;
            Delimit();
        }
        public new void Add(T t)
        {
            base.Add(t);
            Delimit();
        }
        void Delimit()
        {
            while (Count > limit)
            {
                RemoveAt(0);
            }
        }
    }
}