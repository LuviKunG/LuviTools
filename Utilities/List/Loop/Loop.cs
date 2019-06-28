using System;
using System.Collections.Generic;

[Serializable]
public sealed class Loop<T> : List<T>
{
    private int index;
    public int Index { get => index; }
    public T Current { get => this[index]; }
    public T Next { get { index++; if (index > Count - 1) index = 0; return Current; } }
    public T Prev { get { index--; if (index < 0) index = Count - 1; return Current; } }
    public Loop() : base() { }
    public Loop(IEnumerable<T> _list) : base(_list) { }
}