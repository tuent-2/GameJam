using System;
using UnityEngine;

public class S_GM_Event : GM_Event
{
    public Action<EWeightScaleStatus> OnWeightScaleStatusChange;
    public Action<int> OnCurrentTryAmountChange;
}
