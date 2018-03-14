using UnityEngine;

namespace LuviKunG
{
    /// <summary>
    /// This class will decrease process of internal GetComponent<Transform> and GetComponent<GameObject>.
    /// It's have issue when component are move to new GameObject, bacause it's still return cached. Use wisely.
    /// </summary>
    public class CacheBehaviour : MonoBehaviour
    {
        private Transform _transform;
        private GameObject _gameObject;
        /// <summary>
        /// Return cached Transform.
        /// </summary>
        public new Transform transform
        {
            get
            {
                if (_transform == null)
                    _transform = base.transform;
                return _transform;
            }
        }
        /// <summary>
        /// Return cached Game Object.
        /// </summary>
        public new GameObject gameObject
        {
            get
            {
                if (_gameObject == null)
                    _gameObject = base.gameObject;
                return _gameObject;
            }
        }
    }
}