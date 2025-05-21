using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;


[System.Serializable]
public class ImageSliderGroup
{
    public List<ImageSlide> buildSlide = new List<ImageSlide>();
}

public class EventManager : MonoBehaviour
{
    public static EventManager instance;
    public int eventTriggered;
    public DropDown dropDown;

    public GameObject[] contents;


    public List<ImageSliderGroup> imageSliderGroup;
    public ImageSlide[] buildSlides;

    ImageSlide curBuild;

    public float build_Timer1;
    public float build_Timer2;
    public float build_Timer3;
    public float total_Timer;

    bool open;

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
                imageSliderGroup[i].buildSlide.Add(contents[i].transform.GetChild(sec_i).GetComponent<ImageSlide>());
            }
        }
    }

    public void Add_Event()
    {
        eventTriggered++;

        dropDown.triggered_event = eventTriggered;
    }

    public void Slide_Open()
    {
        
        ImageSlide buildslide = EventSystem.current.currentSelectedGameObject.GetComponentInParent<ImageSlide>();

        if (curBuild == null || curBuild != buildslide)
        {
            curBuild = buildslide;
        }
        else
        {
            if (curBuild == buildslide)
            {
                if (buildslide.open)
                    buildslide.Sliding_Close();
                else
                    buildslide.Sliding_Open();
                return;
            }
        }

        foreach (ImageSlide img in imageSliderGroup[7].buildSlide)
        {
            if (img == buildslide)
            {
                continue;
            }
            else
            {
                if (img.open)
                    img.Sliding_Close();
            }
        }

        buildslide.Sliding_Open();

    }
    
}
