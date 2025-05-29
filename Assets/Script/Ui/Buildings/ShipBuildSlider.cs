using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShipBuildSlider : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI building_Amount;
    public TextMeshProUGUI building_Time;
    public Ships ships;
    Infomations info;


    void Awake()
    {
        slider = transform.GetChild(0).GetComponentInChildren<Slider>();
        building_Amount = transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
        building_Time = transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>();
        info = GetComponentInParent<Infomations>();
    }

    public void OnSliderValueChanged()
    {
        //building_Amount.text = $"<       {slider.value}     /     {ships.maxShip_Amount - ships.have_CurrentShip}       >";

        UpgradeInfomation();

    }


    // �������� ���׷��̵�� �Լ����� ���� �ش� �Լ��� ���� ���׷��̵� ���� ǥ��
    public void ControlCenterUpgrade_for_Ship(int amount) // amount���� allowble������ �ѱ��
    {
        slider.maxValue = amount;
        ships.maxShip_Amount = amount;
        building_Amount.text = $"<       {slider.value}     /     {amount}       >";
        if (this.gameObject.activeInHierarchy)
            UpgradeInfomation();

    }

    void UpgradeInfomation()
    {
        int amount = ships.maxShip_Amount - ships.have_CurrentShip;
        building_Amount.text = $"<       {slider.value}     /     {amount}       >";

        string timeStr = "";
        int time = (int)slider.value * (int)ships.shipBuild_Time;
        if (time >= 3600)
        {
            int hours = time / 3600;
            int minutes = (time % 3600) / 60;
            int seconds = time % 60;
            timeStr = string.Format("{0}�ð� {1}�� {2}��", hours, minutes, seconds);
        }
        else if (time >= 60)
        {
            int minutes = time / 60;
            int seconds = time % 60;
            timeStr = string.Format("{0}�� {1}��", minutes, seconds);
        }
        else
        {
            timeStr = string.Format("{0}��", time);
        }



        building_Time.text = $"{timeStr}";
        for (int i = 0; i < info.buildResource.init_Needs.Length; i++)
        {
            info.resources[i].text = $"{slider.value * info.buildResource.init_Needs[i]}";
        }
        

    }
}
