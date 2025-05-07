using UnityEngine;
using UnityEngine.EventSystems;

public class EventManager : MonoBehaviour
{
    public int eventTriggered;
    public DropDown dropDown;

    public GameObject content;
    public BuildSlide[] buildSlides;

    public float build_Timer1;
    public float build_Timer2;
    public float build_Timer3;
    public float total_Timer;

    bool open;

    void Start()
    {
        int num = content.transform.childCount;
        buildSlides = new BuildSlide[num];
        for (int i = 0; i < num; i++)
        {
            buildSlides[i] = content.transform.GetChild(i).GetComponent<BuildSlide>();
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


        /*if (open)
        {
            int num = 0;
            for (int i = 0; i < buildSlides.Length; i++)
            {
                if (buildslide == buildSlides[i])
                {
                    num = i;
                    return;
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
                    buildSlides[i].Sliding_Close();
                }
            }
            open = false;
            buildSlides[num].Sliding();
        }
        else
        {
            buildslide.Sliding();
            open = true;
        }*/
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
