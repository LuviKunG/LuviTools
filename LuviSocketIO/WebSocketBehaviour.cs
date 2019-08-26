using UnityEngine;
using System.Collections.Generic;

namespace LuviKunG.Web.Socket
{
    public class WebSocketBehaviour : MonoBehaviour
    {
        public delegate void DispatchAction();

        private static WebSocketBehaviour instance = default;
        public static WebSocketBehaviour Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject go = new GameObject(nameof(WebSocketBehaviour));
                    instance = go.AddComponent<WebSocketBehaviour>();
                    DontDestroyOnLoad(go);
                }
                return instance;
            }
        }

        public List<DispatchAction> actions;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (instance != this)
            {
                Debug.LogError($"Two or more instance of '{nameof(WebSocketBehaviour)}' has been found. The new one will be destroyed.");
                Destroy(this);
            }
            Initialize();
        }

        private void Initialize()
        {
            Debug.Log($"{nameof(WebSocketBehaviour)} is initialize.");
            actions = actions ?? new List<DispatchAction>();
            actions.Clear();
        }

        private void Update()
        {
            for (int i = 0; i < actions.Count; i++)
            {
                actions[i]?.Invoke();
                actions.RemoveAt(i--);
            }
        }

        public void AddAction(DispatchAction action)
        {
            actions.Add(action);
        }
    }
}