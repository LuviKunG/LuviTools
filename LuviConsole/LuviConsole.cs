using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

// LuviConsole 2.3.7
// https://github.com/LuviKunG

[AddComponentMenu("LuviKunG/LuviConsole")]
public class LuviConsole : MonoBehaviour
{
    private static LuviConsole _instance;
    public static LuviConsole Instance
    {
        get
        {
            if (_instance == null)
                throw new Exception("Cannot get any instantiate of LuviConsole.");
            return _instance;
        }
    }

    [SerializeField]
    private GUISkin guiSkin;
    public bool isShowing { get; private set; }
    public bool isLandscape { get; private set; }

    void Awake()
    {
        if (!_instance) _instance = this;
        Initialize();
        CheckScreenRotation();
    }

    void Initialize()
    {
        if (guiSkin == null) guiSkin = Resources.Load<GUISkin>("LuviConsoleGUI");
        guiSkin.label.fontSize = defaultFontSize;
#if UNITY_4
        Application.RegisterLogCallbackThreaded(LogReceiveCallback);
#endif
        DontDestroyOnLoad(gameObject);
    }

    void CheckScreenRotation()
    {
        isLandscape = Screen.width > Screen.height ? true : false;
    }

    #region DEBUG

    public int logCapacity = 64;
    List<string> _log = new List<string>();
    public float swipeRatio = 0.9f;
    public int defaultFontSize = 16;
    public bool autoShowWarning = false;
    public bool autoShowError = false;
    public bool autoShowException = false;
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
    private Vector3 _swipePosStart = Vector3.zero;
    private Vector3 _swipePosMoving = Vector3.zero;
#endif

#if UNITY_5 || UNITY_2018 || UNITY_2019
    void OnEnable()
    {
        Application.logMessageReceivedThreaded += LogReceiveCallback;
    }

    void OnDisable()
    {
        Application.logMessageReceivedThreaded -= LogReceiveCallback;
    }
#endif

    void ToggleConsole()
    {
        isShowing = !isShowing;
    }

    public static void Log(string str, string colorhex = null, bool bold = false, bool italic = false)
    {
        if (_instance == null)
            return;
        if (string.IsNullOrEmpty(str))
            return;
        if (!string.IsNullOrEmpty(colorhex))
        {
            if (colorhex.Length == 7 && colorhex.Contains("#"))
                str = string.Format("<color={0}>{1}</color>", colorhex, str);
        }
        if (bold)
            str = string.Format("<b>{0}</b>", str);
        if (italic)
            str = string.Format("<i>{0}</i>", str);
        _instance._log.Add(str);
        _instance._scrollDebugPosition = _instance._scrollLockPosition;
    }

    public static void LogError(string str)
    {
        if (_instance == null)
            return;
        Log(string.Format("Error: {0}", str), colorhex: "#FF0000", bold: true);
        if (_instance.autoShowError & !_instance.isShowing) _instance.isShowing = true;
    }

    public static void LogWarning(string str)
    {
        if (_instance == null)
            return;
        Log(string.Format("Warning: {0}", str), colorhex: "#FFFF00", italic: true);
        if (_instance.autoShowWarning & !_instance.isShowing) _instance.isShowing = true;
    }

    void LogReceiveCallback(string log, string stack, LogType type)
    {
        switch (type)
        {
            case LogType.Warning:
                Log(string.Format("Warning: {0}", log), colorhex: "#FFFF00", italic: true);
                if (autoShowWarning & !isShowing) isShowing = true;
                break;
            case LogType.Error:
                Log(string.Format("Error: {0}", log), colorhex: "#FF0000", bold: true);
                if (autoShowError & !isShowing) isShowing = true;
                break;
            case LogType.Exception:
                Log(string.Format("<b>Exception: {0}</b>\n{1}", log, stack), colorhex: "#FF0000");
                if (autoShowException & !isShowing) isShowing = true;
                break;
            default:
                Log(log);
                break;
        }
        while (_log.Count > logCapacity)
            _log.RemoveAt(0);
        _scrollDebugPosition = _scrollLockPosition;
    }

    void Clear()
    {
        _log.Clear();
    }

