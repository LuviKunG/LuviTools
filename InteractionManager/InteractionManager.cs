using System.Collections.Generic;
using UnityEngine;

namespace LuviKunG
{
    /// <summary>
    /// Manager class that knowing what interaction is doing or delaying.
    /// Using with <see cref="IsAllow" /> to check what interaction is allowed or not.
    /// </summary>
    public class InteractionManager : MonoBehaviour
    {
        /// <summary>
        /// Event class that holding event name.
        /// </summary>
        private class InteractiveEvent : IPool, IResetable
        {
            public string eventName;
            public bool isActive { get; private set; }

            public InteractiveEvent()
            {
                Reset();
            }

            /// <summary>
            /// Set event name.
            /// </summary>
            /// <param name="eventName">Event name</param>
            public void Set(string eventName)
            {
                this.eventName = eventName;
                isActive = true;
            }

            /// <summary>
            /// Reset event.
            /// </summary>
            public void Reset()
            {
                eventName = null;
                isActive = false;
            }
        }

        /// <summary>
        /// Event class that holding time.
        /// </summary>
        private class InteractiveDelay : IPool, IResetable
        {
            public float second;
            public bool isActive { get; private set; }

            public InteractiveDelay()
            {
                Reset();
            }

            /// <summary>
            /// Set event delay.
            /// </summary>
            /// <param name="second">Second to delay.</param>
            public void Set(float second)
            {
                this.second = second;
                isActive = true;
            }

            /// <summary>
            /// Reset event.
            /// </summary>
            public void Reset()
            {
                second = 0;
                isActive = false;
            }
        }

        /// <summary>
        /// (Private) Instance.
        /// </summary>
        private static InteractionManager m_instance;

