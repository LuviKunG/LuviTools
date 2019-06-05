using System;
using System.Collections.Generic;
using System.Text;

public class Gacha<T>
{
    private List<GachaElement> list;
    private Random random;

    public Gacha()
    {
        list = new List<GachaElement>();
        random = new Random();
    }

    public Gacha(int seed)
    {
        list = new List<GachaElement>();
        random = new Random(seed);
    }

    public class GachaElement
    {
        public T item;
        public float weight;
        public float minPercentage;
        public float maxPercentage;

        public GachaElement(float weight, T item)
        {
            SetItem(item);
            SetWeight(weight);
        }

        public bool Jackpot(float chance)
        {
            if (chance < 0 || chance > 1f)
                throw new InvalidCastException("chance is lower than 0 or higher than 1");
            return chance >= minPercentage && chance < maxPercentage;
        }

        public void SetItem(T item)
        {
            this.item = item;
        }

        public void SetWeight(float weight)
        {
            this.weight = weight;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"item\":");
            sb.Append(item.ToString());
            sb.Append(",\"weight\":");
            sb.Append(weight);
            sb.Append(",\"minPercentage\":");
            sb.Append(minPercentage);
            sb.Append(",\"maxPercentage\":");
            sb.Append(maxPercentage);
            sb.Append("}");
            return sb.ToString();
        }
    }

    public void Add(float weight, T item, bool roll = false)
    {
        list = list ?? new List<GachaElement>();
        list.Add(new GachaElement(weight, item));
        if (roll)
            Roll();
    }

    public void Clear()
    {
        if (list == null)
            return;
        list.Clear();
    }

    public void Roll()
    {
        if (list == null)
            return;
        float overallWeight = 0;
        float percent = 0;
        for (int i = 0; i < list.Count; i++)
        {
            overallWeight += list[i].weight;
        }
        for (int i = 0; i < list.Count; i++)
        {
            list[i].minPercentage = percent;
            percent = percent + (list[i].weight / overallWeight);
            list[i].maxPercentage = percent;
        }
    }

    public T Pok()
    {
        float percent = Convert.ToSingle(random.NextDouble());
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].Jackpot(percent))
                return list[i].item;
        }
        throw new InvalidOperationException("No items return.");
    }

    public List<T> Poks(int count)
    {
        if (count <= 0)
            throw new InvalidCastException("Length is less or equal than 0");
        List<T> poks = new List<T>();
        for (int n = 0; n < count; n++)
        {
            bool found = false;
            float chance = Convert.ToSingle(random.NextDouble());
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Jackpot(chance))
                {
                    poks.Add(list[i].item);
                    found = true;
                    break;
                }
            }
            if (!found)
                throw new InvalidOperationException("no items return.");
        }
        return poks;
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("[");
        for (int i = 0; i < list.Count - 1; i++)
        {
            sb.Append(list[i].ToString());
            sb.Append(",");
        }
        sb.Append(list[list.Count - 1].ToString());
        sb.Append("]");
        return sb.ToString();
    }
}