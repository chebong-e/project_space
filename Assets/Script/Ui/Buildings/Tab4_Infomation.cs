using UnityEngine;

public class Tab4_Infomation : Base_Infomation
{
    protected override void SelectedTabs()
    {
        tabs = Tabs.Tab4;
    }

    protected override void Init_ContainerSlide()
    {
        shipBuildSlider = GetComponentInChildren<ShipBuildSlider>();
        shipBuildSlider.Init();
        base.Init_ContainerSlide();
        
    }

    protected override void Init_CostInfo()
    {
        var (metal, cristal, gas) = (ship.shipMake_Cost[0], ship.shipMake_Cost[1], ship.shipMake_Cost[2]);

        Debug.Log(timeText[0]);
        timeText[0].text = $"{TimerTexting((int)ship.shipMaking_Time)}";
        title_Text["name"].text = $"{ship.name}";
        child_InfoContainer.transform.GetChild(0).gameObject.SetActive(false);

        resources[0].text = $"{metal}";
        resources[1].text = $"{cristal}";
        resources[2].text = $"{gas}";
    }
}
