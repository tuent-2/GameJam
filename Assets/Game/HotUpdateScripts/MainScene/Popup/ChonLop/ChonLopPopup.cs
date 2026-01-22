using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChonLopPopup : BasePopup<ChonLopPopup>
{
    [SerializeField] private TMP_Dropdown provinceDropdown;
    [SerializeField] private Button btnSendConfirm;
    private int selectedProvince = -1;
    private int selectedLop;
    private void Start()
    {
        InitDropdown();
        provinceDropdown.onValueChanged.AddListener(OnProvinceChanged);
        btnSendConfirm.onClick.AddListener(OnClickSendConfirm);
    }

    private void OnClickSendConfirm()
    {
        if (selectedLop == 0 || selectedProvince ==-1  ) return;
        GameControllerModel.Instance.SendSetUpClass(selectedLop, selectedProvince);
        Close();
    }

    private void OnEnable()
    {
        ChonLopButton.OnChonLop += HandleChonLop;
    }
    
    private void OnDisable()
    {
        ChonLopButton.OnChonLop -= HandleChonLop;
    }
    
    private void HandleChonLop(int idLop)
    {
        selectedLop = idLop;
    }


    private void InitDropdown()
    {
        provinceDropdown.ClearOptions();

        List<string> options = new List<string>();
        options.Add("Chọn tỉnh / thành phố");

        options.AddRange(VietNamProvinces);

        provinceDropdown.AddOptions(options);
        provinceDropdown.value = 0;
    }

    private void OnProvinceChanged(int index)
    {
        selectedProvince = index;
    }

    public static readonly List<string> VietNamProvinces = new List<string>
    {
        "Hà Nội",
        "TP. Hồ Chí Minh",
        "Hải Phòng",
        "Đà Nẵng",
        "Cần Thơ",
        "Huế",
    };
}
