
using UnityEngine;

public class LogManager : MonoBehaviour
{
    [SerializeField] private bool enableLog;

    private void Start()
    {
#if UNITY_EDITOR
        Debug.unityLogger.logEnabled = true;
#else
        Debug.unityLogger.logEnabled = true;
        Debug.unityLogger.filterLogType = enableLog ? LogType.Log : LogType.Exception;
#endif
        Application.logMessageReceived += HandleException;
    }

    private void HandleException(string logString, string stackTrace, LogType type)
    {
        if (type != LogType.Exception) return;
        Logger.SystemLog(logString, stackTrace);
    }
}