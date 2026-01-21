using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

using Game.HotUpdateScripts.Utils;
using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using Sfs2X.Util;
using strange.extensions.signal.impl;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SmartFoxConnection : MonoSingleton<SmartFoxConnection>
{
    public SmartFox Sfs { get; private set; }
    public bool IsConnected => Sfs is { IsConnected: true };
    public User MySelf => Sfs?.MySelf;
    private const string _ZONE_DEFAULT = "fbwjkbfas";
    private const float _LOST_CONNECTION_TIME = 16;
    private const float _CONNECT_TIME_OUT = 4;
    private const float _CLIENT_PING_PERIOD = 4;
    private Coroutine _clientConnectionCoroutine;
    private Coroutine _connectTimeout;
    private Tween _clientConnectTimeoutTween;
    private Tween _clientConnectTimeoutTweenWhenPauseApp;

    private ConfigData _config;
    private const bool _ENABLE_CLIENT_PING = true;
    public static readonly Signal OnConnectionSuccess = new();
    public static readonly Signal<int> OnPingPongUpdate = new();
    public static readonly Signal OnLostConnectionFromServer = new();

    private bool isPauseApp;

    private void Update()
    {
        Sfs?.ProcessEvents();
    }

    public void Connect()
    {
        if (IsConnected) return;
        Loading.Open();
        _connectTimeout = StartCoroutine(IEConnectTimeout());
        ConfigData cfg = new ConfigData
        {
            Zone = "sfsak",
            Debug = false,
            Host = "dev.sandinhstudio.com",
            Port = 9012
        };
        _config = cfg;
        // cfg.BlueBox.IsActive = false;
        InitSmartFox();
        var platform = "unity-" + GameUtils.GetPlatform();
        //Sfs.SetClientDetails(platform, GameUtils.GetVersion());
        Sfs.Connect(cfg);

        IEnumerator IEConnectTimeout()
        {
            yield return Helpers.GetWaitForSeconds(_CONNECT_TIME_OUT);
            OnLostConnectionFromServer.Dispatch();
        }
    }

    public void OnLoginRequest()
    {
        Debug.Log("OnLoginRequest");
        Sfs.Send(new LoginRequest(UserModel.Instance.Uid.ToString()));
    }

    private void InitSmartFox()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        _sfs = new SmartFox(UseWebSocket.WSS_BIN);
#else
        
            Sfs = new SmartFox();
#endif
        Sfs.AddEventListener(SFSEvent.CONNECTION, OnConnection);
        Sfs.AddEventListener(SFSEvent.CONNECTION_RETRY, OnConnectionRetry);
        Sfs.AddEventListener(SFSEvent.CONNECTION_RESUME, OnConnectionResume);
        Sfs.AddEventListener(SFSEvent.CONNECTION_LOST, OnConnectionLost);
        Sfs.AddEventListener(SFSEvent.LOGIN, OnLogin);
        Sfs.AddEventListener(SFSEvent.LOGIN_ERROR, OnLoginError);
        Sfs.AddEventListener(SFSEvent.LOGOUT, OnLogout);
        Sfs.AddEventListener(SFSEvent.USER_VARIABLES_UPDATE, OnUserVariableUpdate);
        Sfs.AddEventListener(SFSEvent.EXTENSION_RESPONSE, OnExtensionResponse);
        Sfs.AddEventListener(SFSEvent.PING_PONG, OnPingPong);
    }

    public void SubscribeRoom(string roomName)
    {
        Logger.Log($"SubscribeRoom: {roomName}");
        Send(new SubscribeRoomGroupRequest(roomName));
    }

    public void UnSubscribeRoom(string roomName)
    {
        Logger.Log($"UnSubscribeRoom: {roomName}");
        Send(new UnsubscribeRoomGroupRequest(roomName));
    }

    public void JoinRoom(string roomName)
    {
        Logger.Log($"JoinRoom: {roomName}");
        Send(new JoinRoomRequest(roomName));
    }

    private void OnExtensionResponse(BaseEvent evt)
    {
        var cmd = (string)evt.Params["cmd"];
        SFSObject data = (SFSObject)evt.Params["params"];
        if (cmd is not (ExtCmd.PING or ExtCmd.CLIENT_PONG))
            Logger.Log($"Response cmd: {cmd}, data: {data.ToJson()}");
        switch (cmd)
        {
            case ExtCmd.PING:
                SendExt(ExtCmd.PONG);
                break;
            case ExtCmd.CLIENT_PONG:
                StartClientConnectTimeOut();
                _clientConnectTimeoutTweenWhenPauseApp?.Kill();
                _clientConnectTimeoutTweenWhenPauseApp = null;
                Debug.LogWarning("client pong");
                break;
            case ExtCmd.SERVER_TIME:
                ServerTimeUtils.UpdateTime(data.GetLong("t"));
                PaymentUtils.Instance.maxPotPiggyBank = data.GetLong("pb");
                break;
            case ExtCmd.HEADER_NOTICE:
                HeaderNoticeUtils.Instance.EnqueueMessage(data.GetUtfString("m"));
                break;
           
        }
    }

    private void CheckConditionsToOpenPopup()
    {
        // if (UserModel.Instance.UserInfoData.nld.Value == 0)
        // {
        //     NLDTriggerPopup.Open();
        //     return;
        // }
        //
        // if (UserModel.Instance.UserInfoData.PiggyBank.Value != -1)
        // {
        //     long piggyValue = UserModel.Instance.UserInfoData.PiggyBank.Value;
        //     long maxValue = PaymentUtils.Instance.maxPotPiggyBank;
        //     double ratio = (double)piggyValue / maxValue;
        //     if (ratio >= 0.75)
        //     {
        //         if (PiggyBankDetailPopup.IsOpen())
        //         {
        //             PiggyBankDetailPopup.Close();
        //         }
        //
        //         PiggyBankTriggerPopup.Open();
        //         return;
        //     }
        // }
        //
        // var flashSale = UserModel.Instance.UserInfoData.FlashSaleTime.Value;
        //
        //
        // if (flashSale is { amount: > 0 })
        // {
        //     if (FlashSaleDetailPopup.IsOpen())
        //     {
        //         FlashSaleDetailPopup.Close();
        //     }
        //
        //     if (FlashSaleCreatePopup.IsOpen())
        //     {
        //         FlashSaleCreatePopup.Close();
        //     }
        //
        //     FlashSaleTriggerPopup.Open(view => view.UpdateUI(10, true));
        //     return;
        // }
        //
        // NLDTriggerPopup.Open();
    }

    IEnumerator IECheckConditionsToOpenPopup(float time)
    {
        yield return new WaitForSeconds(time);
        CheckConditionsToOpenPopup();
    }

    IEnumerator WaitToOpenFlashSalePopup(SFSObject data, float time)
    {
        yield return new WaitForSeconds(time);
        //PaymentUtils.Instance.FlashSaleCreateSuccess(data);
    }

    IEnumerator WaitToOpenStartGiftPopup(SFSObject data, float time)
    {
        yield return new WaitForSeconds(time);
        // StartupGiftTriggerPopup.Open(view => view.UpdateUILocal(data));
    }

    private void PingServer()
    {
        // if (!_ENABLE_CLIENT_PING) return;
        // StopPingServer();
        // _clientConnectionCoroutine = StartCoroutine(IEAutoPingServer());
        //
        // IEnumerator IEAutoPingServer()
        // {
        //     while (isActiveAndEnabled)
        //     {
        //         SendExt(ExtCmd.CLIENT_PING);
        //         yield return Helpers.GetWaitForSeconds(_CLIENT_PING_PERIOD);
        //     }
        // }
    }

    public void StopPingServer()
    {
        if (_clientConnectionCoroutine != null)
        {
            StopCoroutine(_clientConnectionCoroutine);
            _clientConnectionCoroutine = null;
        }
    }

    private void OnPingPong(BaseEvent evt)
    {
        //var lagValue = (int)evt.Params["lagValue"];
        //OnPingPongUpdate.Dispatch(lagValue);
    }

    public void StartClientConnectTimeOut()
    {
        if (!_ENABLE_CLIENT_PING) return;
        StopClientConnectTimeOut();
        _clientConnectTimeoutTween = DOVirtual.DelayedCall(_LOST_CONNECTION_TIME, () =>
            {
                OnLostConnectionFromServer.Dispatch();
                Logger.Log("Disconnect from client ping");
            }
        );
    }

    private void OnConnection(BaseEvent evt)
    {
        foreach (var kv in evt.Params)
        {
            Debug.Log($"[SFS CONNECTION] {kv.Key} = {kv.Value}");
        }
        if ((bool)evt.Params["success"])
        {
            var packet = new SFSObject();

            packet.PutInt("v", 326);
            packet.PutUtfString("dt", "Android");
            packet.PutUtfString("va", "2.0.2");
            packet.PutUtfString("bv", "2.0.1_75_Android");
            packet.PutUtfString("s", LoginModel.Instance.loginSession);
            Sfs.Send(new LoginRequest(UserModel.Instance.Uid.ToString(), "", "sfsak", packet));
        }
        else
        {
            Debug.Log($"SFS2X API version: {Sfs.Version}");
            Logger.LogError("Connection failed!");
        }
    }

    public void AddEventListener(string e, EventListenerDelegate listener)
    {
        if (Sfs == null)
        {
            Logger.LogError("AddEventListener Smart fox is null");
            return;
        }

        Sfs.AddEventListener(e, listener);
    }

    public void RemoveEventListener(string e, EventListenerDelegate listener)
    {
        Sfs?.RemoveEventListener(e, listener);
    }

    private void OnLogout(BaseEvent evt)
    {
        Logger.Log("Logout Success");
        StopClientConnectTimeOut();
        Disconnect();
        OnLostConnectionFromServer.Dispatch();
        Toast.ShowString("Log out");
      //  LoginModel.Instance.Logout();
    }

    public void Disconnect()
    {
        if (Sfs == null) return;
        if (Sfs.IsConnected) Sfs.Disconnect();

        Sfs = null;
    }

    public void Send(IRequest obj)
    {
        if (Sfs != null)
        {
            Sfs.Send(obj);
        }
    }

    public void SendExt(string cmd, IToSFSObject vo = null)
    {
        if (Sfs == null)
        {
            Logger.Log("-> SendExt null: " + cmd);
            return;
        }

        Sfs.Send(new ExtensionRequest(cmd, vo == null ? new SFSObject() : vo.ToSFSObject()));
        if (cmd != ExtCmd.PONG && cmd != ExtCmd.CLIENT_PING)
            Logger.Log($"-> SendExt: {cmd}, data: {vo?.ToSFSObject().ToJson()}");
        else
            Debug.LogWarning($"-> SendExt: {cmd}");
    }

    public void SendExt(string cmd, SFSObject vo)
    {
        if (Sfs == null)
        {
            Logger.Log("-> SendExt null: " + cmd);
            return;
        }

        Sfs.Send(new ExtensionRequest(cmd, vo ?? new SFSObject()));
        Logger.Log("-> SendExt: " + cmd + ", data: " + vo?.ToJson());
    }

    private static void OnLoginError(BaseEvent evt)
    {
        Debug.LogError("OnLoginError");
        Toast.ShowString("Can not connect GameServer: " + (string)evt.Params["errorMessage"]);
        Loading.Close();
    }

    public void StopOnPauseAppClientConnectTimeOut()
    {
        _clientConnectTimeoutTweenWhenPauseApp?.Kill();
    }

    public void StopClientConnectTimeOut()
    {
        _clientConnectTimeoutTween?.Kill();
    }

    private void OnConnectionRetry(BaseEvent evt)
    {
        StopClientConnectTimeOut();
    }

    private void OnConnectionResume(BaseEvent evt)
    {
        StopClientConnectTimeOut();
    }

    private void OnConnectionLost(BaseEvent evt)
    {
        Debug.LogError("OnConnectionLost");
        var reason = (string)evt.Params["reason"];
        if (reason != ClientDisconnectionReason.MANUAL)
        {
            OnLostConnectionFromServer.Dispatch();
        }
    }

    private void OnUserVariableUpdate(BaseEvent evt)
    {
        var u = (User)evt.Params["user"];
        var changedVars = (List<string>)evt.Params["changedVars"];
        if (u.IsItMe)
        {
            //UserModel.Instance.UpdateUserInfo(u, changedVars);
        }
    }

    private void OnLogin(BaseEvent evt)
    {
        Debug.Log("OnLogin Smart fox");
        Sfs.EnableLagMonitor(true);
        //if (_connectTimeout != null) StopCoroutine(_connectTimeout);
        //StartClientConnectTimeOut();
        //PingServer();
        OnConnectionSuccess.Dispatch();
        
        SceneManager.LoadScene("LeaderBoardScene");
    }

    public Room GetLastJoinedRoom()
    {
        return Sfs?.LastJoinedRoom;
    }

    public void LeaveLastJoinedRoom()
    {
        LeaveSFSRoom(GetLastJoinedRoom());
    }

    public void LeaveSFSRoom(Room room)
    {
        if (room == null)
        {
            return;
        }

        Sfs.Send(new LeaveRoomRequest(room));
    }

    // private void OnApplicationPause(bool pauseStatus)
    // {
    //     if (pauseStatus == false)
    //     {
    //         if (!_ENABLE_CLIENT_PING) return;
    //         StopPingServer();
    //         _clientConnectionCoroutine = StartCoroutine(IEAutoPingServer());
    //
    //         IEnumerator IEAutoPingServer()
    //         {
    //             while (isActiveAndEnabled)
    //             {
    //                 SendExt(ExtCmd.CLIENT_PING);
    //                 yield return Helpers.GetWaitForSeconds(_CLIENT_PING_PERIOD);
    //             }
    //         }
    //
    //         StopClientConnectTimeOut();
    //         _clientConnectTimeoutTweenWhenPauseApp = DOVirtual.DelayedCall(8f, () =>
    //             {
    //                 if (isPauseApp == false) return;
    //                 OnLostConnectionFromServer.Dispatch();
    //
    //                 Logger.Log("Disconnect from client ping");
    //             }
    //         );
    //     }
    //     else
    //     {
    //         isPauseApp = pauseStatus;
    //     }
    // }

//     private void OnApplicationPause(bool pauseStatus)
//     {
// #if !UNITY_EDITOR
//         isPauseApp = pauseStatus;
//
//         if (pauseStatus)
//         {
//             // App đi vào background
//             StopPingServer();
//             StopOnPauseAppClientConnectTimeOut();
//         }
//         else
//         {
//             // App quay lại foreground
//             if (!_ENABLE_CLIENT_PING) return;
//
//
//             // Case resume nhanh, vẫn còn session
//             SendExt(ExtCmd.CLIENT_PING);
//
//             _clientConnectTimeoutTweenWhenPauseApp?.Kill();
//             _clientConnectTimeoutTweenWhenPauseApp = DOVirtual.DelayedCall(8f, () =>
//             {
//                 OnLostConnectionFromServer.Dispatch();
//                 Logger.Log("Disconnect: no CLIENT_PONG after resume");
//             });
//             PingServer();
//         }
// #endif
//     }
}