using System;
using UnityEngine;
using UnityEngine.UI;

public class StartPlayPopup : BasePopup<StartPlayPopup>
{
    [SerializeField] private Button btnClose;

    private void Start()
    {
        btnClose.onClick.AddListener(Close);
    }
}
