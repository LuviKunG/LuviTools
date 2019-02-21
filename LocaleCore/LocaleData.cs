using UnityEngine;

namespace LuviKunG.LocaleCore
{
    public class LocaleData : ScriptableObject
    {
        // Edit this for default undefined locale key
        protected const string UNDEFINED = "##UNDEFINED##";

        public LocaleKey[] list;

        protected int searchIndex;
        public virtual string Get(string key)
        {
            for (searchIndex = 0; searchIndex < list.Length; searchIndex++)
            {
                if (list[searchIndex].Key == key)
                    return list[searchIndex].Value;
            }
            return UNDEFINED;
        }

        public bool Has(string key)
        {
            for (searchIndex = 0; searchIndex < list.Length; searchIndex++)
            {
                if (list[searchIndex].Key == key)
                    return true;
            }
            return false;
        }
    }
}