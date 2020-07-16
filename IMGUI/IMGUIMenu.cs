using System;
using System.Collections.Generic;
using UnityEngine;

namespace LuviKunG.IMGUI
{
    public abstract class IMGUIMenu : MonoBehaviour
    {
        private const float MENU_SIZE = 200.0f;
        private const float LAYOUT_SIZE = 160.0f;

        [Serializable]
        public class Command
        {
            public string menuName;
            public Action onSelect;
            public Action onGUI;

            public Command(string menuName, Action onSelect, Action onGUI)
            {
                this.menuName = menuName;
                this.onSelect = onSelect;
                this.onGUI = onGUI;
            }
        }

        private class FlexibleSpaceScope : IDisposable
        {
            public FlexibleSpaceScope()
            {
                GUILayout.FlexibleSpace();
            }

            public void Dispose()
            {
                GUILayout.FlexibleSpace();
            }
        }

        private class DisableScope : IDisposable
        {
            private bool cacheEnabled;

            public DisableScope(bool enabled)
            {
                cacheEnabled = GUI.enabled;
                GUI.enabled = enabled;
            }

            public void Dispose()
            {
                GUI.enabled = cacheEnabled;
            }
        }

        [SerializeField]
        protected bool m_isShow = default;

        protected List<Command> m_commands;
        protected Command m_currentCommand;
        protected Vector2 m_scrollMenuPos;
        protected Vector2 m_scrollLayoutPos;

        protected virtual void Awake()
        {
            m_commands = new List<Command>();
        }

        protected virtual void OnGUI()
        {
            if (GUI.Button(new Rect(Screen.width - 60.0f, Screen.height - 20.0f, 60.0f, 20.0f), m_isShow ? "Hide" : "Show"))
            {
                m_isShow = !m_isShow;
            }
            if (m_isShow)
            {
                using (var scopePanel = new GUILayout.HorizontalScope())
                {
                    using (var scopeScrollMenu = new GUILayout.ScrollViewScope(m_scrollMenuPos, GUILayout.Width(MENU_SIZE)))
                    {
                        m_scrollMenuPos = scopeScrollMenu.scrollPosition;
                        for (int i = 0; i < m_commands.Count; i++)
                            if (GUILayout.Button(m_commands[i].menuName))
                            {
                                m_currentCommand = m_commands[i];
                                m_currentCommand.onSelect?.Invoke();
                            }
                        GUILayout.Space(16.0f);
                        if (m_currentCommand != null)
                            if (GUILayout.Button("[Close Panel]"))
                                m_currentCommand = null;

                    }
                    using (var scopeLayout = new GUILayout.ScrollViewScope(m_scrollLayoutPos, GUILayout.Width(Screen.width - MENU_SIZE)))
                    {
                        if (m_currentCommand != null)
                            m_currentCommand.onGUI?.Invoke();
                    }
                }
            }
        }

        #region IMGUI Function
        protected IDisposable Layout()
        {
            return new GUILayout.HorizontalScope(GUI.skin.GetStyle("textarea"));
        }

        protected IDisposable Panel()
        {
            return new GUILayout.VerticalScope(GUI.skin.GetStyle("textarea"));
        }

        protected IDisposable Flex()
        {
            return new FlexibleSpaceScope();
        }

        protected IDisposable Condition(bool enabled)
        {
            return new DisableScope(enabled);
        }

        protected bool BigButton(string text)
        {
            return GUILayout.Button(text, GUILayout.ExpandWidth(true), GUILayout.Height(30.0f));
        }

        protected bool BigButton(string text, bool enabled = true)
        {
            bool isPressed = false;
            using (Condition(enabled))
            {
                isPressed = GUILayout.Button(text, GUILayout.ExpandWidth(true), GUILayout.Height(30.0f));
            }
            return isPressed;
        }

        protected void TextField(ref string target, string label = null)
        {
            if (!string.IsNullOrEmpty(label))
                GUILayout.Label(label, GUILayout.Width(LAYOUT_SIZE));
            target = GUILayout.TextField(target, GUILayout.ExpandWidth(true));
        }

        protected bool ButtonField(string text)
        {
            return GUILayout.Button(text, GUILayout.ExpandWidth(false));
        }

        protected bool ButtonField(string text, bool enabled = true)
        {
            bool isPressed = false;
            using (Condition(enabled))
            {
                isPressed = GUILayout.Button(text, GUILayout.ExpandWidth(false));
            }
            return isPressed;
        }
        #endregion
    }
}