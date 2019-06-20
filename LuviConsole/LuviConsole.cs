using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// LuviConsole 2.4.0
// https://github.com/LuviKunG

namespace LuviKunG
{
    [AddComponentMenu("LuviKunG/LuviConsole")]
    public class LuviConsole : MonoBehaviour
    {
        public class CommandData
        {
            public string name;
            public string description;
            public LuviCommandExecution execution;

            public CommandData(string name, string description, LuviCommandExecution execution)
            {
                this.name = name;
                this.description = description;
                this.execution = execution;
            }
        }

        private static LuviConsole instance;
        public static LuviConsole Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject obj = new GameObject("LuviConsole");
                    instance = obj.AddComponent<LuviConsole>();
                }
                return instance;
            }
        }

        [SerializeField]
        private GUISkin guiSkin;

        public int logCapacity = 64;
        public int excuteCapacity = 16;
        public float swipeRatio = 0.8f;
        public int defaultFontSize = 16;
        public bool autoShowWarning = false;
        public bool autoShowError = false;
        public bool autoShowException = false;

        private void Awake()
        {
            if (!instance) instance = this;
            Initialize();
        }

        private void OnEnable()
        {
            Application.logMessageReceivedThreaded += LogReceiveCallback;
        }
        private void OnDisable()
        {
            Application.logMessageReceivedThreaded -= LogReceiveCallback;
        }

        private void Update()
        {
            UpdateInput();
            if (isShowing)
                UpdateScrollDrag();
        }

#if UNITY_EDITOR || UNITY_EDITOR_OSX
        private void Reset()
        {
            if (guiSkin == null)
                guiSkin = Resources.Load<GUISkin>("LuviConsoleGUI");
            if (guiSkin != null)
                guiSkin.label.fontSize = defaultFontSize;
        }
#endif

        void OnGUI()
        {
            GUI.skin = guiSkin;
            if (isShowing)
            {
                UpdateWindow();
                ShowDebugWindow();
                ShowCommandWindow();
            }
        }

        private void Initialize()
        {
            if (guiSkin == null)
                guiSkin = Resources.Load<GUISkin>("LuviConsoleGUI");
            if (guiSkin != null)
                guiSkin.label.fontSize = defaultFontSize;
            DontDestroyOnLoad(gameObject);
            UpdateWindow();
        }

        private void UpdateInput()
        {
#if UNITY_EDITOR || UNITY_WEBGL
            if (Input.GetKeyDown(KeyCode.F1))
            {
                ToggleConsole();
                GUI.FocusControl("commandfield");
            }
#elif UNITY_EDITOR_OSX
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                ToggleConsole();
                GUI.FocusControl("commandfield");
            }
#elif UNITY_ANDROID || UNITY_IOS
            if (Input.GetMouseButtonDown(0))
            {
                _swipePosStart = Input.mousePosition;
            }
            if (Input.GetMouseButton(0) && _swipePosStart != Vector3.zero)
            {
                _swipePosMoving = Input.mousePosition;
                Vector3 direction = _swipePosMoving - _swipePosStart;
                if (Vector3.Distance(_swipePosStart, _swipePosMoving) > (Screen.height * swipeRatio))
                {
                    if (Mathf.Abs(direction.x) <= Mathf.Abs(direction.y))
                    {
                        if (direction.y > 0)
                        {
                            _swipePosStart = Vector3.zero;
                            _swipePosMoving = Vector3.zero;
                            isShowing = true;
                        }
                        else
                        {
                            _swipePosStart = Vector3.zero;
                            _swipePosMoving = Vector3.zero;
						    isShowing = false;
                        }
                    }
                }
            }
