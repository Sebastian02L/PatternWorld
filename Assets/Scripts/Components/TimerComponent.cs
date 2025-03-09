using System;
using UnityEngine;

public class TimerComponent : MonoBehaviour
{
    [SerializeField] float timerMaxTime;
    public Action onTimerEnd;
    bool active = false;

    public void StartTimer()
    {
        active = true;
    }

    void Update()
    {
        if(!active) return;

        timerMaxTime -= Time.deltaTime;

        if(timerMaxTime <= 0)
        {
            onTimerEnd?.Invoke();
            active = false;
        }
    }
}
