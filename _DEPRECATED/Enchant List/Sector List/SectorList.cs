using System.Collections.Generic;

namespace LuviKunG
{
    public class SectorList<T> : List<T>
    {
        int index;
        public bool Next { get { return index < Count; } }
        public void Reset() { index = 0; }
        public T GetNext() { return this[index++]; }
    }
}