    Rect _debugBackgroundLandscape = new Rect(0, 0, Screen.width / 2, Screen.height - 64);
    Rect _debugButtonClearLandscape = new Rect(0, Screen.height - 64, 128, 64);
    Rect _debugButtonFontSizeIncreaseLandscape = new Rect((Screen.width / 2) - 128, Screen.height - 64, 64, 64);
    Rect _debugButtonFontSizeDecreaseLandscape = new Rect((Screen.width / 2) - 64, Screen.height - 64, 64, 64);
    Rect _debugScrollViewAreaLandscape = new Rect(8, 24, (Screen.width / 2) - 16, Screen.height - 96);


    Rect _debugBackgroundPortrait = new Rect(0, 0, Screen.width, (Screen.height / 2) - 64);
    Rect _debugButtonClearPortrait = new Rect(0, (Screen.height / 2) - 64, 128, 64);
    Rect _debugButtonFontSizeIncreasePortrait = new Rect(Screen.width - 128, (Screen.height / 2) - 64, 64, 64);
    Rect _debugButtonFontSizeDecreasePortrait = new Rect(Screen.width - 64, (Screen.height / 2) - 64, 64, 64);
    Rect _debugScrollViewAreaPortrait = new Rect(8, 24, Screen.width - 16, (Screen.height / 2) - 96);

    Vector2 _scrollDebugPosition = Vector2.zero;
    Vector2 _scrollLockPosition = new Vector2(0, Mathf.Infinity);

    int indexDebug;

    void ShowDebugWindow()
    {
        if (isLandscape)
        {
            GUI.Box(_debugBackgroundLandscape, GUIContent.none, guiSkin.customStyles[0]);
            GUILayout.Space(24f);
            GUILayout.BeginArea(_debugScrollViewAreaLandscape);
            _scrollDebugPosition = GUILayout.BeginScrollView(_scrollDebugPosition, guiSkin.scrollView);
            for (indexDebug = 0; indexDebug < _log.Count; indexDebug++)
            {
                if (indexDebug > logCapacity)
                    break;
                GUILayout.Label(_log[indexDebug], guiSkin.label, GUILayout.Width((Screen.width / 2) - 52));
            }
            GUILayout.EndArea();
            GUILayout.EndScrollView();
            if (GUI.Button(_debugButtonClearLandscape, "Clear", guiSkin.button))
            {
                Clear();
            }
            if (GUI.Button(_debugButtonFontSizeIncreaseLandscape, "A+", guiSkin.button))
            {
                if (guiSkin.label.fontSize < 64)
                    guiSkin.label.fontSize += 2;
            }
            if (GUI.Button(_debugButtonFontSizeDecreaseLandscape, "A-", guiSkin.button))
            {
                if (guiSkin.label.fontSize > 8)
                    guiSkin.label.fontSize -= 2;
            }
        }
        else
        {
            GUI.Box(_debugBackgroundPortrait, GUIContent.none, guiSkin.customStyles[0]);
            GUILayout.Space(24f);
            GUILayout.BeginArea(_debugScrollViewAreaPortrait);
            _scrollDebugPosition = GUILayout.BeginScrollView(_scrollDebugPosition, guiSkin.scrollView);
            for (indexDebug = 0; indexDebug < _log.Count; indexDebug++)
            {
                if (indexDebug > logCapacity)
                    break;
                GUILayout.Label(_log[indexDebug], guiSkin.label, GUILayout.Width(Screen.width - 52));
            }
            GUILayout.EndArea();
            GUILayout.EndScrollView();
            if (GUI.Button(_debugButtonClearPortrait, "Clear", guiSkin.button))
            {
                Clear();
            }
            if (GUI.Button(_debugButtonFontSizeIncreasePortrait, "A+", guiSkin.button))
            {
                if (guiSkin.label.fontSize < 64)
                    guiSkin.label.fontSize += 2;
            }
            if (GUI.Button(_debugButtonFontSizeDecreasePortrait, "A-", guiSkin.button))
            {
                if (guiSkin.label.fontSize > 8)
                    guiSkin.label.fontSize -= 2;
            }
        }
    }

    #endregion

