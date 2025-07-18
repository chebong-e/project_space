using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventLine : MonoBehaviour
{
    public Event_Triggered event_Triggered;
    public TextMeshProUGUI targetText;
    public TextMeshProUGUI timerText;
    public Image missionImg;
    public float timer;

    void Start()
    {
        timer = 500;
    }

    /*void Update()
    {

        if (timer > 0)
        {
            timer -= Time.deltaTime;
            TimeSpan timespan = TimeSpan.FromSeconds(timer);
            timerText.text = timespan.ToString(@"hh\:mm\:ss");
        }
        else
        {

        }
        
        
    }*/

    public void ActionStart(float time)
    {
        timer = time;   
        StartCoroutine(ActionTimerCoroutine());
    }

    IEnumerator ActionTimerCoroutine()
    {

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            TimeSpan timespan = TimeSpan.FromSeconds(timer);
            timerText.text = timespan.ToString(@"hh\:mm\:ss");
            yield return null;
        }

        timerText.text = $"-";
    }
}
