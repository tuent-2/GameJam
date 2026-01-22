using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGamePopup:BasePopup<InGamePopup>
{
    [SerializeField] private TextMeshProUGUI txtQuest,txtCountQuest;
    [SerializeField] List<CauHoiButton> buttons;
    [SerializeField] private Image imgBgCountTime;
    [SerializeField] private TextMeshProUGUI txtTime;
    private Coroutine countTimeCoroutine;
    [SerializeField] private List<PlayerUI> playerUI;

    private int TotalQuestion = 5;
    private int timeStart = 30;

    private void OnEnable()
    {
        Debug.Log(GameControllerModel.Instance.quesJamResponse.Value != null);
       
            
            if (GameControllerModel.Instance.quesJamResponse.Value != null)
            {
                UpdateUIQuesJam();
            }

        if (GameControllerModel.Instance.scoreJamResponse.Value != null)
        {
            UpdateUIScoreJam();
        }

        GameControllerModel.Instance.quesJamResponse.OnChanged += UpdateUIQuesJam;
        GameControllerModel.Instance.scoreJamResponse.OnChanged += UpdateUIScoreJam;
    }

    private void OnDisable()
    {
        GameControllerModel.Instance.quesJamResponse.OnChanged -= UpdateUIQuesJam;
        GameControllerModel.Instance.scoreJamResponse.OnChanged -= UpdateUIScoreJam;
    }

    public void UpdateUIScoreJam()
    {
        Debug.Log("UpdateUIScoreJam");
        for (int i = 0; i < GameControllerModel.Instance.scoreJamResponse.Value.playDatas.Count; i++)
        {
            playerUI[i].SetUpUI(GameControllerModel.Instance.scoreJamResponse.Value.playDatas[i].id == UserModel.Instance.Uid
                ,  GameControllerModel.Instance.scoreJamResponse.Value.playDatas[i].name, GameControllerModel.Instance.scoreJamResponse.Value.playDatas[i].camelId,GameControllerModel.Instance.scoreJamResponse.Value.playDatas[i].score );
        }
    }

    public void UpdateUIQuesJam()
    {
        
        Debug.Log("UpdateUIQuesJamResponse");
        txtCountQuest.text = $"{GameControllerModel.Instance.quesJamResponse.Value.id}/5";
        

        txtQuest.text = GameControllerModel.Instance.quesJamResponse.Value.question;
        for (int i = 0; i < GameControllerModel.Instance.quesJamResponse.Value.answersDetail.Count; i++)
        {
            buttons[i].SetUpUi(i, GameControllerModel.Instance.quesJamResponse.Value.answersDetail[i].questionContent);
        }
        
        StartCountTime();
    }
    
    private void StartCountTime()
    {
        if (countTimeCoroutine != null)
            StopCoroutine(countTimeCoroutine);

        countTimeCoroutine = StartCoroutine(CountTimeCoroutine());
    }
    
    private IEnumerator CountTimeCoroutine()
    {
        float duration = timeStart; // 30s
        float timeLeft = duration;

        imgBgCountTime.fillAmount = 1f;
        txtTime.text = timeLeft.ToString("0");

        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;

            float normalized = Mathf.Clamp01(timeLeft / duration);
            imgBgCountTime.fillAmount = normalized;
            txtTime.text = Mathf.CeilToInt(timeLeft).ToString();

            yield return null;
        }

        imgBgCountTime.fillAmount = 0f;
        txtTime.text = "0";

        OnTimeOut();
    }
    
    private void OnTimeOut()
    {
        Debug.Log("TIME OUT");
    }
}