    #region COMMAND

    public int excuteCapacity = 16;
    string commandHelpText = "";
    string _command = "";
    List<LuviCommandPair> _commandList = new List<LuviCommandPair>();
    List<string> _excuteLog = new List<string>();
    Rect _commandBackgroundLandscape = new Rect(Screen.width / 2, 64, Screen.width / 2, Screen.height - 64);
    Rect _commandInputLandscape = new Rect(Screen.width / 2, 0, (Screen.width / 2) - 64, 64);
    Rect _commandHelpBoxLandscape = new Rect((Screen.width / 2) + 8, 72, (Screen.width / 2) - 64, 60);
    Rect _commandButtonReturnLandscape = new Rect(Screen.width - 64, 0, 64, 64);
    Rect _commandAreaLandscape = new Rect((Screen.width / 2) + 8, 136, (Screen.width / 2) - 16, Screen.height - 144);

    Rect _commandBackgroundPortrait = new Rect(0, (Screen.height / 2) + 64, Screen.width, (Screen.height / 2) - 64);
    Rect _commandInputPortrait = new Rect(0, Screen.height / 2, Screen.width - 64, 64);
    Rect _commandHelpBoxPortrait = new Rect(8, (Screen.height / 2) + 72, Screen.width - 64, 60);
    Rect _commandButtonReturnPortrait = new Rect(Screen.width - 64, Screen.height / 2, 64, 64);
    Rect _commandAreaPortrait = new Rect(8, (Screen.height / 2) + 136, Screen.width - 16, (Screen.height / 2) - 144);

    Vector2 _scrollCommandPosition = Vector2.zero;
    Vector2 _commandButtonPos;
    Vector2 _commandButtonCalcSize;
    GUIStyle _commandButtonStyle;
    int _executeLogPos;

    int indexConsole;

    void ShowCommandWindow()
    {
        if (isLandscape)
        {
            GUI.SetNextControlName("commandfield");
            _command = GUI.TextField(_commandInputLandscape, _command, guiSkin.textField);
            if (GUI.Button(_commandButtonReturnLandscape, "Send", guiSkin.button))
                RunCommand();
            GUI.Box(_commandBackgroundLandscape, GUIContent.none, guiSkin.customStyles[1]);
            GUI.Label(_commandHelpBoxLandscape, commandHelpText);
            GUILayout.BeginArea(_commandAreaLandscape);
            _scrollCommandPosition = GUILayout.BeginScrollView(_scrollCommandPosition, guiSkin.scrollView);
            GUILayout.BeginVertical();
            for (indexConsole = 0; indexConsole < _commandList.Count; indexConsole++)
            {
                if (GUILayout.Button(_commandList[indexConsole].name, guiSkin.button))
                {
                    if (_commandList[indexConsole].size == 0)
                        _command = _commandList[indexConsole].preCommand;
                    else
                        _command = _commandList[indexConsole].preCommand + " ";
                    commandHelpText = _commandList[indexConsole].helpDescription;
                    GUI.FocusControl("commandfield");
                }
            }
            GUILayout.EndVertical();
            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }
        else
        {
            GUI.SetNextControlName("commandfield");
            _command = GUI.TextField(_commandInputPortrait, _command, guiSkin.textField);
            if (GUI.Button(_commandButtonReturnPortrait, "Send", guiSkin.button))
                RunCommand();
            GUI.Box(_commandBackgroundPortrait, GUIContent.none, guiSkin.customStyles[1]);
            GUI.Label(_commandHelpBoxPortrait, commandHelpText);
            GUILayout.BeginArea(_commandAreaPortrait);
            _scrollCommandPosition = GUILayout.BeginScrollView(_scrollCommandPosition, guiSkin.scrollView);
            GUILayout.BeginVertical();
            for (indexConsole = 0; indexConsole < _commandList.Count; indexConsole++)
            {
                if (GUILayout.Button(_commandList[indexConsole].name, guiSkin.button))
                {
                    if (_commandList[indexConsole].size == 0)
                        _command = _commandList[indexConsole].preCommand;
                    else
                        _command = _commandList[indexConsole].preCommand + " ";
                    commandHelpText = _commandList[indexConsole].helpDescription;
                    GUI.FocusControl("commandfield");
                }
            }
            GUILayout.EndVertical();
            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }
        GetInputFromKeyboard();
    }

