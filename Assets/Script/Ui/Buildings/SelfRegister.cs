using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelfRegister : MonoBehaviour
{
    public enum TitleType { Title, Resource, TimeSlide, Button, BuildShips, HaveShipCount, Production }
    public enum SubType0 { Name, UpTime, DownTime }
    public enum SubType1 { Metal, Cristal, Gas, Allowable }
    public enum SubType2 { UpSlider, DownSlider }
    public enum SubType3 { Confirm, Cancle }
    public enum SubType4 { Slider, Build_Amount, Build_Time, container }
    public enum SubType6 { Manufacture, Electricity_Consumption }

    public TitleType titleType;
    public SubType0 subType0;
    public SubType1 subType1;
    public SubType2 subType2;
    public SubType3 subType3;
    public SubType4 subType4;
    public SubType6 subType6;
    bool upgrading;

    public void Init_Setting()
    {
        Base_Infomation info = GetComponentInParent<Base_Infomation>();

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
                if (subType2 == SubType2.UpSlider)
                {
                    info.timeText[0] = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
                }
                else
                {
                    info.timeText[1] = transform.GetChild(3).GetComponent<TextMeshProUGUI>();
                    info.making_Targets_Text = transform.parent.GetChild(1).GetComponent<TextMeshProUGUI>();
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
            case TitleType.BuildShips:
                if (subType4 == SubType4.container)
                {
                    info.child_InfoContainer = this.gameObject;
                }

                break;
            case TitleType.HaveShipCount:
                info.haveShipCount = gameObject.GetComponent<TextMeshProUGUI>();
                GetComponent<TextMeshProUGUI>().text = $"보유 함선 수 : {info.ship.currentHave_Ship}";
                break;
            case TitleType.Production:
                if (subType6 == SubType6.Manufacture)
                {
                    info.production[0] = GetComponent<TextMeshProUGUI>();
                    // 2번은 설명 텍스트
                    info.production[2] = transform.parent.parent.GetChild(0).GetComponent<TextMeshProUGUI>();
                }
                else
                {
                    info.production[1] = GetComponent<TextMeshProUGUI>();
                }
                break;
        }
    }


}
