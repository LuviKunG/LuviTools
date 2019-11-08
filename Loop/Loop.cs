using System.Collections.Generic;

namespace LuviKunG
{
    public sealed class Loop<T>
    {
        private List<T> list;
        public int currentIndex;

        public Loop(IEnumerable<T> list)
        {
            this.list = new List<T>(list);
            currentIndex = 0;
        }
        public void Add(T item)
        {
            list.Add(item);
        }

        public void Clear()
        {
            list.Clear();
        }

        public bool Remove(T item)
        {
            return list.Remove(item);
        }

        public T Current()
        {
            return list[currentIndex];
        }

        public T Prev()
        {
            currentIndex--;
            if (currentIndex < 0)
                currentIndex = list.Count - 1;
            return list[currentIndex];
        }

        public T Next()
        {
            currentIndex++;
            if (currentIndex > list.Count - 1)
                currentIndex = 0;
            return list[currentIndex];
        }
    }
}