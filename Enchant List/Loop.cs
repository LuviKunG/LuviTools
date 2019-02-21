using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loop<T> : List<T>
{
    int index;

    public Loop()
    {
        index = -1;
    }

    public Loop(IEnumerable<T> collection) : base(collection)
    {
        index = -1;
    }

    public Loop(int capacity) : base(capacity)
    {
        index = -1;
    }

    public T GetCurrent()
    {
        if (Count > 0)
        {
            return this[index];
        }
        else
        {
            return default(T);
        }
    }

    public T GetPrev()
    {
        if (Count > 0)
        {
            index--;
            if (index < 0)
                index = Count - 1;
            return this[index];
        }
        else
        {
            return default(T);
        }
    }

    public T GetNext()
    {
        if (Count > 0)
        {
            index++;
            if (index > Count - 1)
                index = 0;
            return this[index];
        }
        else
        {
            return default(T);
        }
    }
}