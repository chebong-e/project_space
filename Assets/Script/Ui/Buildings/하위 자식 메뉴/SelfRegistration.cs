using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class SelfRegistration : MonoBehaviour
{
    public enum GrateTitle { MainTab, BuildTab1, BuildTab2, BuildTab3, ResearchTab, ControlCenterTab, FleetMision }
    public enum TitleType { Title, Resource, TimeSlide, Button, BuildShips }
    public enum SubType0 { Name, UpTime, DownTime }
    public enum SubType1 { Metal, Cristal, Gas, Allowable }
    public enum SubType2 { UpSlider, DownSlider }
    public enum SubType3 { Confirm, Cancle }
    public enum SubType4 { Slider, Build_Amount, Build_Time }
    public GrateTitle grateTitle;
    public TitleType titleType;
    public SubType0 subType0;
    public SubType1 subType1;
    public SubType2 subType2;
    public SubType3 subType3;
    public SubType4 subType4;
    bool upgrading;






    public void Init_Setting()
    {
        Con_Infomation info = GetComponentInParent<Con_Infomation>();

        if (info == null)
            return;

        switch (grateTitle)
        {
            case GrateTitle.BuildTab1:

                break;
            case GrateTitle.BuildTab2:

                break;
            case GrateTitle.BuildTab3:
                ExSwitch(titleType, info);
                /*switch (subType4)
                {
                    case SubType4.Slider:
                        info.amountSlider = GetComponent<Slider>();
                        break;

                    case SubType4.Build_Amount:
                        info.amount_Text[0] = GetComponent<TextMeshProUGUI>();
                        break;

                    default:
                        info.amount_Text[1] = GetComponent<TextMeshProUGUI>();
                        break;
                }*/
                break;
            case GrateTitle.ResearchTab:

                break;
            case GrateTitle.ControlCenterTab:
                ExSwitch(titleType, info);
                break;
            case GrateTitle.FleetMision:

                break;
        }
    }

    void ExSwitch(TitleType titleType, Con_Infomation info)
    {
        switch (titleType)
        {
            case TitleType.Title:
                TextMeshProUGUI titleText = GetComponent<TextMeshProUGUI>();
                if (subType0 == SubType0.Name)
                {
                    info.title_Text["name"] = titleText;
                }
                else if (subType0 == SubType0.UpTime)
                {
                    info.title_Text["up_upgradeTime"] = titleText;
                }
                else
                {
                    info.title_Text["down_upgradeTime"] = titleText;
                }
                break;

            case TitleType.Resource:
                info.resources[(int)subType1] = GetComponentInChildren<TextMeshProUGUI>();
                if (subType1 == SubType1.Allowable)
                {
                    info.resources[(int)subType1 + 1] = transform.parent.GetChild(0).GetComponent<TextMeshProUGUI>();
                }
                break;
            case TitleType.TimeSlide:
                info.timeSlider[(int)subType2] = GetComponent<Slider>();
                if (subType2 != SubType2.UpSlider)
                {
                    info.timeText[1] = transform.GetChild(3).GetComponent<TextMeshProUGUI>();
                }
                else
                {
                    info.timeText[0] = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
                }
                break;
            case TitleType.Button:
                Button btn = GetComponent<Button>();
                if (subType3 == SubType3.Confirm)
                {
                    info.btns[0] = btn;
                    info.btns[0].gameObject.SetActive(!upgrading);
                }
                else
                {
                    info.btns[1] = btn;
                    info.btns[1].gameObject.SetActive(upgrading);
                }
                break;

        }
    }
}
