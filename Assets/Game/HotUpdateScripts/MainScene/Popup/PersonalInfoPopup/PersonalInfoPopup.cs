using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PTT;
using TMPro;
using UnityEngine.UI;

public class PersonalInfoPopup : BasePopup<PersonalInfoPopup>
{
    [Space(20)]
    //===================================================================== Variables

    [SerializeField]
    private Button _btnClose;
    
    [SerializeField]
    private Image _imgAvatar;
    
    [Space(10)]
    [SerializeField]
    private TextMeshProUGUI _tmpTotalPlay;
    [SerializeField]
    private TextMeshProUGUI _tmpTotalWin;
    [SerializeField]
    private TextMeshProUGUI _tmpTotalLose;
    
    [Space(10)]
    [SerializeField]
    private TextMeshProUGUI _tmpGame1Info;
    [SerializeField]
    private TextMeshProUGUI _tmpGame2Info;
    [SerializeField]
    private TextMeshProUGUI _tmpGame3Info;
    
    [SerializeField] private List<Sprite> _sprites;
    //===================================================================== Unity Methods

    private void Awake()
    {
        _btnClose.onClick.AddListener(OnClickBtn);
    }

    private void OnClickBtn()
    {
        Close();
    }

    private void OnEnable()
    {
        StartCoroutine(WaitForDataAndUpdate());
    }

    private IEnumerator WaitForDataAndUpdate()
    {
        int attempts = 0;
        while (attempts < 600)
        {
            var data = GameControllerModel.Instance.userDataResponse.Value;
            if (data != null)
            {
                UpdatePopupVisual();
                yield break;
            }
            
            yield return null;
            attempts++;
        }
    }

    //===================================================================== Action Handle

    //===================================================================== Local Methods

    public void UpdatePopupVisual()
    {
        _imgAvatar.sprite = _sprites[GameControllerModel.Instance.userDataResponse.Value.camelId % _sprites.Count];
        var data = GameControllerModel.Instance.userDataResponse.Value;

        _tmpTotalPlay.text = $"Tổng số lần chơi : <color=white>{data.sumGame}</color> lượt";
        _tmpTotalWin.text = $"Tổng số lần thắng : <color=green>{data.sumGameWin}</color> lượt";
        _tmpTotalLose.text = $"Tổng số lần thua : <color=red>{data.sumGame-data.sumGameWin}</color> lượt";

        var game1Data = data.ThanhTuus[0];
        _tmpGame1Info.text = $"Cân (Nhưng dễ) tỉ lệ thắng : <color=green>{game1Data.totalWin}</color>/<color=white>{(game1Data.totalWin/game1Data.totalMatch)*100}</color> (<color=yellow>x</color>%)";

        var game2Data = data.ThanhTuus[1];
        _tmpGame2Info.text = $"Cân (Nhưng khó vl) tỉ lệ thắng : <color=green>{game2Data.totalWin}</color>/<color=white>{(game2Data.totalWin/game2Data.totalMatch)*100}</color> (<color=yellow>x</color>%)";

        var game3Data = data.ThanhTuus[2];
        _tmpGame3Info.text = $"1v1 tỉ lệ thắng : <color=green>{game3Data.totalWin}</color>/<color=white>{(game3Data.totalWin/game3Data.totalMatch)*100}</color> (<color=yellow>x</color>%)";
    }
}
