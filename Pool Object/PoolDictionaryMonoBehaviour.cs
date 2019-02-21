using System.Collections.Generic;
using UnityEngine;

namespace LuviKunG.Pool
{
    public sealed class PoolDictionaryMonoBehaviour<TKey, TMonoBehaviour> where TMonoBehaviour : MonoBehaviour
    {
        private Dictionary<TKey, TMonoBehaviour> dict = new Dictionary<TKey, TMonoBehaviour>();

        public void Add(TKey name, TMonoBehaviour obj)
        {
            dict.Add(name, obj);
        }

        public bool Remove(TKey name)
        {
            if (dict.ContainsKey(name))
            {
                dict.Remove(name);
                return true;
            }
            else
                return false;
        }

        public TMonoBehaviour Get(TKey name)
        {
            return dict.ContainsKey(name) ? dict[name] : null;
        }

        public bool Has(TKey name)
        {
            return dict.ContainsKey(name);
        }
    }
}