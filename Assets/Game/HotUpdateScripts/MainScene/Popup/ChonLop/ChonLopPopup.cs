using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChonLopPopup : BasePopup<ChonLopPopup>
{
    [SerializeField] private TMP_Dropdown yearDropdown;
    [SerializeField] private Button btnSendConfirm;

    private int selectedYear;

    private void Start()
    {
        InitDropdown();
        yearDropdown.onValueChanged.AddListener(OnYearChanged);
        btnSendConfirm.onClick.AddListener(OnClickSendConfirm);
    }

    private void OnClickSendConfirm()
    {
        if (selectedYear == 0) return;

        GameControllerModel.Instance.SendSetUpClass(selectedYear);
        Close();
    }

    private void InitDropdown()
    {
        yearDropdown.ClearOptions();

        List<string> options = new List<string>();
        foreach (var year in NamSinh)
        {
            options.Add(year.ToString());
        }

        yearDropdown.AddOptions(options);

        // Mặc định chọn năm đầu tiên (2025)
        yearDropdown.value = 0;
        selectedYear = NamSinh[0];
    }

    private void OnYearChanged(int index)
    {
        if (index < 0 || index >= NamSinh.Count) return;
        selectedYear = NamSinh[index];
    }

    public static readonly List<int> NamSinh = new List<int>
    {
        2025, 2024, 2023, 2022, 2021, 2020,
        2019, 2018, 2017, 2016, 2015, 2014,
        2013, 2012, 2011, 2010,
        2009, 2008, 2007, 2006, 2005, 2004,
        2003, 2002, 2001, 2000,
        1999, 1998, 1997, 1996, 1995, 1994,
        1993, 1992, 1991, 1990,
        1989, 1988, 1987, 1986, 1985, 1984,
        1983, 1982, 1981, 1980,
        1979, 1978, 1977, 1976, 1975, 1974,
        1973, 1972, 1971, 1970,
        1969, 1968, 1967, 1966, 1965, 1964,
        1963, 1962, 1961, 1960
    };
}