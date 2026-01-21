using System.Collections;
using System.Collections.Generic;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using UnityEngine;

public abstract class BaseConnection : MonoBehaviour
{
    private SmartFoxConnection SmartFoxConnection => SmartFoxConnection.Instance;
    protected virtual Dictionary<string, EventListenerDelegate> RegisterEvents { get; } = new();

    protected virtual void OnEnable()
    {
        if (SmartFoxConnection.Sfs != null)
        {
            SmartFoxConnection.AddEventListener(SFSEvent.EXTENSION_RESPONSE, OnExtensionResponse);
            foreach (var registerEvent in RegisterEvents)
            {
                SmartFoxConnection.AddEventListener(registerEvent.Key, registerEvent.Value);
            }
        }
        else
            StartCoroutine(IEWaitConnectServer());

        IEnumerator IEWaitConnectServer()
        {
            yield return new WaitUntil(() => SmartFoxConnection.Sfs != null);
            SmartFoxConnection.AddEventListener(SFSEvent.EXTENSION_RESPONSE, OnExtensionResponse);
            foreach (var registerEvent in RegisterEvents)
            {
                SmartFoxConnection.AddEventListener(registerEvent.Key, registerEvent.Value);
            }
        }
    }

    protected virtual void OnDisable()
    {
        if (SmartFoxConnection)
            SmartFoxConnection.RemoveEventListener(SFSEvent.EXTENSION_RESPONSE, OnExtensionResponse);
        foreach (var registerEvent in RegisterEvents)
        {
            if (SmartFoxConnection)
                SmartFoxConnection.RemoveEventListener(registerEvent.Key, registerEvent.Value);
        }
    }

    private void OnExtensionResponse(BaseEvent evt)
    {
        var cmd = (string)evt.Params["cmd"];
        var data = (SFSObject)evt.Params["params"];
        HandleExtensionData(cmd, data);
    }

    protected abstract void HandleExtensionData(string cmd, SFSObject data);
}