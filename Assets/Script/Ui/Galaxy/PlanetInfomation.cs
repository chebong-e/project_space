using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetInfomation : MonoBehaviour
{
    public enum PlanetType { Central_Planet, Base_Planet, Empty_Planet, Colony, AlienColony, Resource_Planet }
    public enum ResourcePlanet { G1, G2, G3, G4, G5 }
    public int alienLv;
    string alienColonyLv;

    public PlanetType planetType;
    public ResourcePlanet resourcePlanet;
    public List<Planet_Resources> haveResource;
    public Base_Planet_TabInfomations[] infomation_Tabs;

    int[,] resource_Value;
    int[,] cyon_Value;

    public int totalMiningRate;  // 총 채굴속도
    public float miningPerSecond;
    public bool isPossess; // 플레이어가 보유중인가 여부 확인

    private void Awake()
    {
        infomation_Tabs = GetComponentsInChildren<Base_Planet_TabInfomations>();
        alienColonyLv = $"외계인 식민지Lv.{alienLv}";

        // 자원행성의 자원 랜덤 배정
        resource_Value = new int[,]
        {
            { 100_000, 100_000, 100_000 },
            { 150_000, 75_000, 75_000 },
            { 75_000, 150_000, 75_000 },
            { 75_000, 75_000, 150_000 }
        };
        cyon_Value = new int[,]
        {
            { 1500, 1500, 1500 },
            { 2500, 1000, 1000 },
            { 1000, 2500, 1000 },
            { 1000, 1000, 2500 }
        };


        foreach (Base_Planet_TabInfomations baseInfo in infomation_Tabs)
        {
            baseInfo.Init_Set();
        }

        // 보유중인 플레이어가 없다면 플레이어 이름을 표시할 필요가 없음으로
        if (!isPossess)
        {
            infomation_Tabs[4].transform.GetChild(0).gameObject.SetActive(false);
        }

        StartCoroutine(NumberingSetting());
        
    }

    IEnumerator NumberingSetting()
    {
        int num = 0;

        Transform parent = transform.parent;

        if (planetType == PlanetType.Resource_Planet)
        {
            transform.SetSiblingIndex(9);
        }
        yield return new WaitForFixedUpdate();

        if (planetType == PlanetType.AlienColony)
        {
            transform.SetSiblingIndex(11);
        }
        yield return new WaitForFixedUpdate();

        for (int i = 0; i < parent.childCount; i++)
        {
            if (gameObject == parent.GetChild(i).gameObject)
            {
                num = i + 1;
                break;
            }
        }



        string myName = gameObject.name.Split(".")[1];
        gameObject.name = $"{num}.{myName}";
        infomation_Tabs[0].planet_coordinate.text = $"3:119:{num}";

    }



    public void PlanetEventExcution(Event_Triggered event_Triggered)
    {

    }

    // 자원행성의 그레이드 등급 설정 로직
    // 초기화마다 랜덤하게 설정되도록 함.
    public void ResourcePlanet_GradeSet()
    {
        int resIndex = Random.Range(0, 4);
        int cyIndex = Random.Range(0, 4);
        Debug.Log($"자원: {resIndex}, 시온: {cyIndex}");


        int metal = resource_Value[resIndex, 0];
        int cristal = resource_Value[resIndex, 1];
        int gas = resource_Value[resIndex, 2];
        int cyon_Red = cyon_Value[cyIndex, 0];
        int cyon_Green = cyon_Value[cyIndex, 1];
        int cyon_Black = cyon_Value[cyIndex, 2];

        haveResource.Clear();
        int grade = Random.Range(0, 5);
        int num = grade + 1;

        resourcePlanet = (ResourcePlanet)grade;
        infomation_Tabs[1].StarGradeSelect(num);

        (metal, cristal, gas) = (metal * num, cristal * num, gas * num);
        (cyon_Red, cyon_Green, cyon_Black) = (cyon_Red * num, cyon_Green * num, cyon_Black * num);

        haveResource = new List<Planet_Resources>();
        for (int i = 0; i < 3; i++)
            haveResource.Add(new Planet_Resources());

        haveResource[0].res_amount = metal;
        haveResource[0].cyon_amount = cyon_Red;
        haveResource[1].res_amount = cristal;
        haveResource[1].cyon_amount = cyon_Green;
        haveResource[2].res_amount = gas;
        haveResource[2].cyon_amount = cyon_Black;

    }



    public void 채굴고()
    {
        InvokeRepeating(nameof(MineResource), 0f, 1f);
    }

    public void 채굴스탑()
    {
        CancelInvoke(nameof(MineResource));
        Debug.Log("채굴종료");
    }
    void MineResource()
    {
        miningPerSecond = totalMiningRate / 3f;

        int baseMine = Mathf.FloorToInt(miningPerSecond);
        float fractional = miningPerSecond - baseMine;

        foreach (Planet_Resources res in haveResource)
        {
            res.res_amount -= baseMine;
            res.fractionalCarry += fractional;

            if (res.fractionalCarry >= 1f)
            {
                int extraLoss = Mathf.FloorToInt(res.fractionalCarry);
                res.res_amount -= extraLoss;
                res.fractionalCarry -= extraLoss;
            }
        }

    }

}


/*[System.Serializable]
public class Planet_Resources
{
    public int res_amount;
    public int cyon_amount;
    public float fractionalCarry = 0f;
}*/
