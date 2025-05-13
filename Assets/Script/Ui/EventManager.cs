using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;


[System.Serializable]
public class BuildSliderGroup
{
    public List<BuildSlide> buildSlide = new List<BuildSlide>();
}

public class EventManager : MonoBehaviour
{
    public int eventTriggered;
    public DropDown dropDown;

    public GameObject content;
    public GameObject[] contents;


    public List<BuildSliderGroup> buildSliderGroup;
    public BuildSlide[] buildSlides;

    BuildSlide curBuild;

    public float build_Timer1;
    public float build_Timer2;
    public float build_Timer3;
    public float total_Timer;

    bool open;

    void Start()
    {
        /*int num = content.transform.childCount;
        buildSlides = new BuildSlide[num];
        for (int i = 0; i < num; i++)
        {
            buildSlides[i] = content.transform.GetChild(i).GetComponent<BuildSlide>();
        }*/
        buildSliderGroup = new List<BuildSliderGroup>();

        for (int i = 0; i < 3; i++)
        {
            BuildSliderGroup group = new BuildSliderGroup();
            buildSliderGroup.Add(group);
        }


        for (int i = 0; i < contents[0].transform.childCount; i++)
        {
            buildSliderGroup[0].buildSlide.Add(contents[0].transform.GetChild(i).GetComponent<BuildSlide>());
        }

        for (int i = 0; i < contents[1].transform.childCount; i++)
        {
            buildSliderGroup[1].buildSlide.Add(contents[1].transform.GetChild(i).GetComponent<BuildSlide>());
        }

        for (int i = 0; i < contents[2].transform.childCount; i++)
        {
            buildSliderGroup[2].buildSlide.Add(contents[2].transform.GetChild(i).GetComponent<BuildSlide>());
        }
    }

    public void Add_Event()
    {
        eventTriggered++;

        dropDown.triggered_event = eventTriggered;
    }

    public void Slide_Open()
    {
        
        BuildSlide buildslide = EventSystem.current.currentSelectedGameObject.GetComponentInParent<BuildSlide>();

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

        int num = 0;
        for (int i = 0; i < buildSlides.Length; i++)
        {
            if (buildslide == buildSlides[i])
            {
                num = i;
                Debug.Log(num);
                break;
            }
        }
        for (int i = 0; i < buildSlides.Length; i++)
        {
            if (i == num)
            {
                continue;
            }
            else
            {
                if (buildSlides[i].open)
                    buildSlides[i].Sliding_Close();
            }
        }

        buildslide.Sliding_Open();

    }
    
}
