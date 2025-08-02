using TMPro;
using UnityEngine;

public class PlanetInfomation : MonoBehaviour
{
    public enum PlanetType { Central_Planet, Base_Planet, Empty_Planet, Colony, AlienColony, Resource_Planet }
    public enum ResourcePlanet { G1, G2, G3, G4, G5 }
    public int alienLv;
    string alienColonyLv;

    public PlanetType planetType;
    public ResourcePlanet resourcePlanet;
    public Base_Planet_TabInfomations[] infomation_Tabs;

    private void Awake()
    {
        infomation_Tabs = GetComponentsInChildren<Base_Planet_TabInfomations>();
        alienColonyLv = $"외계인 식민지Lv.{alienLv}";

        foreach (Base_Planet_TabInfomations baseInfo in infomation_Tabs)
        {
            baseInfo.Init_Set();
        }
    }

    public void PlanetEventExcution(Event_Triggered event_Triggered)
    {

    }


    public void StarGradeSelect(int grade)
    {
        infomation_Tabs[1].StarGradeSelect(grade);
    }
}
