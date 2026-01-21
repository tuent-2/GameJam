using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class NotifyPopup : BasePopup<NotifyPopup>
{
    [SerializeField] private Button btnClose, bgClose;
    [SerializeField] private LocalizeText ltTitle, ltMessage;

    private void Start()
    {
        btnClose.onClick.AddListener(Close);
        bgClose.onClick.AddListener(Close);
    }

    public void InitData(LocalizedString title, LocalizedString message)
    {
        ltTitle.UpdateStringReference(title);
        ltMessage.UpdateStringReference(message);
    }
}