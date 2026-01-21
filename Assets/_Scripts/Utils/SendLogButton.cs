using System;
// using Game.HotUpdateScripts.PortalScene.Portal;
using UnityEngine;
using UnityEngine.UI;

public class SendLogButton : MonoSingleton<SendLogButton>
{
    [SerializeField] private Button btnSendLog;

    protected override void SingletonStarted()
    {
        btnSendLog.onClick.AddListener(Logger.UploadLogToServer);
        //btnSendLog.gameObject.SetActive(PortalModel.IS_DEV);
    }

    protected override void NotifyInstanceRepeated()
    {
        Destroy(gameObject);
    }
    
}