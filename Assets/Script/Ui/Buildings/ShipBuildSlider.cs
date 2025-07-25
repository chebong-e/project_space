using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShipBuildSlider : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI building_Amount;
    public TextMeshProUGUI building_Time;
    Base_Infomation info;


    public void Init()
    {
        slider = transform.GetChild(0).GetComponentInChildren<Slider>();
        building_Amount = transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
        building_Time = transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>();
        info = GetComponentInParent<Base_Infomation>();

        building_Amount.text = $"<       {slider.value}     /     {info.ship.maxHaveShip_Amount - info.ship.currentHave_Ship}       >";
        if (info.buildResource.level > 0)
        {
            slider.maxValue = info.ship.maxHaveShip_Amount - info.ship.currentHave_Ship;
        }
        else
        {
            slider.maxValue = 0;
        }
        
    }

    public void OnSliderValueChanged()
    {
        if (!info)
            return;

        int amount = info.ship.maxHaveShip_Amount - info.ship.currentHave_Ship;
        building_Amount.text = $"<       {slider.value}     /     {amount}       >";

        float time1 = slider.value * PlayerAbilityInfo.GetCalculatedTime("Ship", info.ship.shipMaking_Time);
        int time = Mathf.CeilToInt(time1);

        if (time >= 3600)
            building_Time.text = $"{time / 3600}시간 {(time % 3600) / 60}분 {time % 60}초";
        else if (time >= 60)
            building_Time.text = $"{time / 60}분 {time % 60}초";
        else
            building_Time.text = $"{time}초";

        for (int i = 0; i < info.ship.shipMake_Cost.Length; i++)
        {
            info.resources[i].text = $"{slider.value * info.ship.shipMake_Cost[i]}";
        }
    }


    // 관제센터 업그레이드시 함선생산 탭의 해당 함선에 대한 업그레이드 정보 표시
    /*public void ControlCenterUpgrade_for_Ship(int amount) // amount값은 allowble변수로 넘기기
    {
        slider.maxValue = amount;
        info.ships.maxHaveShip_Amount = amount;
        building_Amount.text = $"<       {slider.value}     /     {amount}       >";
        if (this.gameObject.activeInHierarchy)
            UpgradeInfomation();

    }*/

    
    ///////////////////////////////////////////
    public void Confirm()
    {
        
    }





}
