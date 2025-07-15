using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class ResourceWindow : SerializableDictionary<string, ResourceValue> { };
public class ResourceManager : MonoBehaviour
{

    public static ResourceManager instance;

    public GameObject resourceContainer;
    [Tooltip("key:자원명 \nvalue(GameObject(TMP) / 소지하고있는 자원의 값)")]
    public ResourceWindow resourceWindow; // 각 자원의 컨테이너 key값을 통해 찾기
    [Tooltip("metal, cristal, gas, energy")]
    public int[] basicProductions;  // private로 해도 됨
    public int[] build_Productions;
    public float[] total_Productions;

    float timer = 0f;
    float productionInterval = 1f;

    public int[] myRes;

    //
    public float[] productionPerSecond;
    public float[] temporarily_production;

    public int consume_Electric, increase_Electric;
    public int previousConsumeElectric = 0;
    Dictionary<string, int> previousValues = new Dictionary<string, int>();


    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        productionPerSecond = new float[3];
        temporarily_production = new float[3];

        foreach (SelfRegister_Res res in resourceContainer.GetComponentsInChildren<SelfRegister_Res>())
        {
            res.Init();
        }


        myRes[0] = 50000;
        myRes[1] = 20000;
        myRes[2] = 10000;
        myRes[3] = 200;

        resourceWindow["Metal"].container.text = ResourceMarkChange(myRes[0]);
        resourceWindow["Cristal"].container.text = ResourceMarkChange(myRes[1]);
        resourceWindow["Gas"].container.text = ResourceMarkChange(myRes[2]);
        resourceWindow["Energy"].container.text = ResourceMarkChange(myRes[3]);

    }

    void Update()
    {
        timer += Time.deltaTime;

        updateTotalProduction();
        total_Productions[3] = basicProductions[3] + build_Productions[3];
        myRes[3] = (int)total_Productions[3];

        if (timer >= productionInterval)
        {
            timer -= productionInterval;

            PlayerProduction_Apply();

        }
    }

    void updateTotalProduction()
    {
        for (int i = 0; i < 3; i++)
        {
            total_Productions[i] = calcultateProduction(i);
        }
    }

    public float calcultateProduction(int i)
    {
        float baseProd = basicProductions[i] + build_Productions[i];
        float researchBonus = PlayerAbilityInfo.ResourceMakingSpeed(i) * 0.05f;
        float result = baseProd + (baseProd * researchBonus);

        if (myRes[3] < 0 && myRes[3] > -1000)
        {
            result *= 0.5f;
        }
        else if (myRes[3] <= -1000 )
        {
            result *= 0.1f;
        }

        return result;
    }


    public void Electricity_Calculated(string callerID, int value)
    {
        int previousValue = previousValues.ContainsKey(callerID) ? previousValues[callerID] : 0;

        int delta = value - previousValue;

        if (value > 0)
        {
            consume_Electric += delta;
        }
        else
        {
            increase_Electric -= delta;
        }
        
        previousValues[callerID] = value;



        build_Productions[3] = increase_Electric - consume_Electric;
        myRes[3] = basicProductions[3] + build_Productions[3];







        /*build_Productions[3] = increase_Electric - consume_Electric;

        myRes[3] = basicProductions[3] + build_Productions[3];*/
        resourceWindow["Energy"].container.text = ResourceMarkChange(myRes[3]);
    }

    // 플레이어의 연구 능력 및 기타 특정 능력으로 인한 자원 생성 상승률 반영 로직
    void PlayerProduction_Apply()
    {
        for (int i = 0; i < 3; i++)
        {
            productionPerSecond[i] = total_Productions[i] / 3600f;
            temporarily_production[i] += productionPerSecond[i];

            if (temporarily_production[i] >= 1f)
            {
                int tem_res = Mathf.FloorToInt(temporarily_production[i]);
                myRes[i] += tem_res;
                temporarily_production[i] -= tem_res;
                resourceWindow[i == 0 ? "Metal" : i == 1 ? "Cristal" : "Gas"].container.text = ResourceMarkChange(myRes[i]);
            }
        }
    }

    // 건설 및 업그레이드에 따른 자원 감소처리 로직
    public bool UpgradePerDeduct(int[] cost, bool confirm)
    {
        if (confirm)
        {
            for (int i = 0; i < cost.Length; i++)
            {
                if (myRes[i] < cost[i])
                {
                    // 자원이 모잘라서 업그레이드 또는 생산이 되지 않을때는 화면 중앙에 0.5초? ~1초 정도 
                    // 안내 창이 뜨는 팝업 표시 구현하도록
                    Debug.Log("자원 모자름");
                    return false;
                }
            }
        }

        for (int i = 0; i < cost.Length; i++)
        {
            if (confirm)
            {
                myRes[i] -= cost[i];
            }
            else
            {
                myRes[i] += cost[i];
            }
            resourceWindow[i == 0 ? "Metal" : i == 1 ? "Cristal" : "Gas"].container.text
                    = $"{ResourceMarkChange(myRes[i])}";
        }
        return true;
    }

    public string ResourceMarkChange(long value)
    {
        float raw;
        float floored;
        if (value >= 1_000_000_000)
        {
            raw = value / 1_000_000_000f;
            floored = Mathf.Floor(raw * 10f) / 10f;
            return floored.ToString("0.#") + "B";
        }
        else if (value >= 1_000_000)
        {
            raw = value / 1_000_000f;
            floored = Mathf.Floor(raw * 10f) / 10f;
            return floored.ToString("0.#") + "M";
        }
        else if (value >= 1_000)
        {
            raw = value / 1_000f;
            floored = Mathf.Floor(raw * 10f) / 10f;
            return floored.ToString("0.#") + "K";
        }
        else return value.ToString();
    }
}

[System.Serializable]
public class ResourceValue
{
    public TextMeshProUGUI container;
    public int value;

}
