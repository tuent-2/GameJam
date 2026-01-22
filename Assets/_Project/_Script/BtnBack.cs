using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PTT;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BtnBack : ExtendMonoBehaviour
{
    //===================================================================== Variables
    [SerializeField] private string _sSceneName;
    [SerializeField] private Button _btnBack;   
    //===================================================================== Unity Methods
    protected override void Awake()
    {
        base.Awake();
        _btnBack.onClick.AddListener(OnClickBtn);
    }
    //===================================================================== Action Handle

    private void OnClickBtn()
    {
        SceneManager.UnloadSceneAsync(_sSceneName);     
        SceneManager.LoadSceneAsync("LeaderBoardScene");
    }
    
    //===================================================================== Local Methods
}
