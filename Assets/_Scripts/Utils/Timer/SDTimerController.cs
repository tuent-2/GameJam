using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//Dùng chạy ngược time...
public class SDTimerController : MonoSingleton<SDTimerController>
{
    public static long IncrementId;
    private float _timeOnPause = 0;

    private readonly List<SDTimer> _timers = new List<SDTimer>();

    public void AddTimer(SDTimer timer)
    {
        if (_timers.All(s => s.id != timer.id))
        {
            _timers.Add(timer);
            timer.timerCt = Executors.RunCoroutineWithReturn(ISDTimer(timer));
        }
        else
        {
            timer.timeLeft = timer.countTime;
            if (timer.timerCt != null)
            {
                Executors.StopIEnumerator(timer.timerCt);
            }

            timer.timerCt = Executors.RunCoroutineWithReturn(ISDTimer(timer));
        }
    }

    public void RemoveTimer(SDTimer t)
    {
        _timers.Remove(t);
    }

    private IEnumerator ISDTimer(SDTimer t, float stepCounter = 0.2f)
    {
        while (t.timeLeft > 0)
        {
            var previousTime = Time.realtimeSinceStartup;
            t.running = true;
            if (t.isRealtimeCounter)
            {
                yield return Helpers.GetWaitForSecondsRealTime(stepCounter);
            }
            else
            {
                yield return Helpers.GetWaitForSeconds(stepCounter);
            }

            t.timeLeft -= (Time.realtimeSinceStartup - previousTime);
            if (t.isRunEveryStep)
            {
                t.callBackRunEveryStep?.Invoke(t.timeLeft);
            }
        }

        t.running = false;
        t.completeCallback?.Invoke();
        Instance.RemoveTimer(t);
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            _timeOnPause = Time.realtimeSinceStartup;
        }
        else
        {
            var currentTime = Time.realtimeSinceStartup;
            var pauseTime = currentTime - _timeOnPause;
            foreach (var t in _timers)
            {
                if (!t.ignorePauseTime)
                {
                    t.timeLeft -= pauseTime;
                }
            }
        }
    }
}

public class SDTimer
{
    public long id;
    public bool ignorePauseTime;
    public bool running;
    public Coroutine timerCt;
    public float countTime;
    public float timeLeft;
    public Action completeCallback;
    public bool isRunEveryStep;
    public bool isRunEveryMinute;
    public Action<float> callBackRunEveryStep;
    public bool isRealtimeCounter;

    public SDTimer(float timeDelay, bool _ignorePauseTime = false, bool isRealtime = false)
    {
        id = SDTimerController.IncrementId++;
        countTime = timeDelay;
        timeLeft = timeDelay;
        running = false;
        ignorePauseTime = _ignorePauseTime;
        isRunEveryStep = false;
        isRunEveryMinute = false;
        isRealtimeCounter = isRealtime;
    }

    public SDTimer AddCompleteEvent(Action @event)
    {
        completeCallback += @event;
        return this;
    }

    public SDTimer RemoveCompleteEvent(Action @event)
    {
        completeCallback -= @event;
        return this;
    }

    public SDTimer AddCallbackForEveryStep(Action<float> @event)
    {
        //SDLogger.LogError("Clear callback everytime");
        isRunEveryStep = true;
        callBackRunEveryStep += @event;
        return this;
    }

    public SDTimer RemoveCallbackForEveryStep(Action<float> @event)
    {
        callBackRunEveryStep -= @event;
        return this;
    }

    public void ClearAllCallBackRunEveryStep()
    {
        completeCallback = null;
    }

    public void ClearAllCompleteEvents()
    {
        completeCallback = null;
    }

    public SDTimer StartTimer()
    {
        SDTimerController.Instance.AddTimer(this);
        return this;
    }

    public void StopTimer()
    {
        if (timerCt != null)
        {
            Executors.StopIEnumerator(timerCt);
        }

        running = false;
        SDTimerController.Instance.RemoveTimer(this);
    }

    public void ResetTimer()
    {
        timeLeft = countTime;
    }
}