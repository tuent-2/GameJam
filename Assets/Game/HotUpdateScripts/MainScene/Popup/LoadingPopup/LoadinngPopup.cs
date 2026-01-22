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

    private void OnEnable()
    {
        GameControllerModel.Instance.scoreJamResponse.OnChanged += OpenUI;
    }

    private void OnDisable()
    {
        GameControllerModel.Instance.scoreJamResponse.OnChanged -= OpenUI;
    }

    private void OpenUI()
    {
        InGamePopup.Open();
        Close();
        
    }


    private void OnClickCancel()
    {
        GameControllerModel.Instance.SendCancelMatching();
        Close();
    }
}
