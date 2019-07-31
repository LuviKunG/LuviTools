using System;
using System.Collections.Generic;

[Serializable]
public sealed class Loop<T> : List<T>
{
    private int m_index;
    public int index
    {
        get => m_index;
        set
        {
            m_index = value;
            if (m_index < 0)
                m_index = 0;
            if (m_index > Count - 1)
                m_index = Count - 1;
        }
    }
    public T Current { get => this[m_index]; }
    public T Next { get { m_index++; if (m_index > Count - 1) m_index = 0; return Current; } }
    public T Prev { get { m_index--; if (m_index < 0) m_index = Count - 1; return Current; } }
    public Loop() : base() { m_index = -1; }
    public Loop(IEnumerable<T> _list) : base(_list) { m_index = -1; }
    public new void Clear()
    {
        base.Clear();
        m_index = -1;
    }
}