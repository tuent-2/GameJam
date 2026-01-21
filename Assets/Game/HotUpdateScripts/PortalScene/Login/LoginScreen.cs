using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginScreen : UIScreen
{
    [SerializeField] private Button btnChangeForm, btnBackLogin;
    [SerializeField] private LoginForm loginForm;
  //  [SerializeField] private RegisterForm registerForm;


    private void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        btnChangeForm.onClick.AddListener(ChangeLoginRegisterForm);
        btnBackLogin.onClick.AddListener(OnBackLogin);
        //Debug.Log(EncryptUtils.Decrypt("2WfMNoap2GNuyxHxzHWItFwCdCddhoNNJLaA789jFLISDUkL6a6rmQ60uPhhFVH8"));
        Debug.Log(EncryptUtils.Decrypt("sO0yqfvPGI/1o6xV/QT7VaMcWXrsIM/ascRjP7nxvnZdL9QaC3ThLldL6uOfalax"));
        Debug.Log(EncryptUtils.Encrypt("https://15.235.162.188/api/user/register"));
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        SmartFoxConnection.Instance.StopPingServer();
        SmartFoxConnection.Instance.StopClientConnectTimeOut();
        SmartFoxConnection.OnConnectionSuccess.AddListener(EnterPortal);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        SmartFoxConnection.OnConnectionSuccess.RemoveListener(EnterPortal);
    }

    private void EnterPortal()
    {
        ScreenManager.Instance.OpenScreen(ScreenId.PortalScreen);
    }

    private void ChangeLoginRegisterForm()
    {
        loginForm.Hide();
        //registerForm.Show();
    }

    private void OnBackLogin()
    {
        loginForm.Show();
        //registerForm.Hide();
    }
}