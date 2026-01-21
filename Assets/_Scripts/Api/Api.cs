using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sfs2X.Entities.Data;
using UnityEngine;

public static class Api
{
    public const string DEV_SUFFIX = "dev";
    static string version = Application.platform == RuntimePlatform.IPhonePlayer ? "1.0.3" : "1.0.4";

    public static void GetCountryCodeFromCurrentIP(Action<string> onSuccess, Action<string> onFailure)
    {
        //https://cp.xm82asz.org/remote
        HttpUtils.SendRequest("GET", EncryptUtils.Decrypt("gr0EUFGASebMbUfFhnAo7jEMVNEqfBg87XTWJ4IfgXs="),
            onSuccess: (responseText) => { onSuccess?.Invoke(responseText); }, onFailure: onFailure);
    }

    public static void GetPortalConfigInfo(Action<JObject> onSuccess, Action<string> onFailure)
    {
        //https://cp.xm82asz.org/cfg/portal
        // HttpUtils.SendRequest("GET",
        //     $"{EncryptUtils.Decrypt("gr0EUFGASebMbUfFhnAo7obxQjlKw1AJiXH1YTNuojtjZXZYirGWuqR/TWbZiqEC")}{(PortalModel.IS_DEV ? DEV_SUFFIX : "")}",
        //     onSuccess: (responseText) => { onSuccess?.Invoke(JObject.Parse(responseText)); }, onFailure: onFailure);

        string baseUrl = EncryptUtils.Decrypt("gr0EUFGASebMbUfFhnAo7obxQjlKw1AJiXH1YTNuojtjZXZYirGWuqR/TWbZiqEC");
        string fullUrl = baseUrl + "?t=" + DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        HttpUtils.SendRequest("GET", fullUrl,
            onSuccess: (responseText) => { onSuccess?.Invoke(JObject.Parse(responseText)); }, onFailure: onFailure);
    }
    
    public static void Login(Action<JObject> onSuccess, Action<string> onFailure,
        string username, string password, string deviceId)
    {
        var packet = new SFSObject();
        //https://acc.xm82asz.org/api/user/
            
        // packet.PutInt("app", 6);
        // packet.PutUtfString("deviceId", GameUtils.GetDeviceId());
        // packet.PutUtfString("version", "2.1.0");
        // //packet.PutUtfString("npt", null);
        // packet.PutUtfString("packageName", "");
        // packet.PutUtfString("os", GameUtils.GetPlatform());
        // {"username":"tuent2","pass":"132456","agent":{"app":6,"os":"Android","version":"2.0.2","deviceId":"73eded60897550606bf8c97056bc293702463e95","npt":"chanhoi","packageName":"chan.hoi.online"}}
        var agent = new JObject
        {
            { "app", 6 },
            { "os", GameUtils.GetPlatform() },
            { "version", "2.0.2" },
            { "deviceId", GameUtils.GetDeviceId() },
            { "npt", "chanhoi" },
            { "packageName", "chan.hoi.online" }
        };

        var data = new JObject
        {
            { "username", username },
            { "pass", "" },  
            { "agent", agent }
        };
        
        
        HttpUtils.SendRequest("POST",
            
            "https://dev.sandinhstudio.com/api-c3/user/login",
            data.ToString(),
            (responseText) => { onSuccess?.Invoke(JObject.Parse(responseText)); }, onFailure);
    }

    public static void LoginGuest(Action<JObject> onSuccess, Action<string> onFailure,
        string deviceId)
    {
        //https://acc.xm82asz.org/api/user/login-guest
        var o = new JObject
        {
            { "deviceId", deviceId },

            { "os", GameUtils.GetPlatform() },
            { "version", GameUtils.GetPlatform() == "IOS" ? "1.0.3" : "1.0.4" }
            //{ "version", Application.version }
        };

        HttpUtils.SendRequest("POST",
            
                 EncryptUtils.Decrypt("2WfMNoap2GNuyxHxzHWItFwCdCddhoNNJLaA789jFLKviHgF4nMOBISnZuowBjih"),
            o.ToString(),
            (responseText) => { onSuccess?.Invoke(JObject.Parse(responseText)); }, onFailure);
    }

