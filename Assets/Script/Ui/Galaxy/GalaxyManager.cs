using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class GalaxyManager : MonoBehaviour
{
    public static GalaxyManager instance;

    int[,] resource_Value;
    int[,] cyon_Value;
    int[] grade;

    public GameObject[] galaxyMapContainer;
    public GameObject[] Galaxy_Map1;
    public GalaxyMap[] galaxyMaps;
    public List<Planet_Resources> haveResource;
    public GameObject[] galaxyContainer; // �ε��� ���� �� ���ϸ� ����
    public GameObject galaxyMap_Prefab;
    public GameObject[] planet_Prefabs;
    [SerializeField]
    [Range(10, 130)]
    private int galaxyMap = 10;
    
    // ex
    public int[] finalCount;

    // �ڿ��༺�� ��� ���� ������� 08.07
    public List<GameObject> res_Planets = new List<GameObject>();


    public int[] grade_Sum = new int[5];


    // �ڿ��༺�� �׷��̵� ��� ���� ����
    /*public void ResourcePlanet_GradeSet()
    {
        int resIndex = Random.Range(0, 4);
        int cyIndex = Random.Range(0, 4);
        Debug.Log($"�ڿ�: {resIndex}, �ÿ�: {cyIndex}");


        int metal = resource_Value[resIndex, 0];
        int cristal = resource_Value[resIndex, 1];
        int gas = resource_Value[resIndex, 2];
        int cyon_Red = cyon_Value[cyIndex, 0];
        int cyon_Green = cyon_Value[cyIndex, 1];
        int cyon_Black = cyon_Value[cyIndex, 2];

        haveResource.Clear();
        int grade = Random.Range(0, 5);
        int num = grade + 1;

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
    }*/

    private void Awake()
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

        ResourcePlanetGradeSet();
    }
    public void ResourcePlanetGradeSet() // ���� ����ġ ����ȭ ���� �ߵǴ��� Ȯ�� ����
    {
        int totalNum = galaxyMap;
        int[] baseCount = { 1, 1, 1, 1, 1 };
        int[] weight = { 40, 30, 15, 10, 5 };

        int sumBase = baseCount.Sum();
        int remain = totalNum - sumBase;

        float totalWeight = weight.Sum();
        float[] expected = new float[5];
        int[] rounded = new int[5];

        // 1. weight ������ ������ ���� ���
        for (int i = 0; i < 5; i++)
        {
            expected[i] = weight[i] / totalWeight * remain;
            rounded[i] = Mathf.RoundToInt(expected[i]);
        }

        // 2. �ݿø� ���� ����
        int total = rounded.Sum();
        int diff = total - remain;
        if (diff != 0)
        {
            int[] indices = Enumerable.Range(0, 5).ToArray();

            if (diff > 0) // �ʰ� �� ���̱�
            {
                Array.Sort(indices, (a, b) =>
                    (expected[a] - rounded[a]).CompareTo(expected[b] - rounded[b]));
                for (int i = 0; i < diff; i++)
                    rounded[indices[i]] -= 1;
            }
            else // ���� �� �ø���
            {
                Array.Sort(indices, (a, b) =>
                    (expected[b] - rounded[b]).CompareTo(expected[a] - rounded[a]));
                for (int i = 0; i < -diff; i++)
                    rounded[indices[i]] += 1;
            }
        }

        // 3. �⺻���� ����
        finalCount = new int[5];
        for (int i = 0; i < 5; i++)
            finalCount[i] = baseCount[i] + rounded[i];

        // 4. �ִ밪 ���� ��� (���� �� ����)
        float[] percentLimit = { 0.40f, 0.30f, 0.15f, 0.10f, 0.05f };
        int[] maxLimit = new int[5];
        float[] rawValues = new float[5];
        int sumLimit = 0;

        for (int i = 0; i < 5; i++)
        {
            rawValues[i] = totalNum * percentLimit[i];
            maxLimit[i] = Mathf.FloorToInt(rawValues[i]);
            sumLimit += maxLimit[i];
        }

        // �������� ������ �Ҽ��� �ս� ū ������� ä���
        int limitDiff = totalNum - sumLimit;
        if (limitDiff > 0)
        {
            int[] indices = Enumerable.Range(0, 5).ToArray();
            Array.Sort(indices, (a, b) =>
                (rawValues[b] - maxLimit[b]).CompareTo(rawValues[a] - maxLimit[a]));

            for (int i = 0; i < limitDiff; i++)
                maxLimit[indices[i % 5]]++;
        }

        // 5. �ִ밪 ����
        for (int i = 0; i < 5; i++)
            finalCount[i] = Mathf.Min(finalCount[i], maxLimit[i]);

        // 6. ���� �� ��й�
        int currentTotal = finalCount.Sum();
        int leftOver = totalNum - currentTotal;

        if (leftOver > 0)
        {
            for (int i = 0; i < 5; i++)
            {
                if (finalCount[i] < maxLimit[i])
                {

                    finalCount[i]++;
                    leftOver--;
                }
            }
        }

        // 7. ��� ���
        for (int i = 0; i < 5; i++)
        {
            Debug.Log($"{i + 1}��: {finalCount[i]}�� (�ִ� {maxLimit[i]}��)");
        }
            

        Debug.Log(finalCount.Sum());

        
    }

    public void CreatGalaxyMap(int creatNum)
    {
        for (int i = 0; i < creatNum; i++)
        {
            GameObject mapObj = Instantiate(galaxyMap_Prefab, galaxyContainer[1].transform);
            mapObj.transform.SetSiblingIndex(3);
            mapObj.name = $"Galaxy_{i}";
            int ranLen = UnityEngine.Random.Range(12, 19);
            mapObj.GetComponent<GalaxyInitSet>().Galaxy_InitSetting(ranLen, i);
        }

        int len = res_Planets.Count;

        List<int> values = new List<int>() { 0, 1, 2, 3, 4 };
        float[] weights = { 40f, 30f, 15f, 10f, 5f };

        for (int i = 0; i < len; i++)
        {
            int getNumber = GetWeightedRandom(values, weights);
            while (grade_Sum[getNumber] >= finalCount[getNumber])
            {
                values.Remove(getNumber);
                if (values.Count == 0)
                    break;
                getNumber = GetWeightedRandom(values, weights);
            }

            grade_Sum[getNumber]++;

            res_Planets[i].GetComponent<PlanetInfomation>().ResourcePlanet_GradeSet(getNumber);
        }

        GameObject[] gg = galaxyMapContainer[0].GetComponentsInChildren<ScrollRect>()
            .Select(sr => sr.gameObject)
            .ToArray();
        galaxyMaps = new GalaxyMap[2];
        galaxyMaps[0] = new GalaxyMap();
        galaxyMaps[0].galaxy_Index = galaxyMapContainer[0];
        galaxyMaps[0].galaxy_Index_To_Map = gg;
        Array.Reverse(galaxyMaps[0].galaxy_Index_To_Map);


        /*foreach (GameObject obj in galaxyMaps[0].galaxy_Index_To_Map)
        {
            obj.SetActive(false);
        }*/
        galaxyMaps[0].galaxy_Index_To_Map[0].SetActive(true);
    }

    public void NextMap_Or_Back(int num)
    {
        int index = 0;
        for (int i = 0; i < galaxyMaps[0].galaxy_Index_To_Map.Length; i++)
        {
            if (galaxyMaps[0].galaxy_Index_To_Map[i].activeInHierarchy)
            {
                index = i;
                break;
            }
        }

        galaxyMaps[0].galaxy_Index_To_Map[index].SetActive(false);
        Debug.Log(index);
        if (index == 0 && num == -1)
        {
            index = galaxyMaps[0].galaxy_Index_To_Map.Length;
        }
        else if (index == 29 && num == 1)
        {
            index = -1;
        }
        galaxyMaps[0].galaxy_Index_To_Map[index + num].SetActive(true);
        
    }



    int GetWeightedRandom(List<int> values, float[] weights)
    {
        float totalWeight = 0f;
        for (int i = 0; i < weights.Length; i++)
            totalWeight += weights[i];

        float randomValue = UnityEngine.Random.Range(0, totalWeight);
        float cumulative = 0f;

        for (int i = 0; i < values.Count; i++)
        {
            cumulative += weights[i];
            if (randomValue < cumulative)
                return values[i];
        }

        return values[values.Count - 1]; // ���� ������
    }

    private void Start()
    {
        CreatGalaxyMap(galaxyMap);
    }







    /*private void Start()
    {
        // ��޺� ������ �����Ǿ����� ��� �Ҵ������� ���� ����... �Ʒ������� �̿ϼ�

        for (int i = 0; i < res_Planets.Count; i++)
        {
            res_Planets[i].GetComponent<PlanetInfomation>().ResourcePlanet_GradeSet(grade[i]);
        }
    }*/
}

[System.Serializable]
public class Planet_Resources
{
    public int res_amount;
    public int cyon_amount;
    public float fractionalCarry = 0f;
}

[System.Serializable]
public class GalaxyMap
{
    public GameObject galaxy_Index;
    public GameObject[] galaxy_Index_To_Map;
}
