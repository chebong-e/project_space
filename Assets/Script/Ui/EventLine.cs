using System;
using TMPro;
using UnityEngine;

public class EventLine : MonoBehaviour
{
    public Event_Triggered event_Triggered;
    public TextMeshProUGUI targetText;
    public TextMeshProUGUI timerText;
    float timer;

    void Start()
    {
        targetText = transform.GetChild(1).GetChild(1).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        timerText = transform.GetChild(1).GetChild(1).GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
        timer = 500;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        TimeSpan timespan = TimeSpan.FromSeconds(timer);
        timerText.text = timespan.ToString(@"hh\:mm\:ss");
    }
}
