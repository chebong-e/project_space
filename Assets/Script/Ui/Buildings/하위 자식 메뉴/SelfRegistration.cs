using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelfRegistration : MonoBehaviour
{
    public enum TitleType { Title, Resource, TimeSlide }
    public enum SubType0 { Name, Time }
    public enum SubType1 { Metal, Cristal, Gas }
    public TitleType titleType;
    public SubType0 subType0;
    public SubType1 subType1;
    void Start()
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
            else
            {
                info.slider = GetComponent<Slider>();
                info.timeText = transform.GetChild(3).GetComponent<TextMeshProUGUI>();
            }
            
        }
    }
}
