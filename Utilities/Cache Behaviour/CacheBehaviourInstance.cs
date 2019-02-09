using UnityEngine;

namespace LuviKunG
{
    /// <summary>
    /// Similar of CacheBehaviour, but able to have instance.
    /// It can't inherit and need to be sealed.
    /// </summary>
    /// <typeparam name="T">MonoBehaviour component</typeparam>
    public class CacheBehaviourInstance<T> : CacheBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        /// <summary>
        /// Return instance of this class. Even it's null
        /// </summary>
		public static T Instanced
        {
            get
            {
                return _instance;
            }
            private set
            {
                _instance = value;
            }
        }
        /// <summary>
        /// Return instance of this class. If it's null, it will create new instance.
        /// </summary>
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                    if (_instance == null)
                    {
                        GameObject obj = new GameObject();
                        _instance = obj.AddComponent<T>();
						_instance.name = typeof(T).ToString();
                    }            
                }
                return _instance;
            }
            private set
            {
                _instance = value;
            }
        }
    }
}