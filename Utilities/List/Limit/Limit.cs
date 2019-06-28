using System;
using System.Collections.Generic;

[Serializable]
public sealed class Limit<T> : List<T>
{
    private int limitCapacity;
    int LimitCapacity
    {
        get { return limitCapacity; }
        set { limitCapacity = value; Delimit(); }
    }
    public Limit(int limit) : base()
    {
        limitCapacity = limit;
    }
    public Limit(int limit, IEnumerable<T> collection) : base(collection)
    {
        limitCapacity = limit;
        Delimit();
    }
    public Limit(int limit, int capacity) : base(capacity)
    {
        if (limit <= capacity)
            limitCapacity = limit;
        else
            limitCapacity = capacity;
        Delimit();
    }
    public new void Add(T t)
    {
        base.Add(t);
        Delimit();
    }
    private void Delimit()
    {
        while (Count > limitCapacity)
        {
            RemoveAt(0);
        }
    }
}