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
        slider = transform.GetChild(0).GetComponentInChildren<Slider>();
        building_Amount = transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
        building_Time = transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>();
        info = GetComponentInParent<Con_Infomation>();
        /*info.shipBuildSlider = this;
        Init();*/
    }

    public void Init()
    {
        building_Amount.text = $"<       {slider.value}     /     {info.ship.maxHaveShip_Amount - info.ship.currentHave_Ship}       >";
        slider.maxValue = info.ship.maxHaveShip_Amount;
    }

    public void OnSliderValueChanged()
    {
        //building_Amount.text = $"<       {slider.value}     /     {ships.maxShip_Amount - ships.have_CurrentShip}       >";

        UpgradeInfomation();

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

    void UpgradeInfomation()
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

        /*string timeStr = "";
        if (time >= 3600)
        {
            int hours = time / 3600;
            int minutes = (time % 3600) / 60;
            int seconds = time % 60;
            timeStr = string.Format("{0}시간 {1}분 {2}초", hours, minutes, seconds);
        }
        else if (time >= 60)
        {
            int minutes = time / 60;
            int seconds = time % 60;
            timeStr = string.Format("{0}분 {1}초", minutes, seconds);
        }
        else
        {
            timeStr = string.Format("{0}초", time);
        }
        building_Time.text = $"{timeStr}";*/





        for (int i = 0; i < info.ship.shipMake_Cost.Length; i++)
        {
            info.resources[i].text = $"{slider.value * info.ship.shipMake_Cost[i]}";
        }
        

    }







    ///////////////////////////////////////////
    public void Confirm()
    {
        
    }





}
