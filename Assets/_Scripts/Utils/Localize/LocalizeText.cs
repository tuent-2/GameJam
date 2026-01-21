using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(TextMeshProUGUI), typeof(LocalizeStringEvent))]
public class LocalizeText : MonoBehaviour
{
    public const string TABLE_NAME = "Localization";
    public const int ENGLISH_POSITION = 0;
    public const int VN_POSITION = 1;
  //  public const int BENGALI_POSITION = 2;
    [SerializeField] private LocalizeStringEvent stringEvent;
    [SerializeField] private TextMeshProUGUI txtContent;
    [SerializeField] private LocalizedString localizedString;
    [SerializeField] private string stringKey;
    [SerializeField] private TMP_FontAsset bengaliFont;
    private TMP_FontAsset _khmerFont;
    //private bool isOpenBengali = false;

    private void OnValidate()
    {
        stringEvent ??= GetComponent<LocalizeStringEvent>();
        txtContent ??= GetComponent<TextMeshProUGUI>();

#if UNITY_EDITOR
        bengaliFont ??=
            AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(
                "Assets/Game/Fonts/Noto_Sans_Bengali/NotoSansBengali-VariableFont_wdth,wght SDF.asset");
#endif
    }

    private void Awake()
    {
        _khmerFont = txtContent.font;
        if (!string.IsNullOrEmpty(stringKey))
        {
            UpdateStringReferenceByKey(stringKey);
        }
        else
        {
            UpdateStringReference(localizedString);
        }
    }

    private void OnEnable()
    {
        stringEvent.OnUpdateString.AddListener(OnUpdateLocalizeString);
        UpdateString();
    }

    private void OnDisable()
    {
        stringEvent.OnUpdateString.RemoveListener(OnUpdateLocalizeString);
    }

    public void UpdateColorGradientPreset(TMP_ColorGradient colorGradient)
    {
        txtContent.colorGradientPreset = colorGradient;
    }

    public void UpdateStringReference(LocalizedString localString, bool useGradientColor = false,
        string prependText = "", string appendText = "")
    {
        stringEvent.StringReference = localString;
        UpdateString(useGradientColor, prependText, appendText);
    }

    public void UpdateStringReferenceByKey(string key, bool useGradientColor = false, string prependText = "",
        string appendText = "")
    {
        stringEvent.StringReference = new LocalizedString(TABLE_NAME, key);
        UpdateString(useGradientColor, prependText, appendText);
    }

    private void OnUpdateLocalizeString(string text)
    {
        UpdateString();
    }

    private void UpdateString(bool useGradientColor = false, string prependText = "", string appendText = "")
    {
        if (stringEvent.StringReference.IsEmpty)
        {
            return;
        }


        var currentLocale = LocalizationSettings.SelectedLocale;


        // if (currentLocale == LocalizationSettings.AvailableLocales.Locales[BENGALI_POSITION])
        // {
        //     txtContent.font = bengaliFont;
        // }
        // else
        // {
        //     txtContent.font = _khmerFont;
        // }


        string localizedText = stringEvent.StringReference.GetLocalizedString();


        if (string.IsNullOrEmpty(localizedText))
        {
            return;
        }

        // Ghép nội dung
        string content = $"{prependText}{localizedText}{appendText}";


        // Chọn màu
        var usedColor = useGradientColor ? txtContent.colorGradientPreset.bottomRight : txtContent.color;

        // Áp dụng xử lý font Khmer + màu
        txtContent.text = content;
    }


    public void UpdateStringText(string text)
    {
        txtContent.text = text;
    }
}