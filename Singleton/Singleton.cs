using UnityEngine;

namespace LuviKunG
{
    public class Singleton<T> where T : MonoBehaviour
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                return instance;
            }
            set
            {
                instance = value;
            }
        }
    }
}