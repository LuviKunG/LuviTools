using System.Collections.Generic;
using System.Text;
using UnityRandom = UnityEngine.Random;

public static class IListExtension
{
    /// <summary>
    /// Shuffles the element order of the specified list.
    /// </summary>
    public static void Shuffle<T>(this IList<T> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityRandom.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }

    /// <summary>
    /// Get combination of count.
    /// </summary>
    /// <param name="list">List to get combination.</param>
    /// <param name="sample">Sample count of comibnation.</param>
    /// <returns>List of combination set.</returns>
    public static List<List<T>> Combination<T>(this IList<T> list, int sample)
    {
        List<List<T>> g = new List<List<T>>();
        (T m, bool p, int w)[] e = new (T m, bool p, int w)[list.Count];
        bool end = false;
        bool lp = false;
        bool ws = false;
        int cw = 1;
        int extw = sample;
        int cwc = cw;
        for (int i = 0; i < list.Count; i++)
            e[i] = (list[i], extw > 0, extw > 0 ? extw-- : 0);
        List<T> gf = new List<T>();
        for (int i = 0; i < e.Length; i++)
            if (e[i].p)
                gf.Add(e[i].m);
        g.Add(gf);
        do
        {
            end = false;
            lp = false;
            cw = 1;
            while (e[e.Length - cw].p && e[e.Length - cw].w == cw)
            {
                lp = true;
                cw++;
                if (cw > sample)
                {
                    end = true;
                    break;
                }
            }
            if (end)
                break;
            if (lp)
            {
                ws = false;
                cwc = cw;
                for (int i = 0; i < e.Length; i++)
                {
                    if (ws)
                    {
                        if (cwc > 0)
                        {
                            e[i].w = cwc;
                            e[i].p = true;
                            cwc--;
                        }
                        else
                        {
                            e[i].w = 0;
                            e[i].p = false;
                        }
                    }
                    else if (i < e.Length - 1 && e[i].w == cw)
                    {
                        ws = true;
                        (T m, bool p, int w) tmp = e[i];
                        e[i].p = e[i + 1].p;
                        e[i].w = e[i + 1].w;
                        e[i + 1].p = tmp.p;
                        e[i + 1].w = tmp.w;
                    }
                }
            }
            else
            {
                for (int i = e.Length - 1; i > 0; i--)
                    if (e[i - 1].p)
                    {
                        (T m, bool p, int w) tmp = e[i];
                        e[i].p = e[i - 1].p;
                        e[i].w = e[i - 1].w;
                        e[i - 1].p = tmp.p;
                        e[i - 1].w = tmp.w;
                        break;
                    }
            }
            List<T> gm = new List<T>();
            for (int i = 0; i < e.Length; i++)
                if (e[i].p)
                    gm.Add(e[i].m);
            g.Add(gm);
        }
        while (true);
        return g;
    }
}