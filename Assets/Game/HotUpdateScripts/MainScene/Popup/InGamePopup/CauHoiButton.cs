using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CauHoiButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtContent;
    private int id;
    [SerializeField] private Button SelectButton;
    public static event Action<int> OnSelectAnswerButton;
    public void SetUpUi(int idCauHoi, string noiDung)
    {
        //Debug.Log($"CauHoiButton {idCauHoi} {noiDung}");
        SelectButton.interactable = true;
        txtContent.text = noiDung;
        id = idCauHoi;
    }

    private void Start()
    {
        SelectButton.onClick.AddListener(OnSelectButton);
    }

    private void OnEnable()
    {
        OnSelectAnswerButton += ListenOnSelectAnswerButton;
    }

    private void OnDisable()
    {
        OnSelectAnswerButton -= ListenOnSelectAnswerButton;
    }

    private void ListenOnSelectAnswerButton(int obj)
    {
        SelectButton.interactable = false;
    }

    private void OnSelectButton()
    {
        OnSelectAnswerButton?.Invoke(id);
        GameControllerModel.Instance.SendPlayRequest(id, GameControllerModel.Instance.quesJamResponse.Value.id);
    }
}
