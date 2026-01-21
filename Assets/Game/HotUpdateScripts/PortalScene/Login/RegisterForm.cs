// using TMPro;
// using UnityEngine;
// using UnityEngine.UI;
//
// public class RegisterForm : MonoBehaviour
// {
//     [SerializeField] private TMP_InputField ifAccount, ifPassword, ifConfirmPassword;
//     [SerializeField] private Button btnRegister;
//     [SerializeField] private Toggle togAgree;
//     [SerializeField] private Button btnShowPassword, btnShowRePassword;
//     [SerializeField] private Image imgShowPassword, imgShowRePassword;
//     [SerializeField] private Sprite iconShowPassword, iconHidePassword;
//     private bool isHidePassword = true;
//     private bool isHideRePassword = true;
//
//     Vector2 originalAnchoredPos;
//     private RectTransform panelToMove;
//
//     private void Start()
//     {
//         panelToMove = GetComponent<RectTransform>();
//         originalAnchoredPos = panelToMove.anchoredPosition;
//
//         btnRegister.onClick.AddListener(OnRegister);
//         btnShowPassword.onClick.AddListener(SeePassword);
//         btnShowRePassword.onClick.AddListener(SeeRePassword);
//         ifAccount.onSelect.AddListener(OnInputSelect);
//         ifAccount.onDeselect.AddListener(OnInputDeselect);
//         ifPassword.onSelect.AddListener(OnInputSelect);
//         ifPassword.onDeselect.AddListener(OnInputDeselect);
//         ifConfirmPassword.onSelect.AddListener(OnInputSelect);
//         ifConfirmPassword.onDeselect.AddListener(OnInputDeselect);
//     }
//
//     void OnInputDeselect(string text)
//     {
//         panelToMove.anchoredPosition = originalAnchoredPos;
//     }
//
//     private void OnInputSelect(string arg0)
//     {
// #if UNITY_ANDROID || UNITY_IOS
//         if (!Application.isEditor)
//             StartCoroutine(WaitAndMoveUp());
// #endif
//     }
//
//     System.Collections.IEnumerator WaitAndMoveUp()
//     {
//         yield return new WaitForSeconds(0.25f);
//         if (TouchScreenKeyboard.area.height > 0)
//         {
//             panelToMove.anchoredPosition = originalAnchoredPos + new Vector2(0, 200);
//             Debug.Log("Moved UI up by 200");
//         }
//     }
//
//
//     private void OnRegister()
//     {
//         FirebaseAnalyticsUtils.Instance.LogEvent("Register");
//         LoginModel.Instance.RegisterWithUsernamePassword(ifAccount.text, ifPassword.text, ifConfirmPassword.text,
//             togAgree.isOn);
//     }
//
//     private void SeePassword()
//     {
//         if (isHidePassword)
//         {
//             ifPassword.contentType = TMP_InputField.ContentType.Standard;
//             imgShowPassword.sprite = iconHidePassword;
//             isHidePassword = false;
//         }
//         else
//         {
//             ifPassword.contentType = TMP_InputField.ContentType.Password;
//             imgShowPassword.sprite = iconShowPassword;
//             isHidePassword = true;
//         }
//
//         ifPassword.ForceLabelUpdate();
//     }
//
//     private void SeeRePassword()
//     {
//         if (isHideRePassword)
//         {
//             FirebaseAnalyticsUtils.Instance.LogEvent("Register_ShowPassword");
//             ifConfirmPassword.contentType = TMP_InputField.ContentType.Standard;
//             imgShowRePassword.sprite = iconHidePassword;
//             isHideRePassword = false;
//         }
//         else
//         {
//             FirebaseAnalyticsUtils.Instance.LogEvent("Register_HidePassword");
//             ifConfirmPassword.contentType = TMP_InputField.ContentType.Password;
//             imgShowRePassword.sprite = iconShowPassword;
//             isHideRePassword = true;
//         }
//
//         ifConfirmPassword.ForceLabelUpdate();
//     }
// }