using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Planet_Tab_Info : Base_Planet_TabInfomations
{
    TextMeshProUGUI text;
    Image[] child_icon;
    PlanetInfomation planetInfomation;




    protected override void VariableMatching()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        child_icon = GetComponentsInChildren<Image>(true);
        planetInfomation = GetComponentInParent<PlanetInfomation>();

        switch (tabs)
        {
            case Tabs.Tab1:
                planet_coordinate = text;
                break;
            case Tabs.Tab2:
            case Tabs.Tab3:
            case Tabs.Tab5:
                icons = child_icon;
                planetName = text;
                break;
            case Tabs.Tab4:
                for (int i = 0; i < transform.childCount; i++)
                {
                    debris[i] = transform.GetChild(i).gameObject;
                    debrisInResource[i]
                        = transform.GetChild(1).GetChild(i).GetComponent<TextMeshProUGUI>();
                }
                break;
        }
    }

    
}
