using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;
    public int eventTriggered;
    public DropDown dropDown;

    //È®ÀÎ¿ë
    public GameObject[] Ex_contentsCheck;
    public GameObject canvas;


    public GameObject[] TabContainer;


    public float build_Timer1;
    public float build_Timer2;
    public float build_Timer3;
    public float total_Timer;


    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

    }

    public void Add_Event()
    {
        eventTriggered++;

        dropDown.triggered_event = eventTriggered;
    }
}
