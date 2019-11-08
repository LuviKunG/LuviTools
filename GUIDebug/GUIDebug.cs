using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace LuviKunG.Debug
{
    public class GUIDebug : MonoBehaviour
    {
        public bool isShow;

        [SerializeField]
        private int m_logCapacity = 32;
        public int logCapacity
        {
            get => m_logCapacity;
            set
            {
                m_logCapacity = value;
                UpdateLogCapacity();
            }
        }

        private List<string> logs = new List<string>();
        private StringBuilder cacheSB = new StringBuilder();
        private GUISkin skin;
        private GUIStyle styleLabel;
        private int indexLog;

        private void Awake()
        {
            skin = ScriptableObject.CreateInstance<GUISkin>();
            styleLabel = skin.label;
            styleLabel.padding = new RectOffset(0, 0, 0, 0);
            styleLabel.wordWrap = true;
        }

        private void OnEnable()
        {
            Application.logMessageReceivedThreaded += LogReceiveCallback;
        }

        private void OnDisable()
        {
            Application.logMessageReceivedThreaded -= LogReceiveCallback;
        }

        private void LogReceiveCallback(string condition, string stackTrace, LogType type)
        {
            cacheSB.Clear();
            switch (type)
            {
                case LogType.Warning:
                    {
                        cacheSB.Append("<color=yellow><i>Warning: </i>");
                        cacheSB.Append(condition);
                        cacheSB.Append("</color>");
                    }
                    break;
                case LogType.Error:
                    {
                        cacheSB.Append("<color=red><b>Error: </b>");
                        cacheSB.Append(condition);
                        cacheSB.Append("</color>");
                    }
                    break;
                case LogType.Exception:
                    {
                        cacheSB.Append("<color=red><b>Exception: </b>");
                        cacheSB.Append(condition);
                        cacheSB.Append(stackTrace);
                        cacheSB.Append("</color>");
                    }
                    break;
                default:
                    {
                        cacheSB.Append("<color=white>");
                        cacheSB.Append(condition);
                        cacheSB.Append("</color>");
                    }
                    break;
            }
            logs.Add(cacheSB.ToString());
            UpdateLogCapacity();
        }

        private void UpdateLogCapacity()
        {
            while (logs.Count > m_logCapacity)
                logs.RemoveAt(0);
        }

        private void OnGUI()
        {
            if (isShow)
            {
                using (var verticalScope = new GUILayout.VerticalScope())
                {
                    for (indexLog = logs.Count - 1; indexLog >= 0; indexLog--)
                    {
                        GUILayout.Label(logs[indexLog], styleLabel);
                    }
                }
            }
        }
    }
}