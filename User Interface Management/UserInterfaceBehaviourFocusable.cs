namespace LuviKunG.UI
{
    public abstract class UserInterfaceBehaviourFocusable : UserInterfaceBehaviour, IUserInterface
    {
        protected UserInterfaceManager uiManager;

        protected virtual void Awake() { uiManager = UserInterfaceManager.Instance; }
        protected virtual void OnEnable() { uiManager.Active(this); }
        protected virtual void OnDisable() { uiManager.Deactive(this); }

        public virtual void OnUnfocused() { }
        public virtual void OnFocused() { }
        public virtual void OnEscape() { }
    }
}