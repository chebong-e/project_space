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

    //확인용
    public GameObject[] Ex_contentsCheck;



    public GameObject[] contents;
    public GameObject[] TabContainer;

    public List<ImageSliderGroup> imageSliderGroup;

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

        imageSliderGroup = new List<ImageSliderGroup>();

        for (int i = 0; i < contents.Length; i++)
        {
            ImageSliderGroup group = new ImageSliderGroup();
            imageSliderGroup.Add(group);

            for (int sec_i = 0; sec_i < contents[i].transform.childCount; sec_i++)
            {
                imageSliderGroup[i].imageSlide.Add(contents[i].transform.GetChild(sec_i).GetComponent<ImageSlide>());
            }
        }


        /*// 확인용
        for (int i = 0; i < TabContainer.Length; i++)
        {
            scrRectGroup group = new scrRectGroup();
            group.scrolls = TabContainer[i].GetComponentsInChildren<ScrollRect>(true);
            
            containers[TabContainer[i]] = group;


        }*/
    }

    void Start()
    {
        IndexCheck();
    }

    public void Add_Event()
    {
        eventTriggered++;

        dropDown.triggered_event = eventTriggered;
    }

    void IndexCheck()
    {
        int num = 0;
        for (int i = 0; i < TabContainer.Length; i++)
        {
            num += TabContainer[i].transform.GetComponentsInChildren<ScrollRect>(true).Length;
        }

        
        Ex_contentsCheck = new GameObject[num];

        for (int i = 0; i < TabContainer.Length; i++)
        {
            if (TabContainer[i].transform.GetComponentsInChildren<ScrollRect>(true).Length > 1)
            {
                GameObject[] objs = TabContainer[i].transform.GetComponentsInChildren<ScrollRect>(true);
            }
            for (int ii = 0; ii < TabContainer[i].transform.GetComponentsInChildren<ScrollRect>(true).Length; ii++)
            {
                Ex_contentsCheck[ii] = TabContainer[i].transform.GetComponentInChildren<ScrollRect>().gameObject;
            }
        }
        Debug.Log(num);
    }
}
