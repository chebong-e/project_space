using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShipBuildSlider : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI building_Amount;
    public TextMeshProUGUI building_Time;
    Con_Infomation info;


    void Awake()
    {
        /*slider = transform.GetChild(0).GetComponentInChildren<Slider>();
        building_Amount = transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
        building_Time = transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>();
        info = GetComponentInParent<Con_Infomation>();*/
        /*info.shipBuildSlider = this;
        Init();*/
    }

    public void Init()
    {
        slider = transform.GetChild(0).GetComponentInChildren<Slider>();
        building_Amount = transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
        building_Time = transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>();
        info = GetComponentInParent<Con_Infomation>();

        building_Amount.text = $"<       {slider.value}     /     {info.ship.maxHaveShip_Amount - info.ship.currentHave_Ship}       >";
        slider.maxValue = info.ship.maxHaveShip_Amount - info.ship.currentHave_Ship;
    }

    public void OnSliderValueChanged()
    {
        int amount = info.ship.maxHaveShip_Amount - info.ship.currentHave_Ship;
        building_Amount.text = $"<       {slider.value}     /     {amount}       >";


        int time = (int)slider.value * (int)info.ship.shipMaking_Time;
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
