using UnityEngine;

public class Tab5_Infomation : Base_Infomation
{

    protected override void SelectedTabs()
    {
        tabs = Tabs.Tab5;
    }

    protected override void Init_CostInfo()
    {
        var (metal, cristal, gas) = (buildResource.init_Needs[0], buildResource.init_Needs[1], buildResource.init_Needs[2]);

        for (int i = 0; i < buildResource.level; i++)
        {
            metal = Mathf.FloorToInt(metal * buildResource.build_require[i]);
            cristal = Mathf.FloorToInt(cristal * buildResource.build_require[i]);
            gas = Mathf.FloorToInt(gas * buildResource.build_require[i]);
        }

        buildResource.cur_Needs[0] = metal;
        buildResource.cur_Needs[1] = cristal;
        buildResource.cur_Needs[2] = gas;

        resources[3].text = $"{buildResource.AllowableBuild} (+{Supplement_Allowable()})";
        resources[4].text = $"생산 가능 {buildResource.name}";

        int timer = Mathf.CeilToInt(Build_Manager.instance.TimerCalculation(tabs, buildResource.building_Time[buildResource.level]));
        foreach (var tt in timeText)
        {
            tt.text = $"{TimerTexting(timer)}";
        }

        title_Text["name"].text = $"Lv.{buildResource.level} {buildResource.name.Split('.')[1]} 관제센터";

        resources[0].text = $"{metal}";
        resources[1].text = $"{cristal}";
        resources[2].text = $"{gas}";
    }

    
}