        /// <summary>
        /// Instance of this class.
        /// This should be only one in runtime (as singleton).
        /// </summary>
        public static InteractionManager Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = CreateInstance();
                return m_instance;
            }
        }

        /// <summary>
        /// Create new instance of this manager.
        /// </summary>
        /// <returns>New instance</returns>
        private static InteractionManager CreateInstance()
        {
            GameObject gameObject = new GameObject(nameof(InteractionManager));
            InteractionManager instance = gameObject.AddComponent<InteractionManager>();
            return instance;
        }

        private Pool<InteractiveDelay> m_poolDelay;
        private List<InteractiveDelay> m_activeDelay;

        private Pool<InteractiveEvent> m_poolEvent;
        private List<InteractiveEvent> m_activeEvent;

        /// <summary>
        /// (MonoBehaviour) Awake.
        /// </summary>
        private void Awake()
        {
            if (m_instance == null)
                m_instance = this;
            else if (m_instance != this)
            {
                Debug.LogWarning($"Duplication instance of {nameof(InteractionManager)} has awaken, the latest one will be destroyed.");
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
            InternalAwake();
        }

        /// <summary>
        /// (MonoBehaviour) Update.
        /// </summary>
        private void Update()
        {
            float deltaTime = Time.deltaTime;
            InternalUpdate(deltaTime);
        }

        /// <summary>
        /// Awake and initialize this class.
        /// This should be called only once.
        /// (Constructive method)
        /// </summary>
        private void InternalAwake()
        {
            m_poolDelay = new Pool<InteractiveDelay>(OnCreateInteractiveDelay);
            m_activeDelay = new List<InteractiveDelay>();
            m_poolEvent = new Pool<InteractiveEvent>(OnCreateInteractiveEvent);
            m_activeEvent = new List<InteractiveEvent>();
        }

        /// <summary>
        /// Update of this class.
        /// This should be called in loop update by framerate.
        /// </summary>
        /// <param name="deltaTime">Delta time of this frame. (Unity framerate)</param>
        private void InternalUpdate(in float deltaTime)
        {
            for (int i = 0; i < m_activeDelay.Count; i++)
            {
                m_activeDelay[i].second -= deltaTime;
                if (m_activeDelay[i].second < 0)
                {
                    m_activeDelay[i].Reset();
                    m_activeDelay.RemoveAt(i--);
                }
            }
        }

        /// <summary>
        /// Add new active interaction event.
        /// </summary>
        /// <param name="eventName">Event name</param>
        /// <returns>Is new event?</returns>
        public bool AddEvent(string eventName)
        {
            if (!HasEvent(eventName))
            {
                var interactive = m_poolEvent.Pick();
                interactive.Set(eventName);
                m_activeEvent.Add(interactive);
                return true;
            }
            else return false;
        }

        /// <summary>
        /// Remove active interaction event.
        /// </summary>
        /// <param name="eventName">Event name</param>
        /// <returns>Is removed? or event doesn't exist.</returns>
        public bool RemoveEvent(string eventName)
        {
            for (int i = 0; i < m_activeEvent.Count; i++)
            {
                if (m_activeEvent[i].eventName == eventName)
                {
                    m_activeEvent[i].Reset();
                    m_activeEvent.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Is interaction event active?
        /// </summary>
        /// <param name="eventName">Event name</param>
        /// <returns>Is active? or not found.</returns>
        public bool HasEvent(string eventName)
        {
            for (int i = 0; i < m_activeEvent.Count; i++)
                if (m_activeEvent[i].eventName == eventName)
                    return true;
            return false;
        }

        /// <summary>
        /// Add interaction delay.
        /// </summary>
        /// <param name="second">Delay in second</param>
        public void AddDelay(float second)
        {
            var interactive = m_poolDelay.Pick();
            interactive.Set(second);
            m_activeDelay.Add(interactive);
        }

        /// <summary>
        /// Clear all active delay and event.
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < m_activeEvent.Count; i++)
                m_activeEvent[i].Reset();
            m_activeEvent.Clear();
            for (int i = 0; i < m_activeDelay.Count; i++)
                m_activeDelay[i].Reset();
            m_activeDelay.Clear();
        }

        /// <summary>
        /// Allow for interaction?
        /// </summary>
        /// <returns>True if is allowed</returns>
        public bool IsAllow()
        {
            return m_activeDelay.Count == 0 && m_activeEvent.Count == 0;
        }

        /// <summary>
        /// Get max delay in second that still active.
        /// </summary>
        /// <returns>Delay in second</returns>
        public float GetMaxDelay()
        {
            float maxDelay = 0;
            if (m_activeDelay.Count > 0)
                for (int i = 0; i < m_activeDelay.Count; i++)
                    if (m_activeDelay[i].second > maxDelay)
                        maxDelay = m_activeDelay[i].second;
            return maxDelay;
        }

        /// <summary>
        /// (Callback) When pooling create <see cref="InteractiveDelay" /> class.
        /// </summary>
        /// <returns>New <see cref="InteractiveDelay" /></returns>
        private InteractiveDelay OnCreateInteractiveDelay()
        {
            return new InteractiveDelay();
        }

        /// <summary>
        /// (Callback) When pooling create <see cref="InteractiveEvent" /> class.
        /// </summary>
        /// <returns>New <see cref="InteractiveEvent" /></returns>
        private InteractiveEvent OnCreateInteractiveEvent()
        {
            return new InteractiveEvent();
        }

        /// <summary>
        /// Get information as string for using in log.
        /// </summary>
        /// <returns>Information string.</returns>
        public override string ToString()
        {
            string listEvent = string.Empty;
            if (m_activeEvent.Count > 0)
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                for (int i = 0; i < m_activeEvent.Count; i++)
                {
                    if (i > 0) sb.Append(',');
                    sb.Append(m_activeEvent[i].eventName);
                }
                listEvent = sb.ToString();
            }
            return $"{{delay:{{count:{m_activeDelay.Count},max:{GetMaxDelay()}}},event:{{count:{m_activeEvent.Count},list:[{listEvent}]}}}}";
        }
    }
}