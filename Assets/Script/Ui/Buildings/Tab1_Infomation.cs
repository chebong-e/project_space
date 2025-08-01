using UnityEngine;

public class Tab1_Infomation : Base_Infomation
{
    protected override void SelectedTabs()
    {
        tabs = Tabs.Tab1;
    }

    protected override void Init_CostInfo()
    {
        var (metal, cristal, gas) = (buildResource.init_Needs[0], buildResource.init_Needs[1], buildResource.init_Needs[2]);

        buildResource.manufacture[1] = buildResource.manufacture[0];
        for (int i = 0; i < buildResource.level; i++)
        {
            metal = Mathf.FloorToInt(metal * buildResource.build_require[i]);
            cristal = Mathf.FloorToInt(cristal * buildResource.build_require[i]);
            gas = Mathf.FloorToInt(gas * buildResource.build_require[i]);
            buildResource.manufacture[1] = (int)(buildResource.manufacture[1] * buildResource.magnification);
        }

        buildResource.cur_Needs[0] = metal;
        buildResource.cur_Needs[1] = cristal;
        buildResource.cur_Needs[2] = gas;

        //자원생산량 및 전력소모량 적용
        switch (buildResource.resource_Factory)
        {
            case BuildResource.Resource_Factory.Metal:
            case BuildResource.Resource_Factory.Cristal:
            case BuildResource.Resource_Factory.Gas:
                ResourceManager.instance.build_Productions[(int)buildResource.resource_Factory]
                        = Manufacture_Conversion(buildResource);
                break;
        }
        //

        /*buildResource.electricity_Consumption = buildResource.manufacture[buildResource.level] / 10;*/
        buildResource.electricity_Consumption = 30;
        production[0].text = $"{Manufacture_Conversion(buildResource)}";
        production[1].text = $"{buildResource.electricity_Consumption}";


        int timer = Mathf.CeilToInt(PlayerAbilityInfo.GetCalculatedTime("Build", buildResource.building_Time[buildResource.level]));
        foreach (var tt in timeText)
        {
            //tt.text = $"{TimerTexting(buildResource.building_Time[buildResource.level])}";
            tt.text = $"{TimerTexting(timer)}";
        }

        title_Text["name"].text = $"Lv.{buildResource.level} {buildResource.name.Split('.')[1]}";

        resources[0].text = $"{metal}";
        resources[1].text = $"{cristal}";
        resources[2].text = $"{gas}";

        /*buildResource.electricity_Consumption = buildResource.manufacture[buildResource.level] / 10;*/

        /*production[0].text = $"{buildResource.manufacture[buildResource.level]}";
        production[1].text = $"{buildResource.electricity_Consumption}";*/
    }




}
