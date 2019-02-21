using BitStrap;
using System;
using System.Text;
using System.Collections.Generic;

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
        if (list == null)
            list = new List<GachaElement>();
        list.Add(new GachaElement(weight, item));
        if (roll)
            Roll();
    }

    public void Roll()
    {
        float overallWeight = 0;
        float percent = 0;
        foreach (GachaElement element in list.Each())
        {
            overallWeight += element.weight;
        }
        foreach (GachaElement element in list.Each())
        {
            element.minPercentage = percent;
            percent = percent + (element.weight / overallWeight);
            element.maxPercentage = percent;
        }
    }

    public T Pok()
    {
        float percent = Convert.ToSingle(random.NextDouble());
        foreach (GachaElement element in list.Each())
        {
            if (element.Jackpot(percent))
                return element.item;
        }
        throw new InvalidOperationException("no items return.");
    }

    public List<T> Poks(int count)
    {
        if (count <= 0)
            throw new InvalidCastException("length is less or equal than 0");
        List<T> poks = new List<T>();
        for (int i = 0; i < count; i++)
        {
            bool found = false;
            float chance = Convert.ToSingle(random.NextDouble());
            foreach (GachaElement element in list.Each())
            {
                if (element.Jackpot(chance))
                {
                    poks.Add(element.item);
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