using UnityEngine;

public class DownInfomation_Anim : MonoBehaviour
{
    Con_Infomation con_Infomation;

    void Awake()
    {
        con_Infomation = GetComponentInParent<Con_Infomation>();
    }

    public void Slider_On_Off()
    {
        if (con_Infomation.controlCenter_confirm)
        {
            con_Infomation.timeSlider[0].gameObject.SetActive(true);
        }

    }
}
