using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using static SelfRegistration;


public class EventManager : MonoBehaviour
{
    public static EventManager instance;
    public int eventTriggered;
    public DropDown dropDown;

    public GameObject[] contents;

    public List<ImageSliderGroup> imageSliderGroup;
    ImageSlide curImgSlide;

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


    }

    public void Add_Event()
    {
        eventTriggered++;

        dropDown.triggered_event = eventTriggered;
    }


    // 버튼은 항상 하나만 열리게 하는 동작
    public void Slide_Open()
    {
        
        ImageSlide imgslide = EventSystem.current.currentSelectedGameObject.GetComponentInParent<ImageSlide>();

        if (curImgSlide == null || curImgSlide != imgslide)
        {
            curImgSlide = imgslide;
        }
        else
        {
            if (curImgSlide == imgslide)
            {
                if (imgslide.open)
                    imgslide.Sliding_Close();
                else
                    imgslide.Sliding_Open();
                return;
            }
        }

        foreach (ImageSlide img in imageSliderGroup[BuildManager.instance.AllWindow_Active_IndexCheck()].imageSlide)
        {
            if (img == imgslide)
            {
                continue;
            }
            else
            {
                if (img.open)
                    img.Sliding_Close();
            }
        }

        imgslide.Sliding_Open();

    }
}
