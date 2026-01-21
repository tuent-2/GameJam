using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class HttpUtils : MonoSingleton<HttpUtils>
{
    public static void SendRequest(string method, string url, string jsonData = "",
        Action<string> onSuccess = null, Action<string> onFailure = null)
    {
        Instance.StartCoroutine(Request(method, url, onSuccess, onFailure, jsonData));
    }

    public static void SendRequestForChangePassword(string method, string url, string jsonData = "",
        Action<string> onSuccess = null, Action<string> onFailure = null)
    {
        Instance.StartCoroutine(RequestForChangePassword(method, url, onSuccess, onFailure, jsonData));
    }

    public static void SendRequestNew(string method, string url, string jsonData = "",
        Action<string> onSuccess = null, Action<string> onFailure = null)
    {
        Instance.StartCoroutine(RequestNew(method, url, onSuccess, onFailure, jsonData));
    }

    public static void SendRequest(string method, string url, Dictionary<string, string> data,
        Action<string> onSuccess, Action<string> onFailure = null)
    {
        Instance.StartCoroutine(RequestEncode(method, url, onSuccess, onFailure, data));
    }

    private static IEnumerator Request(string method, string url,
        Action<string> onSuccess, Action<string> onFailure, string jsonData)
    {
        Logger.Log(method + ": " + url + "\n" + jsonData);
        using (UnityWebRequest request = new UnityWebRequest(url, method))
        {
            var bodyRaw = string.IsNullOrEmpty(jsonData) ? null : Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.certificateHandler = new BypassCertificate();
            //var session = LoginModel.Instance.LoginToken;
            var session = "1";
            request.SetRequestHeader("content-type", "application/json");
            request.SetRequestHeader("SdAuthorization", session);

            yield return request.SendWebRequest();

            if (request.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
            {
                Logger.LogError("Error: " + request.error);
                onFailure?.Invoke(request.error);
            }
            else
            {
                Logger.Log("Received: " + request.downloadHandler.text);
                var data = request.downloadHandler.text;
                onSuccess(data);
            }
        }
    }

    private static IEnumerator RequestForChangePassword(string method, string url,
        Action<string> onSuccess, Action<string> onFailure, string jsonData)
    {
        Logger.Log(method + ": " + url + "\n" + jsonData);
        using (UnityWebRequest request = new UnityWebRequest(url, method))
        {
            var bodyRaw = string.IsNullOrEmpty(jsonData) ? null : Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.certificateHandler = new BypassCertificate();
            //var session = LoginModel.Instance.LoginToken;
            var session = "1";
            request.SetRequestHeader("content-type", "application/json");
            request.SetRequestHeader("Authorization", session);

            yield return request.SendWebRequest();

            if (request.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
            {
                string serverResponse = request.downloadHandler?.text;

                Logger.LogError("Server message: " + serverResponse);
                Logger.LogError("Error: " + request.error);
                onFailure?.Invoke(request.error);
            }
            else
            {
                Logger.Log("Received: " + request.downloadHandler.text);
                var data = request.downloadHandler.text;
                onSuccess(data);
            }
        }
    }

    private static IEnumerator RequestNew(string method, string url,
        Action<string> onSuccess, Action<string> onFailure, string jsonData)
    {
        Logger.Log(method + ": " + url + "\n" + jsonData);
        using (UnityWebRequest request = new UnityWebRequest(url, method))
        {
            var bodyRaw = string.IsNullOrEmpty(jsonData) ? null : Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.certificateHandler = new BypassCertificate();
            //var session = LoginModel.Instance.LoginToken;
            var session = "1";
            request.SetRequestHeader("content-type", "application/json");
            request.SetRequestHeader("Authorization", session);

            yield return request.SendWebRequest();

            if (request.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
            {
                string serverResponse = request.downloadHandler?.text;

                // Logger.LogError("Server message: " + serverResponse);
                // Logger.LogError("Error: " + request.error);
                onFailure?.Invoke(serverResponse);
            }
            else
            {
                // Logger.Log("Received: " + request.downloadHandler.text);
                var data = request.downloadHandler.text;
                onSuccess(data);
            }
        }
    }


    private static IEnumerator RequestEncode(string method, string url,
        Action<string> onSuccess, Action<string> onFailure, Dictionary<string, string> encodeData)
    {
        Logger.Log(method + ": " + url);
        using (var request = new UnityWebRequest(url, method))
        {
            var form = new WWWForm();
            MergeFormData(form, encodeData);
            request.uploadHandler = new UploadHandlerRaw(form.data);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.certificateHandler = new BypassCertificate();
            //var session = LoginModel.Instance.LoginToken;
            var session = "1";
            request.SetRequestHeader("content-type", "application/x-www-form-urlencoded");
            request.SetRequestHeader("SdAuthorization", session);

            yield return request.SendWebRequest();

            if (request.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
            {
                Logger.LogError("Error: " + request.error);
                onFailure?.Invoke(request.error);
            }
            else
            {
                Logger.Log("Received: " + request.downloadHandler.text);
                var data = request.downloadHandler.text;
                onSuccess(data);
            }
        }
    }


    public static void UploadFileToServer(string url, string filename, string paramName, string contentType,
        byte[] dataBytes,
        Action onSuccess, Action<string> onFailure)
    {
       //var session = LoginModel.Instance.LoginToken;
        var session = "1";
        var form = new WWWForm();
        form.AddBinaryData(paramName, dataBytes, filename, contentType);
        var request = new UnityWebRequest(url) { method = "POST" };
        request.certificateHandler = new BypassCertificate();
        var uploader = new UploadHandlerRaw(form.data);
        var downloader = new DownloadHandlerBuffer();
        uploader.contentType = form.headers["Content-Type"];
        request.SetRequestHeader("content-type", form.headers["Content-Type"]);
        if (GameUtils.IsWeb())
        {
            request.SetRequestHeader("SdAuthorization", session);
            request.SetRequestHeader("SdType", "2");
        }

        // request.SetRequestHeader("Cookie", "xf_session=" + session);
        request.uploadHandler = uploader;
        request.downloadHandler = downloader;
        Instance.StartCoroutine(IERunRequest(request, onSuccess, onFailure));
    }

    // todo Add IFix.interpret
    public static IEnumerator IERunRequest(UnityWebRequest request, Action onComplete, Action<string> onFailure)
    {
        yield return request.SendWebRequest();
        if (request.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
        {
            var message = request.error;
            Logger.LogError(message);
            onFailure.Invoke(message);
        }
        else
        {
            onComplete.Invoke();
        }
    }

    private static WWWForm MergeFormData(WWWForm form, Dictionary<string, string> data)
    {
        foreach (var item in data)
        {
            form.AddField(item.Key, item.Value);
        }

        return form;
    }

    public static void RequestMultipleTextures(List<string> urls, Action<Dictionary<string, Texture2D>> onComplete)
    {
        Instance.StartCoroutine(GetTexture(urls, onComplete));
    }

    static IEnumerator GetTexture(List<string> requestUrls, Action<Dictionary<string, Texture2D>> onComplete)
    {
        var requests = new List<UnityWebRequestAsyncOperation>(requestUrls.Count);

        // Start all requests
        for (var i = 0; i < requestUrls.Count; i++)
        {
            var www = UnityWebRequestTexture.GetTexture(requestUrls[i]);
            www.certificateHandler = new BypassCertificate();
            Debug.Log(requestUrls[i]);
            // starts the request but doesn't wait for it for now
            requests.Add(www.SendWebRequest());
        }

        // Now wait for all requests parallel
        yield return new WaitUntil(() => AllRequestsDone(requests));

        // Now evaluate all results
        onComplete?.Invoke(HandleAllRequestsWhenFinished(requests));

        foreach (var request in requests)
        {
            request.webRequest.Dispose();
        }
    }

    private static Dictionary<string, Texture2D> HandleAllRequestsWhenFinished(
        List<UnityWebRequestAsyncOperation> requests)
    {
        var ls = new Dictionary<string, Texture2D>();
        for (var i = 0; i < requests.Count; i++)
        {
            var www = requests[i].webRequest;
            if (www.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                var myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                if (!ls.ContainsKey(www.url))
                {
                    ls.Add(www.url, myTexture);
                }
                else
                {
                    ls.Add(www.url + $"__({i})", myTexture);
                }
            }
        }

        return ls;
    }

    private static bool AllRequestsDone(List<UnityWebRequestAsyncOperation> requests)
    {
        return requests.All(r => r.isDone);
    }
}