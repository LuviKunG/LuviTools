using System.Collections.Generic;
using UnityEngine;

namespace LuviKunG.UI
{
    [DisallowMultipleComponent]
    public class UserInterfaceManager : MonoBehaviour
    {
        private static readonly string NAME = typeof(UserInterfaceManager).Name;

        private static UserInterfaceManager instance;
        public static UserInterfaceManager Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject go = new GameObject(NAME);
                    instance = go.AddComponent<UserInterfaceManager>();
                }
                return instance;
            }
        }

        public bool escapePause;

        public IUserInterface focus
        {
            get
            {
                ClearNullReference();
                if (listFocus.Count > 0)
                    return listFocus[listFocus.Count - 1];
                else
                    return null;
            }
        }

        private IUserInterface cacheFocus;
        private List<IUserInterface> listFocus;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
            {
                Debug.LogError($"There are 2 or more \'{NAME}\' on the scene. The new instance will be destroy.");
                Destroy(this);
                return;
            }
            escapePause = false;
            cacheFocus = null;
            listFocus = new List<IUserInterface>();
            DontDestroyOnLoad(gameObject);
        }

        private void ClearNullReference()
        {
            for (int i = 0; i < listFocus.Count; i++)
                if (listFocus[i] == null)
                    listFocus.RemoveAt(i--);
        }

        public bool HasUI(IUserInterface ui)
        {
            ClearNullReference();
            for (int i = 0; i < listFocus.Count; i++)
                if (listFocus[i] == ui)
                    return true;
            return false;
        }

        public void Active(IUserInterface ui)
        {
            ClearNullReference();
            if (HasUI(ui))
                Deactive(ui);
            listFocus.Add(ui);
            UpdateFocus();
        }

        public bool Deactive(IUserInterface ui)
        {
            ClearNullReference();
            bool isRemoved = listFocus.Remove(ui);
            UpdateFocus();
            return isRemoved;
        }

        private void Update()
        {
            if (escapePause) return;
            if (Input.GetKeyDown(KeyCode.Escape))
                focus?.OnEscape();
        }

        private void UpdateFocus()
        {
            if (cacheFocus != focus)
            {
                cacheFocus?.OnUnfocused();
                cacheFocus = focus;
                cacheFocus?.OnFocused();
            }
        }
    }
}