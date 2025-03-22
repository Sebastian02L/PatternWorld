using System;
using TMPro;
using UnityEngine;

public class TimerComponent : MonoBehaviour
{
    [SerializeField] float timerMaxTime;
    [SerializeField] TextMeshProUGUI timerText;
    public Action onTimerEnd;
    bool active = false;

    public void StartTimer()
    {
        active = true;
    }

    public void PauseTimer()
    {
        active = false;
    }

    public void SetStartTime(float time)
    {
        timerMaxTime = time;
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

        if (timerText != null) 
        { 
            timerText.text = ((int)timerMaxTime).ToString();
        }
    }
}
