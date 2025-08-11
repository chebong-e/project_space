using UnityEngine;
using UnityEngine.UI;

public class GalaxyInitSet : MonoBehaviour
{
    public GalaxyManager galaxyManager;

    ScrollRect scrollRect;

    private void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();
    }

    /*private void Start()
    {
        Galaxy_InitSetting(13);
    }*/

    public void Galaxy_InitSetting(int count, int num)
    {
        galaxyManager = GalaxyManager.instance;

        for (int i = 0; i < count; i++)
        {
            if (i == 0)
            {
                GameObject coordinateObj = Instantiate(galaxyManager.planet_Prefabs[0], scrollRect.content);
                coordinateObj.GetComponent<PlanetInfomation>().infomation_Tabs[0].planet_coordinate.text
                    = $"{1}:{num + 1}:0";
                Instantiate(galaxyManager.planet_Prefabs[2], scrollRect.content);
                Instantiate(galaxyManager.planet_Prefabs[4], scrollRect.content);
                GameObject resObj = Instantiate(galaxyManager.planet_Prefabs[5], scrollRect.content);
                galaxyManager.res_Planets.Add(resObj);
                /*resObj.GetComponent<PlanetInfomation>().ResourcePlanet_GradeSet(1);*/

            }
            else
            {
                Instantiate(galaxyManager.planet_Prefabs[2], scrollRect.content);
            }
        }

        PlanetInfomation[] p_Info = scrollRect.content.GetComponentsInChildren<PlanetInfomation>();
        foreach (PlanetInfomation p in p_Info)
        {
            StartCoroutine(p.NumberingSetting(num + 1));
        }

    }
}
