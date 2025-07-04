using TMPro;
using UnityEngine;

public class Tab2_Infomation : Base_Infomation
{
    protected override void SelectedTabs()
    {
        tabs = Tabs.Tab2;
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

        production[0].transform.parent.parent.GetChild(0).GetComponent<TextMeshProUGUI>().text =
            $"{buildResource.ability_Text}";
        if (production[1] == null)
        {
            production[0].text = "";
        }
        else
        {
            production[0].text = $"{buildResource.buildAbility * buildResource.level}%";
            buildResource.electricity_Consumption = 10;
            production[1].text = $"{buildResource.electricity_Consumption}";
        }

        

        foreach (var tt in timeText)
        {
            tt.text = $"{TimerTexting(buildResource.building_Time[buildResource.level])}";
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
