using UnityEngine;
using UnityEngine.UI;

public class LosePopup : BasePopup<LosePopup>
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