#elif UNITY_WEBGL
#endif
        }

        private void UpdateScrollDrag()
        {
            Vector2 currentPosition = Input.mousePosition;
            // Bacause Input.mousePosition is start at bottom-left, so we need to convert into the same as Rect that start at top-left.
            currentPosition.y = Screen.height - currentPosition.y;
            if (Input.GetMouseButtonDown(0) && rectDebugLogScrollDrag.Contains(currentPosition))
            {
                isScrollDebugDragging = true;
                scrollDebugDragPosition = currentPosition;
            }
            if (Input.GetMouseButtonUp(0))
            {
                isScrollDebugDragging = false;
            }
            if (isScrollDebugDragging)
            {
                Vector2 delta = scrollDebugDragPosition - currentPosition;
                scrollDebugPosition += delta;
                scrollDebugDragPosition = currentPosition;
            }
        }

        private readonly Vector2 scrollLockPosition = new Vector2(0, Mathf.Infinity);
        private Rect rectDebugLogBackground;
        private Rect rectDebugLogScroll;
        private Rect rectDebugLogScrollDrag;
        private Rect rectDebugButtonClear;
        private Rect rectDebugButtonFontInc;
        private Rect rectDebugButtonFontDesc;
        private Rect rectCommandBackground;
        private Rect rectCommandArea;
        private Rect rectCommandInput;
        private Rect rectCommandHelpBox;
        private Rect rectCommandReturn;
        private Vector2 scrollDebugPosition = Vector2.zero;
        private Vector2 scrollCommandPosition = Vector2.zero;
        private Vector2 scrollDebugDragPosition = Vector2.zero;
        private Vector2 scrollCommandDragPosition = Vector2.zero;
        private Dictionary<string, CommandData> commandData = new Dictionary<string, CommandData>();
        private List<string> log = new List<string>();
        private List<string> excuteLog = new List<string>();

        private bool isShowing;
        private bool isScrollDebugDragging = false;
        private string commandHelpText = "";
        private string command = "";
        private int executeLogPos;
        private int indexConsole;
        private int indexDebug;
#if !(UNITY_EDITOR || UNITY_EDITOR_OSX || UNITY_WEBGL) && (UNITY_ANDROID || UNITY_IOS)
        private Vector3 _swipePosStart = Vector3.zero;
        private Vector3 _swipePosMoving = Vector3.zero;
