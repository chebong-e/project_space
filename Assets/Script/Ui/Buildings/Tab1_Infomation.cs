using UnityEngine;

public class Tab1_Infomation : Base_Infomation
{
    public override void SelectedTabs()
    {
        tabs = Tabs.Tab1;
    }

    public override void Init_CostInfo()
    {
        var (metal, cristal, gas) = (buildResource.init_Needs[0], buildResource.init_Needs[1], buildResource.init_Needs[2]);

        for (int i = 0; i < buildResource.level; i++)
        {
            metal = Mathf.FloorToInt(metal * buildResource.build_require[i]);
            cristal = Mathf.FloorToInt(cristal * buildResource.build_require[i]);
            gas = Mathf.FloorToInt(gas * buildResource.build_require[i]);
        }

        production[0].text = $"{buildResource.manufacture}";
        production[1].text = $"{buildResource.electricity_Consumption}";

        foreach (var tt in timeText)
        {
            tt.text = $"{TimerTexting(buildResource.building_Time[buildResource.level])}";
        }

        title_Text["name"].text = $"Lv.{buildResource.level} {buildResource.name}";

        resources[0].text = $"{metal}";
        resources[1].text = $"{cristal}";
        resources[2].text = $"{gas}";
    }
}
