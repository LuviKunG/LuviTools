using System;
using System.Collections.Generic;

namespace LuviKunG
{
    [Serializable]
    public class LoopList<T> : List<T>
    {
        protected int index;
        int Index { get { return index; } set { index = value % Count; } }
        public T GetCurrent() { return this[Index]; }
        public T GetNext() { return this[Index++]; }
        public LoopList() : base() { }
        public LoopList(IEnumerable<T> _list) : base(_list) { }
    }
}