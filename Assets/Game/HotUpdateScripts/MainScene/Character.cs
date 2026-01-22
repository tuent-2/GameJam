using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PTT;
using Sirenix.OdinInspector;
using Spine.Unity;
using TMPro;

public class Character : ExtendMonoBehaviour
{
    //===================================================================== Variables
    [SerializeField] private GameObject[] _arrSkin;
    [SerializeField] private TextMeshProUGUI _tmpName;
    [SerializeField] private TextMeshProUGUI _tmpDes;
    [SerializeField,DisableIn(PrefabKind.All)]
    private LeaderBoardSO.LeaderBoard  _data;
    public LeaderBoardSO.LeaderBoard data => _data;
    //===================================================================== Unity Methods

    //===================================================================== Action Handle

    //===================================================================== Local Methods

    public void UpdateVisualByData(LeaderBoardSO.LeaderBoard  value)
    {
        DebugLog("real");
        _data = value;
        _tmpName.text = data.name;
        foreach (var ele in _arrSkin)
        {
            ele.SetActive(false);
        }
        
        _arrSkin[data.camelId].SetActive(true);
        _tmpDes.text = data.description;
    }
    
    public void FakeVisual()
    {
        DebugLog("fake");
        _tmpName.text = "";
        foreach (var ele in _arrSkin)
        {
            ele.SetActive(false);
        }
        
        int randomIndex = Random.Range(0, _arrSkin.Length);
        _arrSkin[randomIndex].SetActive(true);
        _tmpDes.text = "";
    }
}
