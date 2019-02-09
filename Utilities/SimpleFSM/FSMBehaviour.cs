using UnityEngine;

namespace LuviKunG.FSM
{
    public sealed class FSMBehaviour : MonoBehaviour
    {
        public bool playOnStart;
        public FSMState startState;
        private FSMState current;
        private void Start()
        {
            if (playOnStart && startState != null)
                SetNextState(startState);
            else
                enabled = false;
        }

        private void Update()
        {
            current.OnUpdate();
        }

        public void Restart()
        {
            if (startState != null)
                SetNextState(startState);
        }

        public void Stop()
        {
            if (current != null)
                current.OnExit();
            current = null;
            enabled = false;
        }

        public void SetNextState(FSMState state)
        {
            if (current != null)
                current.OnExit();
            if (state != null)
            {
                current = state;
                current.RegisterOwner(this);
                current.OnEnter();
                enabled = true;
            }
            else
            {
                enabled = false;
            }
        }
    }
}