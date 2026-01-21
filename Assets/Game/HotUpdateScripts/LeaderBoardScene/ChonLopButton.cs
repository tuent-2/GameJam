using System;
using UnityEngine;
using UnityEngine.UI;

public class ChonLopButton : MonoBehaviour
{
    [SerializeField] private int idLop;
    [SerializeField] private Button chonLopButton;

    private void Start()
    {
        chonLopButton.onClick.AddListener(OnClickChonLopAction);
    }

    private void OnClickChonLopAction()
    {
       //GameControllerModel.Instance.SendSetUpClass(idLop);
        ChonLopPopup.Close();
    }
}
