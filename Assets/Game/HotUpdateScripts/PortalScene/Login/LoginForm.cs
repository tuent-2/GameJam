
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class LoginForm : MonoBehaviour
{
    [SerializeField] private TMP_InputField ifAccount, ifPassword;
    [SerializeField] private Toggle tgRememberLogin;
    [SerializeField] private Button btnLogin;
    [SerializeField] private Button btnLoginAsGuest;
    [SerializeField] private Button btnLoginFb;
    [SerializeField] private Button btnShowPassword;
    [SerializeField] private Button btnPlayNow;
    [SerializeField] private Image imgShowPassword;
    [SerializeField] private Sprite hidePasswordIcon, showPasswordIcon;
    [SerializeField] private GameObject normalGo, reviewGo;
    private bool _isHidePassword = true;

    private static bool hasCheckedCountry = false;

    [SerializeField] private Button Test;

    private void TestAction()
    {
       // NotificationNewVersionPopup.Open();
    }

    private void Start()
    {
        btnLogin.onClick.AddListener(NormalLogin);
        //btnLoginAsGuest.onClick.AddListener(LoginGuest);
        // btnLoginFb.onClick.AddListener(LoginFb);
        // btnShowPassword.onClick.AddListener(SeePassword);
        // btnPlayNow.onClick.AddListener(LoginGuest);
        // Test.onClick.AddListener(TestAction);
        // LocalizationSettings.SelectedLocale =
        //     LocalizationSettings.AvailableLocales.Locales[LocalStorageUtils.Localization];


        Loading.Open();
       // UserModel.Instance.IsUseDiamond = true;

        // if (!hasCheckedCountry)
        // {
        //     PortalModel.Instance.GetCountryCodeAndLocalize(ShowLoginLayoutBaseOnCountryCode);
        // }
        // else
        // {
        //     ShowLoginLayoutBaseOnCountryCode();
        // }


        // void ShowLoginLayoutBaseOnCountryCode()
        // {
        //     /*
        //     if (PortalModel.Instance.IsMainCountry() || PortalModel.IS_DEV)
        //     {*/
        //
        //     normalGo.Hide();
        //     normalGo.Show();
        //     reviewGo.Hide();
        //     Loading.Close();
        //     if (LocalStorageUtils.HasKey(LocalStorageEnum.Username.ToString()) &&
        //         LocalStorageUtils.HasKey(LocalStorageEnum.Password.ToString()))
        //     {
        //         DoLogin(LocalStorageUtils.LoginUsername, LocalStorageUtils.LoginPassword);
        //     }
        //
        //
        //     /*}
        //     else
        //     {
        //         normalGo.Hide();
        //         reviewGo.Show();
        //         UserModel.Instance.IsUseDiamond = false;
        //     }*/
        // }
    }


    void RefreshLayout(GameObject go)
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(go.GetComponent<RectTransform>());
    }

    private void LoginFb()
    {
        //FacebookModel.Instance.OnLoginFb();
    }

    private void OnEnable()
    {
        tgRememberLogin.isOn = false;
    }

    private void NormalLogin()
    {
        DoLogin(ifAccount.text, ifPassword.text, tgRememberLogin.isOn);
    }

    private void DoLogin(string username, string password, bool isRemember = false)
    {
       // FirebaseAnalyticsUtils.Instance.LogEvent("Login_Normal_Remember_" + isRemember);
        LoginModel.Instance.LoginWithUsernamePassword(username, password, isRemember);
    }

    private void LoginGuest()
    {
        Loading.Open();

        LoginModel.Instance.LoginGuest();
    }

    private void LoginWithTelegram()
    {
    }

    private void SeePassword()
    {
        if (_isHidePassword)
        {
            //FirebaseAnalyticsUtils.Instance.LogEvent("Login_ShowPassword");
            ifPassword.contentType = TMP_InputField.ContentType.Standard;
            imgShowPassword.sprite = hidePasswordIcon;
            _isHidePassword = false;
        }
        else
        {
            //FirebaseAnalyticsUtils.Instance.LogEvent("Login_HidePassword");
            ifPassword.contentType = TMP_InputField.ContentType.Password;
            imgShowPassword.sprite = showPasswordIcon;
            _isHidePassword = true;
        }

        ifPassword.ForceLabelUpdate();
    }
}