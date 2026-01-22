using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;
using PTT;

public class S_GM_ScoreUpdate : GMModuleBehaviour
{
    //===================================================================== Variables
    [SerializeField] private int _iGameID;
    private GM_Event _gmEvent;

    private S_GM_Data_Dynamic _gmDataDynamic;

    //===================================================================== Unity Methods
    private void Start()
    {
        _gmEvent = GM.Instant.GetModule<GM_Event>();
        _gmDataDynamic = GM.Instant.GetModule<S_GM_Data_Dynamic>();
        _gmEvent.OnGameStateChange += OnGameStateChange;
    }

    //===================================================================== Action Handle

    private void OnGameStateChange(EGameState from, EGameState to)
    {
        int currentLevel = _gmDataDynamic.dataCurrent.iCurrentLevel;
        switch (to)
        {
            case EGameState.None:
                break;
            case EGameState.Ready:
                break;
            case EGameState.Playing:
                break;
            case EGameState.EndLose:
                GameControllerModel.Instance.UpdateScore(_iGameID, -100 * currentLevel);
                break;
            case EGameState.EndWin:
                GameControllerModel.Instance.UpdateScore(_iGameID, +100 * currentLevel);
                break;
            case EGameState.Progressing:
                break;
            default:
                break;
        }
    }

    //===================================================================== Local Methods
}