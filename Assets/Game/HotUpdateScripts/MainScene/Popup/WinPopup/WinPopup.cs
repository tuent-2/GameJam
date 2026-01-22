using System;
using UnityEngine;
using UnityEngine.UI;

public class WinPopup : BasePopup<WinPopup>
{
    [SerializeField] private Button btnClose;

    private void Start()
    {
        btnClose.onClick.AddListener(OnClickClose);
    }

    private void OnClickClose()
    {
        PopupManager.CloseAllPopup();
    }
}
