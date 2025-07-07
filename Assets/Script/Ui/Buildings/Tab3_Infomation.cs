using TMPro;
using UnityEngine;

public class Tab3_Infomation : Base_Infomation
{
    protected override void SelectedTabs()
    {
        tabs = Tabs.Tab3;
    }

    protected override void Init_CostInfo()
    {
        var (metal, cristal, gas) = (research.research_Cost[0], research.research_Cost[1], research.research_Cost[2]);

        for (int i = 0; i < research.level; i++)
        {
            metal = Mathf.FloorToInt(metal * research.upgrade_Cost_Require[i]);
            cristal = Mathf.FloorToInt(cristal * research.upgrade_Cost_Require[i]);
            gas = Mathf.FloorToInt(gas * research.upgrade_Cost_Require[i]);
        }

        production[2].text = $"{research.ability_Text}";
        production[0].text = $"{research.research_Ability * research.level}%";

        /*if (research.level > 0)
        {
            production[0].text =
                research.research_Ability != 0 ? $"{research.research_Ability * research.level} (+{research.research_Ability}%)" : "";
        }
        else
        {
            production[0].text = $"0 (+{research.research_Ability * research.level + 1}%)";
        }*/


        foreach (var tt in timeText)
        {
            tt.text = TimerTexting(research.research_Time[research.level]);
        }

        title_Text["name"].text = $"Lv.{research.level} {research.name.Split('.')[1]}";

        resources[0].text = $"{metal}";
        resources[1].text = $"{cristal}";
        resources[2].text = $"{gas}";


    }
}
