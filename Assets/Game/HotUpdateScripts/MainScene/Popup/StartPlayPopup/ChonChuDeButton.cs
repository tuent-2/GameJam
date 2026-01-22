using System;
using UnityEngine;
using UnityEngine.UI;

public class ChonChuDeButton : MonoBehaviour
{
    [SerializeField] private int subjectId;
    [SerializeField] private Button btnSelect;


    private void Start()
    {
        btnSelect.onClick.AddListener(OnClickSelect);
    }

    private void OnClickSelect()
    {
        GameControllerModel.Instance.SendStartPlay(subjectId);
        StartPlayPopup.Close();
        LoadinngPopup.Open();
    }
}
