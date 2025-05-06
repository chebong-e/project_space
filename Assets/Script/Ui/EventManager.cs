using UnityEngine;
using UnityEngine.EventSystems;

public class EventManager : MonoBehaviour
{
    public int eventTriggered;
    public DropDown dropDown;

    public float build_Timer1;
    public float build_Timer2;
    public float build_Timer3;
    public float total_Timer;

    bool open;

    public void Add_Event()
    {
        eventTriggered++;

        dropDown.triggered_event = eventTriggered;
    }

    public void Slide_Open()
    {
        
        BuildSlide buildslide = EventSystem.current.currentSelectedGameObject.GetComponentInParent<BuildSlide>();
        if (buildslide.anims[0] != null)
        {
            if(open == false)
            {
                foreach (Animator anim in buildslide.anims)
                {
                    anim.SetTrigger("Open");
                }
                open = true;
            }
            else
            {
                foreach (Animator anim in buildslide.anims)
                {
                    anim.SetTrigger("Close");
                }
                open = false;
            }
            
        }


    }
    
}
