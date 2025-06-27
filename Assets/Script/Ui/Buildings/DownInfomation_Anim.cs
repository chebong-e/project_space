using UnityEngine;

public class DownInfomation_Anim : MonoBehaviour
{
    Base_Infomation Infomation;

    void Awake()
    {
        Infomation = GetComponentInParent<Base_Infomation>();
    }

    public void Slider_On_Off()
    {
        if (Infomation.controlCenter_confirm)
        {
            Infomation.timeSlider[0].gameObject.SetActive(true);
        }

    }
}
