using System.Collections;
using System.Reflection;
using UnityEngine;

#if UNITY_EDITOR

public class DevMan : Manager<DevMan>
{
    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.I))
        {
            ClearDebug();
        }
    }

    public void ClearDebug()
    {
        Debug.ClearDeveloperConsole();
        var assembly = Assembly.GetAssembly(typeof(UnityEditor.ActiveEditorTracker));
        var type = assembly.GetType("UnityEditorInternal.LogEntries");
        var method = type.GetMethod("Clear");
        method.Invoke(new object(), null);
    }
}

#endif