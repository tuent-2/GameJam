using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PTT;

public enum ETagGameForDetect
{
    Ball = 3,
    PrepareZone = 0,
    BalanceZone = 1,
    AnswerZone = 2,
}
public class TagGameForDetect : MonoBehaviour
{
    public ETagGameForDetect eTag;
}
