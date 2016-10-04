using UnityEngine;

namespace LuviKunG
{
    public class CacheBehaviourInstance<T> : CacheBehaviour where T : CacheBehaviour
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = GameObject.FindObjectOfType<T>();
                    if (instance == null)
                    {
                        GameObject obj = new GameObject();
                        instance = obj.AddComponent<T>();
                    }
                    instance.name = typeof(T).ToString();
                }
                return instance;
            }
            private set
            {
                instance = value;
            }
        }
    }
}