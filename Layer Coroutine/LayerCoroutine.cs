using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LuviKunG
{
    public sealed class LayerCoroutine
    {
        private MonoBehaviour behaviour;
        private List<Coroutine> stack;

        public int Stack => stack.Count;

        public LayerCoroutine(MonoBehaviour behaviour)
        {
            this.behaviour = behaviour;
            stack = new List<Coroutine>();
        }

        public Coroutine StartCoroutine(IEnumerator enumerator)
        {
            Coroutine routine = behaviour.StartCoroutine(enumerator);
            stack.Add(routine);
            return routine;
        }

        public void StopCoroutine(Coroutine coroutine)
        {
            for (int i = 0; i < stack.Count; i++)
                if (stack[i] == coroutine)
                {
                    behaviour.StopCoroutine(stack[i]);
                    stack.RemoveAt(i);
                    return;
                }
        }

        public void StopAllCoroutine()
        {
            for (int i = 0; i < stack.Count; i++)
                behaviour.StopCoroutine(stack[i]);
            stack.Clear();
        }
    }
}