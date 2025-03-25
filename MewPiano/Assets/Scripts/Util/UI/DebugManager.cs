using System;
using System.Reflection;
using System.Text;
using UnityEngine;
using System.Diagnostics;

#if UNITY_EDITOR
using UnityEditor;
#endif

public enum DebugColor
{
    none,
    UI,
    Scene,
    Data,
	BackEnd,
	Game,
    AD,
    Login,
    Web,
}

public class DebugManager
{
	public static string[] hexColors = new string[]{
		"" ,        //none
        "#FF0000",  //red
        "#56CBF5",  //blue
        "#FFA500",  //orange
        "#00FF00",  //green
        "#FFFF00",  //yellow
        "#FFC0CB",  //pink
        "#800080",  //purple
        "#9B0065",  //purple
    };

#if !UNITY_EDITOR
    [Conditional( "DEBUG_LOG" ), Conditional( "DEVELOPMENT_BUILD" )]
#endif
	public static void Log(string _message, DebugColor _colorManager = DebugColor.none)
    {
        UnityEngine.Debug.Log(GetSbManager(_message, _colorManager));
    }

    public static void Warnning(string _message, DebugColor _colorManager = DebugColor.none)
    {
        UnityEngine.Debug.LogWarning(GetSbManager(_message, _colorManager));
    }

    public static void Error(string _message, DebugColor _colorManager = DebugColor.none)
    {
        UnityEngine.Debug.LogError(GetSbManager(_message, _colorManager));
    }

    private static StringBuilder GetSbManager(string _message, DebugColor _colorManager = DebugColor.none)
    {
        StringBuilder sb = new StringBuilder();
        if (_colorManager == DebugColor.none)
        {
            sb.Append(_message);
        }
        else
        {
            string head = $"<color={hexColors[(int)_colorManager]}><b>[";
            string middle = _colorManager.ToString();
            string tail = "]</b></color> ";
            sb.Append(head);
            sb.Append(middle);
            sb.Append(tail);
            sb.Append(_message);
        }
        return sb;
    }


#if UNITY_EDITOR
    private static MethodInfo clearConsole;
    private static MethodInfo ClearConsole
    {
        get
        {
            if (clearConsole == null)
            {
                Assembly assembly = Assembly.GetAssembly(typeof(SceneView));
                System.Type logEntries = assembly.GetType("UnityEditor.LogEntries");
                clearConsole = logEntries.GetMethod("Clear");
            }
            return clearConsole;
        }
    }
#endif

    public static void ClearLog()
    {
#if UNITY_EDITOR
        ClearConsole.Invoke(new object(), null);
#endif
    }

    public static void ClearLog<T>(T _message, DebugColor colorManager = DebugColor.none)
    {
#if UNITY_EDITOR
        ClearConsole.Invoke(new object(), null);
#endif
        string message = _message?.ToString();
        if (!string.IsNullOrEmpty(message))
        {
			UnityEngine.Debug.Log(GetSbManager(message, colorManager));
		}
    }
}