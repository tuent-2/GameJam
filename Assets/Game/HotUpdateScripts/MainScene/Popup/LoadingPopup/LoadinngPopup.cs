using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadinngPopup : BasePopup<LoadinngPopup>
{
    [SerializeField] private Button btnCancel;
    [SerializeField] private List<Sprite> _sprites;
    [SerializeField] private Image imgAvatar;
    private void Start()
    {
        btnCancel.onClick.AddListener(OnClickCancel);
    }

    private void OnEnable()
    {
        GameControllerModel.Instance.scoreJamResponse.OnChanged += OpenUI;
        imgAvatar.sprite = _sprites[GameControllerModel.Instance.userDataResponse.Value.camelId / _sprites.Count];
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