    public static void RefreshToken(Action<JObject> onSuccess, Action<string> onFailure,
        string refreshToken)
    {
        var data = new JObject
        {
            ["refresh_token"] = refreshToken
        };
        //https://acc.xm82asz.org/api/user/refresh-token
        HttpUtils.SendRequest("POST",
            EncryptUtils.Decrypt(
                "2WfMNoap2GNuyxHxzHWItFwCdCddhoNNJLaA789jFLKk4hHIUWlrB5vWzvgBg9sf"),
            data.ToString(),
            (responseText) => { onSuccess?.Invoke(JObject.Parse(responseText)); }, onFailure);
    }

    public static void Register(Action<JObject> onSuccess, Action<string> onFailure, string jsonData = null)
    {
        //https://acc.xm82asz.org/api/user/register
        HttpUtils.SendRequest("POST",
            EncryptUtils.Decrypt("2WfMNoap2GNuyxHxzHWItFwCdCddhoNNJLaA789jFLISDUkL6a6rmQ60uPhhFVH8"), jsonData,
            (responseText) => { onSuccess?.Invoke(JObject.Parse(responseText)); }, onFailure);
    }

    public static void ChangePassword(Action<JObject> onSuccess, Action<string> onFailure, string oldPassword,
        string newPassword, string confirmPassword)
    {
        //https://acc.xm82asz.org/api/user/change-password
        var data = new JObject
        {
            { "oldPassword", oldPassword },
            { "newPassword", newPassword },
            { "confirmPassword", confirmPassword }
        };
        HttpUtils.SendRequestForChangePassword("POST",
            EncryptUtils.Decrypt(
                "2WfMNoap2GNuyxHxzHWItFwCdCddhoNNJLaA789jFLL57tGVh3p0f6HYO5Op9Ibdq+FX3qWG1Zmjfg6Mi3N1kw=="),
            data.ToString(),
            (responseText) => { onSuccess?.Invoke(JObject.Parse(responseText)); }, onFailure);
    }

    public static void ProtectAccount(Action<JObject> onSuccess, Action<JObject> onFailure, string username,
        string password)
    {
        //https://acc.xm82asz.org/api/user/link-account
        var data = new JObject
        {
            { "username", username },
            { "password", password },
        };
        HttpUtils.SendRequestNew("POST",
            EncryptUtils.Decrypt(
                "2WfMNoap2GNuyxHxzHWItFwCdCddhoNNJLaA789jFLJ39X8yP6Y3wjTWga+syikd"),
            data.ToString(),
            (responseText) => { onSuccess?.Invoke(JObject.Parse(responseText)); },
            (responseErrorText) => { onFailure?.Invoke(JObject.Parse(responseErrorText)); });
    }


    public static void UploadLogFile(string filename, byte[] data, Action onSuccess, Action<string> onFailure)
    {
        HttpUtils.UploadFileToServer(
            EncryptUtils.Decrypt("2WfMNoap2GNuyxHxzHWItNENkn09FduSqBXaNLWCEQMNgCWF9zKLHHcIQa91AiI7"), filename, "file",
            "multipart/form-data",
            data, () => { onSuccess.Invoke(); }, onFailure.Invoke);
    }

    public static void FbLogin(Action<JObject> onSuccess, Action<string> onFailure, string fbDataRequest)
    {
        HttpUtils.SendRequest("POST",
            EncryptUtils.Decrypt(
                "2WfMNoap2GNuyxHxzHWItFwCdCddhoNNJLaA789jFLIXkz+6kCgHUaqhsxOow4H1"), fbDataRequest,
            onSuccess: (responseText) => { onSuccess?.Invoke(JObject.Parse(responseText)); }, onFailure: onFailure
        );
    }

    public static void GetCsvLanguageFile(Action<string> onSuccess, Action<string> onFailure)
    {
        HttpUtils.SendRequest("GET",
            EncryptUtils.Decrypt("gr0EUFGASebMbUfFhnAo7oKTY/UiN9X5xWZbx1ZnR15Kf+GijUgwlgSJiHoRqO01") + "?t="
            + DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            onSuccess: (responseText) => { onSuccess?.Invoke(responseText); }, onFailure: onFailure);
    }
}


[Serializable]
public class AgentData
{
    public int app;
    public string os;
    public string version;
    public string deviceId;
    public string npt;
    public string packageName;

    public static AgentData Create()
    {
        var obj = new AgentData();
        obj.app = 6;
        obj.os = GameUtils.GetPlatform();
        obj.version = GameUtils.GetVersion();
        obj.deviceId = GameUtils.GetDeviceId();
        obj.packageName = "chan.hoi.online";
        obj.npt = "chanhoi";
        return obj;
    }

    public string ToJson()
    {
        return JsonConvert.SerializeObject(this);
    }
}