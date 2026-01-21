using System;

using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LoginModel : Singleton<LoginModel>
{
    public const int EXPIRE_TIME = 1600;
    public string LoginToken { get; private set; } = "";
    public bool needUpdate { get; private set; } = false;
    public string loginSession { get; private set; }  = "";

    /*public string AuthId { get; private set; } = "";*/
    public bool IsLogin => !string.IsNullOrEmpty(LoginToken);
    public LoginType loginType = LoginType.Default;
    public bool isOpenProtectPopup = false;
    public string IPAddress { get; set; } = "";

    public void LoginWithUsernamePassword(string username, string password, bool isRemember = false,
        Action onLoginSuccess = null)
    {
        if (IsLogin) return;
        Loading.Open();
        Api.Login((d) =>
        {
            Logger.Log($"on login API success");
            if (isRemember)
            {
                LocalStorageUtils.LoginUsername = username;
                LocalStorageUtils.LoginPassword = password;
            }

            OnLoginResponseSuccess(d);
            loginType = LoginType.Normal;
            onLoginSuccess?.Invoke();
        }, OnLoginResponseFailure, username, password, GameUtils.GetDeviceId());
    }

    public void ProtectAccount(string username, string password, Action onProtectAccount = null, Action OnFaile = null)
    {
        Api.ProtectAccount((d) =>
            {
                LoginToken = (string)d["token"];
                loginType = LoginType.Normal;
                SmartFoxConnection.Instance.SendExt("udu");

                if (onProtectAccount != null) onProtectAccount();
            }, error =>
            {
                if (OnFaile != null) OnFaile();
                var e = (string)error["message"];
                Toast.ShowString(e);
            }, username,
            password);
    }

    public void ChangePassWord(string oldPassword, string newPassword, string ConfirmPassword,
        Action onLoginSuccess = null)
    {
        Api.ChangePassword((d) =>
        {
            Toast.Show(GameMsg.CHANGE_PASSWORD_SUCCESS);

            Logout();
        }, OnLoginResponseFailure, oldPassword, newPassword, ConfirmPassword);
    }

    public void LoginGuest()
    {
        Api.LoginGuest((d) =>
        {
            OnLoginResponseSuccess(d);
            //loginType = LoginType.Guest;
            loginType = (bool)d["is_protected"] ? LoginType.Normal : LoginType.Guest;
        }, error =>
        {
            Toast.ShowString(error);
            Loading.Close();
        }, GameUtils.GetDeviceId());
    }

    // public void LoginFaceBook(JObject jObject)
    // {
    //     var fbDataRequest = new FbDataRequest(AccessToken.CurrentAccessToken.TokenString).ToJson();
    //     Api.FbLogin((d) =>
    //     {
    //         OnLoginResponseSuccess(d);
    //         loginType = LoginType.FaceBook;
    //     }, error =>
    //     {
    //         Toast.ShowString(error);
    //         Loading.Close();
    //     }, fbDataRequest);
    // }

    public void RegisterWithUsernamePassword(string username, string password, string confirmPassword,
        bool isAgreePolicy)
    {
        if (StringUtils.CheckRegisterForm(username, password, confirmPassword))
        {
            Loading.Open();
            //var pr = RegisterDataRequest.Create(username, password).ToJson();
            Api.Register((d) =>
            {
                Loading.Close();
                var error = (string)d["error"];
                if (string.IsNullOrEmpty(error))
                    LoginWithUsernamePassword(username, password);
                else
                {
                    switch (error)
                    {
                        case "ev":
                            Toast.Show(GameMsg.REGISTER_INVALID_VERSION);
                            break;
                        case "ldv":
                            Toast.Show(GameMsg.REGISTER_TOMANYACCOUNT);
                            break;
                        case "ld":
                            Toast.Show(GameMsg.REGISTER_DAILYLIMIT);
                            break;

                        default:
                            Toast.ShowString("Can't register");
                            break;
                    }
                }
            }, OnRegisterResponseFailure,"123");
        }
    }

    private void OnLoginResponseFailure(string error)
    {
        Debug.Log("OnLogin Loi");
        Loading.Close();
        ShowErrorMessage(error);
    }

    private void OnRegisterResponseFailure(string error)
    {
        Loading.Close();
        ShowErrorMessage(error);
    }

    private void ShowErrorMessage(string error)
    {
        if (error.Contains(HttpErrorCode.UNAUTHORIZED.ToString()))
        {
            Toast.Show(GameMsg.USERNAME_OR_PASS_WRONG);
            return;
        }

        if (error.Contains(HttpErrorCode.SERVICE_UNAVAILABLE.ToString()) ||
            error.Contains(HttpErrorCode.INTERNAL_SERVER_ERROR.ToString()) ||
            error.Contains(HttpErrorCode.BAD_GATEWAY.ToString()))
        {
            Toast.Show(GameMsg.SERVER_LOGIN_ERROR);
            return;
        }

        Toast.ShowString(error);
    }

    private void OnLoginResponseSuccess(JObject data)
    {
        LoginToken = (string)data["token"];
        UserModel.Instance.Uid = (int)data["uid"];
        Debug.Log((bool) data["needUpdate"]);
        needUpdate = (bool) data["needUpdate"];
        loginSession = (string)data["sid"];
        SmartFoxConnection.Instance.Connect();
        // if (LoginToken == null)
        // {
        //     Debug.LogError("Login token is null");
        //     return;
        // }
        //
        // CheckAndRefreshToken();
       // PortalModel.Instance.GetPortalConfigInfo(data);
    }


    public void Logout()
    {
        StopIntervalRefreshToken();
        loginType = LoginType.Default;
        LoginToken = "";
        PopupManager.CloseAllPopup();
        // IconDragableManager.CloseAll();
        // IconDragableModel.Instance.Clear();
        // FocusItem.Instance.HideView();
        // PortalScreen.isOpenFrist = false;
        HeaderNoticeUtils.Instance.ClearMessages();

        ScreenManager.Instance.OpenScreen(ScreenId.LoginScreen);
        SmartFoxConnection.Instance.Disconnect();

        LocalStorageUtils.DeleteKey(LocalStorageEnum.Password.ToString());
        LocalStorageUtils.DeleteKey(LocalStorageEnum.Username.ToString());
    }

    #region Refresh Token

    private SDTimer _refreshTimer;
    private int _currentRefreshCount = 0;
    private const int _MAX_RETRY_REFRESH_TOKEN = 3;

    private void CheckAndRefreshToken()
    {
        _refreshTimer?.StopTimer();
        _refreshTimer = new SDTimer(EXPIRE_TIME);
        _refreshTimer.AddCompleteEvent(() =>
        {
            RequestRefreshToken();
            CheckAndRefreshToken();
        });
    }

    public void CheckAndRefreshTokenWhenDisconnect()
    {
        _refreshTimer?.StopTimer();
        _refreshTimer = new SDTimer(EXPIRE_TIME);
        _refreshTimer.AddCompleteEvent(() =>
        {
            RequestRefreshTokenWhenDisconnect();
            CheckAndRefreshTokenWhenDisconnect();
        });
    }

    private void RequestRefreshTokenWhenDisconnect()
    {
        Debug.Log("Refresh Token");
        Api.RefreshToken(OnRefreshTokenSuccessWhenDisconnect, OnRefreshTokenFailureWhenDisconnect, LoginToken);
    }

    private void OnRefreshTokenSuccessWhenDisconnect(JToken data)
    {
        var token = (string)data["token"];
        if (string.IsNullOrEmpty(token))
        {
            // Recall Refresh token
            _currentRefreshCount++;
            if (_currentRefreshCount < _MAX_RETRY_REFRESH_TOKEN)
            {
                RequestRefreshToken();
            }
        }
        else
        {
            _currentRefreshCount = 0;
            LoginToken = token;
            SmartFoxConnection.Instance.SendExt("ar");
        }

        Debug.Log($"<- Refresh Token: {token}");
    }


    private void RequestRefreshToken()
    {
        Debug.Log("Refresh Token");
        Api.RefreshToken(OnRefreshTokenSuccess, OnRefreshTokenFailure, LoginToken);
    }

    private void OnRefreshTokenSuccess(JToken data)
    {
        var token = (string)data["token"];
        if (string.IsNullOrEmpty(token))
        {
            // Recall Refresh token
            _currentRefreshCount++;
            if (_currentRefreshCount < _MAX_RETRY_REFRESH_TOKEN)
            {
                RequestRefreshToken();
            }
        }
        else
        {
            _currentRefreshCount = 0;
            LoginToken = token;
        }

        Debug.Log($"<- Refresh Token: {token}");
    }

    private void OnRefreshTokenFailure(string error)
    {
        _currentRefreshCount++;
        if (_currentRefreshCount < _MAX_RETRY_REFRESH_TOKEN)
        {
            RequestRefreshToken();
        }
    }

    private void OnRefreshTokenFailureWhenDisconnect(string error)
    {
        ScreenManager.Instance.OpenScene(SceneId.PortalScene, ScreenId.PortalScreen);
        _currentRefreshCount++;
        if (_currentRefreshCount < _MAX_RETRY_REFRESH_TOKEN)
        {
            RequestRefreshToken();
        }
    }

    private void StopIntervalRefreshToken()
    {
        Debug.Log("Clear Refresh Token");
        _refreshTimer?.StopTimer();
    }

    #endregion

    public enum LoginType
    {
        Default,
        FaceBook,
        Guest,
        Normal
    }
}