using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelfRegistration : MonoBehaviour
{
    public enum TitleType { Title, Resource, TimeSlide, Button }
    public enum SubType0 { Name, Time }
    public enum SubType1 { Metal, Cristal, Gas, Allowable }
    public enum SubType2 { Confirm, Cancle }
    public TitleType titleType;
    public SubType0 subType0;
    public SubType1 subType1;
    public SubType2 subType2;
    bool upgrading;
    

    public void Init_Setting()
    {
        Infomations info = GetComponentInParent<Infomations>();

        if (info != null)
        {
            if (titleType == TitleType.Title)
            {
                if (subType0 == 0)
                    info.titles["name"] = GetComponent<TextMeshProUGUI>();
                else
                    info.titles["upgradeTime"] = GetComponent<TextMeshProUGUI>();
            }
            else if (titleType == TitleType.Resource)
            {
                info.resources[(int)subType1] = GetComponentInChildren<TextMeshProUGUI>();
            }
            else if (titleType == TitleType.TimeSlide)
            {
                info.slider = GetComponent<Slider>();
                info.timeText = transform.GetChild(3).GetComponent<TextMeshProUGUI>();
            }
            else
            {
                if (subType2 == 0)
                {
                    info.btns[0] = GetComponent<Button>();
                    info.btns[0].gameObject.SetActive(!upgrading);
                }
                else
                {
                    info.btns[1] = GetComponent<Button>();
                    info.btns[1].gameObject.SetActive(upgrading);
                }

                
            }
            
            
        }
    }
}
