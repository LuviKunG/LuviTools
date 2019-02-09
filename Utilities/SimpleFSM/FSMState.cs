using UnityEngine;

namespace LuviKunG.FSM
{
    public abstract class FSMState : MonoBehaviour
    {
        protected FSMBehaviour behaviour;
        public void RegisterOwner(FSMBehaviour fsm) { behaviour = fsm; }
        public abstract void OnEnter();
        public abstract void OnUpdate();
        public abstract void OnExit();
    }
}