#endif

        private void ToggleConsole() { isShowing = !isShowing; }
        private void ScrollLogToBottom() { instance.scrollDebugPosition = instance.scrollLockPosition; }

        public static void Log(string str, string colorhex = null, bool bold = false, bool italic = false)
        {
            if (string.IsNullOrEmpty(str))
                return;
            if (!string.IsNullOrEmpty(colorhex))
            {
                if (colorhex.Length == 7 && colorhex.Contains("#"))
                    str = $"<color={colorhex}>{str}</color>";
            }
            if (bold)
                str = $"<b>{str}</b>";
            if (italic)
                str = $"<i>{str}</i>";
            instance.log.Add(str);
            instance.ScrollLogToBottom();
        }

        private void LogReceiveCallback(string message, string stackTrace, LogType type)
        {
            switch (type)
            {
                case LogType.Warning:
                    Log($"Warning: {message}", colorhex: "#FFFF00", italic: true);
                    if (autoShowWarning & !isShowing) isShowing = true;
                    break;
                case LogType.Error:
                    Log($"Error: {message}", colorhex: "#FF0000", bold: true);
                    if (autoShowError & !isShowing) isShowing = true;
                    break;
                case LogType.Exception:
                    Log($"<b>Exception: {message}</b>\n{stackTrace}", colorhex: "#FF0000");
                    if (autoShowException & !isShowing) isShowing = true;
                    break;
                default:
                    Log(message);
                    break;
            }
            while (log.Count > logCapacity)
                log.RemoveAt(0);
            ScrollLogToBottom();
        }

        void Clear()
        {
            log.Clear();
        }

        private void UpdateWindow()
        {
            if (Screen.width > Screen.height)
            {
                //Landscape
                rectDebugLogBackground = new Rect(0, 0, Screen.width / 2, Screen.height - 64);
                rectDebugLogScroll = new Rect(8, 24, (Screen.width / 2) - 16, Screen.height - 96);
                rectDebugLogScrollDrag = new Rect(8, 24, (Screen.width / 2) - 48, Screen.height - 96);
                rectDebugButtonClear = new Rect(0, Screen.height - 64, 128, 64);
                rectDebugButtonFontInc = new Rect((Screen.width / 2) - 128, Screen.height - 64, 64, 64);
                rectDebugButtonFontDesc = new Rect((Screen.width / 2) - 64, Screen.height - 64, 64, 64);
                rectCommandBackground = new Rect(Screen.width / 2, 64, Screen.width / 2, Screen.height - 64);
                rectCommandArea = new Rect((Screen.width / 2) + 8, 136, (Screen.width / 2) - 16, Screen.height - 144);
                rectCommandInput = new Rect(Screen.width / 2, 0, (Screen.width / 2) - 64, 64);
                rectCommandHelpBox = new Rect((Screen.width / 2) + 8, 72, (Screen.width / 2) - 64, 60);
                rectCommandReturn = new Rect(Screen.width - 64, 0, 64, 64);
            }
            else
            {
                //Portrait
                rectDebugLogBackground = new Rect(0, 0, Screen.width, (Screen.height / 2) - 64);
                rectDebugLogScroll = new Rect(8, 24, Screen.width - 16, (Screen.height / 2) - 96);
                rectDebugLogScrollDrag = new Rect(8, 24, Screen.width - 48, (Screen.height / 2) - 96);
                rectDebugButtonClear = new Rect(0, (Screen.height / 2) - 64, 128, 64);
                rectDebugButtonFontInc = new Rect(Screen.width - 128, (Screen.height / 2) - 64, 64, 64);
                rectDebugButtonFontDesc = new Rect(Screen.width - 64, (Screen.height / 2) - 64, 64, 64);
                rectCommandBackground = new Rect(0, (Screen.height / 2) + 64, Screen.width, (Screen.height / 2) - 64);
                rectCommandArea = new Rect(8, (Screen.height / 2) + 136, Screen.width - 16, (Screen.height / 2) - 144);
                rectCommandInput = new Rect(0, Screen.height / 2, Screen.width - 64, 64);
                rectCommandHelpBox = new Rect(8, (Screen.height / 2) + 72, Screen.width - 64, 60);
                rectCommandReturn = new Rect(Screen.width - 64, Screen.height / 2, 64, 64);
            }
        }


        private void ShowDebugWindow()
        {
            GUI.Box(rectDebugLogBackground, GUIContent.none, guiSkin.customStyles[0]);
            GUILayout.Space(24f);
            using (var areaScope = new GUILayout.AreaScope(rectDebugLogScroll))
            {
                using (var scrollScope = new GUILayout.ScrollViewScope(scrollDebugPosition, guiSkin.scrollView))
                {
                    scrollScope.handleScrollWheel = true;
                    scrollDebugPosition = scrollScope.scrollPosition;
                    for (indexDebug = 0; indexDebug < log.Count; indexDebug++)
                    {
                        if (indexDebug > logCapacity)
                            break;
                        GUILayout.Label(log[indexDebug], guiSkin.label, GUILayout.Width((Screen.width / 2) - 52));
                    }
                }
            }
            if (GUI.Button(rectDebugButtonClear, "Clear", guiSkin.button))
            {
                Clear();
            }
            if (GUI.Button(rectDebugButtonFontInc, "A+", guiSkin.button))
            {
                if (guiSkin.label.fontSize < 64)
                    guiSkin.label.fontSize += 2;
            }
            if (GUI.Button(rectDebugButtonFontDesc, "A-", guiSkin.button))
            {
                if (guiSkin.label.fontSize > 8)
                    guiSkin.label.fontSize -= 2;
            }
        }

        private void ShowCommandWindow()
        {
            GUI.SetNextControlName("commandfield");
            GUI.enabled = commandData.Count > 0;
            command = GUI.TextField(rectCommandInput, command, guiSkin.textField);
            if (GUI.Button(rectCommandReturn, "Send", guiSkin.button))
                RunCommand();
            GUI.enabled = true;
            GUI.Box(rectCommandBackground, GUIContent.none, guiSkin.customStyles[1]);
            GUI.Label(rectCommandHelpBox, commandHelpText);
            using (var areaScope = new GUILayout.AreaScope(rectCommandArea))
            {
                using (var scrollScope = new GUILayout.ScrollViewScope(scrollCommandPosition, guiSkin.scrollView))
                {
                    scrollScope.handleScrollWheel = true;
                    scrollCommandPosition = scrollScope.scrollPosition;
                    using (var verticalScope = new GUILayout.VerticalScope())
                    {
                        foreach (var commandElement in commandData)
                        {
                            if (GUILayout.Button(commandElement.Value.name, guiSkin.button))
                            {
                                command = commandElement.Key;
                                commandHelpText = commandElement.Value.description;
                                GUI.FocusControl("commandfield");
                            }
                        }
                    }
                }
            }
            if (GUI.GetNameOfFocusedControl() == "commandfield")
            {
                if (Event.current.isKey)
                {
                    switch (Event.current.keyCode)
                    {
                        case KeyCode.UpArrow:
                            GetPrevExcuteLog();
                            break;
                        case KeyCode.DownArrow:
                            GetNextExcuteLog();
                            break;
                        case KeyCode.Return:
                        case KeyCode.KeypadEnter:
                            RunCommand();
                            break;
                    }
                }
            }
        }

        public void AddCommand(string prefix, string name, string description, LuviCommandExecution execution)
        {
            if (!commandData.ContainsKey(prefix))
                commandData.Add(prefix, new CommandData(name, description, execution));
            else
                Debug.LogWarning($"Luvi Console already have {prefix} command, Disposed.");
        }

        public bool RemoveCommand(string prefix)
        {
            return commandData.Remove(prefix);
        }

        private void RunCommand()
        {
            command = command.Trim();
            if (!string.IsNullOrEmpty(command))
            {
                List<string> splitCommand = SplitCommand(command);
                if (splitCommand.Count > 0)
                {
                    string prefix = splitCommand[0];
                    if (commandData.ContainsKey(prefix))
                    {
                        commandData[prefix].execution(splitCommand);
                        ExcuteLog();
                        return;
                    }
                    else
                    {
                        Log("No command found.", colorhex: "#FFFF00");
                        ExcuteLog();
                        return;
                    }
                }
                else
                {
                    // This can be happen when user put only double quote into the command -> "".
                    // Just reset it.
                    command = "";
                    return;
                }
            }
        }

        private void ExcuteLog()
        {
            excuteLog.Add(command);
            while (excuteLog.Count > excuteCapacity)
                excuteLog.RemoveAt(0);
            executeLogPos = excuteLog.Count;
            command = "";
        }

        private List<string> SplitCommand(string str)
        {
            const char spliter = ' ';
            const char quote = '"';
            bool isQuote = false;
            List<string> list = new List<string>();
            StringBuilder sb = new StringBuilder();
            sb.Clear();
            for (int i = 0; i < str.Length; i++)
            {
                if (isQuote)
                {
                    if (str[i] == quote)
                    {
                        isQuote = false;
                        continue;
                    }
                }
                else
                {
                    if (str[i] == spliter)
                    {
                        if (sb.Length > 0)
                            list.Add(sb.ToString());
                        sb.Clear();
                        continue;
                    }
                    if (str[i] == quote)
                    {
                        isQuote = true;
                        continue;
                    }
                }
                sb.Append(str[i]);
            }
            if (sb.Length > 0)
                list.Add(sb.ToString());
            sb.Clear();
            return list;
        }

        private void GetNextExcuteLog()
        {
            if (excuteLog.Count == 0)
                return;
            executeLogPos++;
            if (executeLogPos >= excuteLog.Count)
            {
                executeLogPos = excuteLog.Count - 1;
                command = "";
                return;
            }
            command = excuteLog[executeLogPos];
        }

        private void GetPrevExcuteLog()
        {
            if (excuteLog.Count == 0)
                return;
            executeLogPos--;
            if (executeLogPos < 0)
                executeLogPos = 0;
            command = excuteLog[executeLogPos];
        }
    }

    public delegate void LuviCommandExecution(List<string> splitCommand);
}