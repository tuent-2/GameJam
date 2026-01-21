using System;
using UnityEngine;
using UnityEngine.UI;

public class ChonLopButton : MonoBehaviour
{
    [SerializeField] private int idLop;
    [SerializeField] private Button chonLopButton;
    [SerializeField] private Image chonLopButtonImage;
    public static event Action<int> OnChonLop;
    private void Start()
    {
        chonLopButton.onClick.AddListener(OnClickChonLopAction);
    }

    private void OnEnable()
    {
        OnChonLop += UpdateUI;
    }

    private void OnDisable()
    {
        OnChonLop -= UpdateUI;
    }

    private void UpdateUI(int obj)
    {
        if (obj == idLop)
        {
            chonLopButtonImage.color = Color.deepSkyBlue;
        }
        else 
            chonLopButtonImage.color = Color.white;
    }

    private void OnClickChonLopAction()
    {
        
        OnChonLop?.Invoke(idLop);
       
       //GameControllerModel.Instance.SendSetUpClass(idLop);
       // ChonLopPopup.Close();
    }
}
