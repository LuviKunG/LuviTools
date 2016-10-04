using UnityEngine;
using System.Collections;

namespace LuviKunG
{
    public class CacheBehaviour : MonoBehaviour
    {
        protected Transform _transform;
        protected GameObject _gameObject;
        public new Transform transform
        {
            get
            {
                if (_transform == null)
                    _transform = base.transform;
                return _transform;
            }
        }
        public new GameObject gameObject
        {
            get
            {
                if (_gameObject == null)
                    _gameObject = base.gameObject;
                return _gameObject;
            }
        }
        public Coroutine Coroutine(ref Coroutine coroutine, IEnumerator routine)
        {
            if (coroutine != null) StopCoroutine(coroutine);
            coroutine = StartCoroutine(routine);
            return coroutine;
        }
    }
}