    public void AddCommand(string commandName, string commandHelp, string commandPre, byte commandRequireSize, Action<string[]> commandAction)
    {
        _commandList.Add(new LuviCommandPair() { name = commandName, helpDescription = commandHelp, preCommand = commandPre, size = commandRequireSize, command = new Action<string[]>(commandAction) });
    }

    public void RemoveCommand(string commandName)
    {
        for (int i = 0; i < _commandList.Count; i++)
            if (_commandList[i].name == commandName)
                _commandList.RemoveAt(i);
    }

    void RunCommand()
    {
        _command = _command.Trim();
        if (!string.IsNullOrEmpty(_command))
        {
            string[] words = _command.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < _commandList.Count; i++)
            {
                if (_commandList[i].preCommand == words[0])
                {
                    if (_commandList[i].size == (words.Length - 1))
                    {
                        _commandList[i].command(words);
                        ExcuteLog();
                        return;
                    }
                    else
                    {
                        Log(string.Format("This \'{0}\' command need {1} parameter(s) to execute", _commandList[i].name, _commandList[i].size), colorhex: "#FF0000", bold: true);
                        ExcuteLog();
                        return;
                    }
                }
            }
            Log("No available command were match!", colorhex: "#FF0000", bold: true);
        }
        ExcuteLog();
    }

    void ExcuteLog()
    {
        _excuteLog.Add(_command);
        while (_excuteLog.Count > excuteCapacity)
            _excuteLog.RemoveAt(0);
        _executeLogPos = _excuteLog.Count;
        _command = "";
    }
    #endregion

    private void Update()
    {
#if UNITY_EDITOR || UNITY_EDITOR_OSX
        GetShowHideByEditor();
#elif UNITY_ANDROID || UNITY_IOS
        GetShowHideByTouch();
#elif UNITY_WEBGL
        GetShowHideByWebGL();
#endif
    }

    void OnGUI()
    {
        GUI.skin = guiSkin;
        if (isShowing)
        {
            ShowDebugWindow();
            ShowCommandWindow();
        }
    }
#if UNITY_EDITOR
    void GetShowHideByEditor()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            ToggleConsole();
            GUI.FocusControl("commandfield");
        }
    }
#elif UNITY_EDITOR_OSX
    void GetShowHideByEditor()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleConsole();
            GUI.FocusControl("commandfield");
        }
    }
#elif UNITY_ANDROID || UNITY_IOS
    void GetShowHideByTouch()
    {
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
    }
#elif UNITY_WEBGL
    void GetShowHideByWebGL()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleConsole();
            GUI.FocusControl("commandfield");
        }
    }
#endif

    void GetInputFromKeyboard()
    {
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

    void GetNextExcuteLog()
    {
        if (_excuteLog.Count == 0)
            return;
        _executeLogPos++;
        if (_executeLogPos >= _excuteLog.Count)
        {
            _executeLogPos = _excuteLog.Count - 1;
            _command = "";
            return;
        }
        _command = _excuteLog[_executeLogPos];
    }

    void GetPrevExcuteLog()
    {
        if (_excuteLog.Count == 0)
            return;
        _executeLogPos--;
        if (_executeLogPos < 0)
            _executeLogPos = 0;
        _command = _excuteLog[_executeLogPos];
    }

    public static bool IsIntOrLong(string str)
    {
        return Regex.IsMatch(str, "^[0-9]+$");
    }

    public static bool IsFloatOrDouble(string str)
    {
        return (Regex.IsMatch(str, "^[0-9]+\\.[0-9]+$") || Regex.IsMatch(str, "^[0-9]+\\.[0-9]+\\+(E|e)[0-9]+$"));
    }

    public static bool IsBoolean(string str)
    {
        return Regex.IsMatch(str, "^(true|false)$");
    }

    public class LuviCommandPair
    {
        public Action<string[]> command = null;
        public byte size;
        public string name;
        public string helpDescription;
        public string preCommand;
    }
}