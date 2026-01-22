using System;
using UnityEngine;
using UnityEngine.UI;

public class LoadinngPopup : BasePopup<LoadinngPopup>
{
    [SerializeField] private Button btnCancel;

    private void Start()
    {
        btnCancel.onClick.AddListener(OnClickCancel);
    }

    private void OnClickCancel()
    {
        GameControllerModel.Instance.SendCancelMatching();
        Close();
    }
}
