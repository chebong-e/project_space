using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static PlanetInfomation;

public class GalaxyInitSet : MonoBehaviour
{
    public GalaxyManager galaxyManager;
    WaitForFixedUpdate waitForFixedUpdate;
    ScrollRect scrollRect;

    private void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();
        waitForFixedUpdate = new WaitForFixedUpdate();
    }

    public void Galaxy_InitSetting(int count, int num)
    {
        galaxyManager = GalaxyManager.instance;

        for (int i = 0; i < count; i++)
        {
            if (i == 0)
            {
                GameObject coordinateObj = Instantiate(galaxyManager.planet_Prefabs[0], scrollRect.content);
                coordinateObj.GetComponent<PlanetInfomation>().infomation_Tabs[0].planet_coordinate.text
                    = $"{1}:1:{num + 1}";
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
            StartCoroutine(NumberingSetting(num + 1, p, SetOff));
        }

    }

    void SetOff()
    {
        gameObject.SetActive(false);
    }

    IEnumerator NumberingSetting(int coordi, PlanetInfomation plan, Action OnFinished)
    {
        yield return waitForFixedUpdate;
        int num = 0;

        Transform parent = plan.transform.parent;


        if (plan.planetType == PlanetType.AlienColony)
        {
            plan.transform.SetSiblingIndex(12);
        }
        yield return waitForFixedUpdate;
        if (plan.planetType == PlanetType.Resource_Planet)
        {
            SetAbsoluteSiblingIndex(plan.transform, 10);
        }

        yield return waitForFixedUpdate;

        for (int i = 0; i < parent.childCount; i++)
        {
            if (plan.gameObject == parent.GetChild(i).gameObject)
            {
                num = i + 1;
                break;
            }
        }

        string myName = plan.gameObject.name.Split(".")[1];
        plan.gameObject.name = $"{num}.{myName}";
        string[] myCoordinate = plan.infomation_Tabs[0].planet_coordinate.text.Split(":");
        myCoordinate[0] = $"{1}";
        myCoordinate[1] = $"{coordi}";
        myCoordinate[2] = $"{num}";
        plan.infomation_Tabs[0].planet_coordinate.text = string.Join(":", myCoordinate);

        yield return waitForFixedUpdate;

        if (coordi != 1)
            OnFinished?.Invoke();
    }

    void SetAbsoluteSiblingIndex(Transform target, int absoluteIndex)
    {
        int currentIndex = target.GetSiblingIndex();

        // 현재 위치보다 뒤쪽으로 이동 시, 한 칸 앞으로 당겨지는 문제 보정
        if (currentIndex < absoluteIndex)
        {
            absoluteIndex--;
        }

        target.SetSiblingIndex(absoluteIndex);
    }
}
