using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class GalaxyManager : MonoBehaviour
{
    public static GalaxyManager instance;

    int[,] resource_Value;
    int[,] cyon_Value;
    int[] grade;

    public List<Planet_Resources> haveResource;
    public GameObject[] galaxyContainer; // 인덱스 별로 각 은하를 지정
    public GameObject galaxyMap_Prefab;
    public GameObject[] planet_Prefabs;
    [SerializeField]
    [Range(10, 130)]
    private int galaxyMap = 10;

    // ex
    List<int> res_grade;
    public int[] finalCount;

    // 자원행성의 등급 설정 실험로직 08.07
    public List<GameObject> res_Planets = new List<GameObject>();




    public int grade_0_Sum = 0;
    public int grade_1_Sum = 0;
    public int grade_2_Sum = 0;
    public int grade_3_Sum = 0;
    public int grade_4_Sum = 0;



    // 자원행성의 그레이드 등급 설정 로직
    /*public void ResourcePlanet_GradeSet()
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
    public void ResourcePlanetGradeSet()
    {
        int totalNum = galaxyMap;
        int[] baseCount = { 1, 1, 1, 1, 1 };
        int[] weight = { 40, 30, 15, 10, 5 };

        int sumBase = baseCount.Sum();
        int remain = totalNum - sumBase;

        float totalWeight = weight.Sum();
        float[] expected = new float[5];
        int[] rounded = new int[5];

        // 1. weight 비율로 나머지 개수 계산
        for (int i = 0; i < 5; i++)
        {
            expected[i] = weight[i] / totalWeight * remain;
            rounded[i] = Mathf.RoundToInt(expected[i]);
        }

        // 2. 반올림 오차 보정
        int total = rounded.Sum();
        int diff = total - remain;
        if (diff != 0)
        {
            int[] indices = Enumerable.Range(0, 5).ToArray();

            if (diff > 0) // 초과 → 줄이기
            {
                Array.Sort(indices, (a, b) =>
                    (expected[a] - rounded[a]).CompareTo(expected[b] - rounded[b]));
                for (int i = 0; i < diff; i++)
                    rounded[indices[i]] -= 1;
            }
            else // 부족 → 늘리기
            {
                Array.Sort(indices, (a, b) =>
                    (expected[b] - rounded[b]).CompareTo(expected[a] - rounded[a]));
                for (int i = 0; i < -diff; i++)
                    rounded[indices[i]] += 1;
            }
        }

        // 3. 기본보장 포함
        finalCount = new int[5];
        for (int i = 0; i < 5; i++)
            finalCount[i] = baseCount[i] + rounded[i];

        // 4. 최대값 제한 계산 (버림 후 보정)
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

        // 부족분이 있으면 소수점 손실 큰 순서대로 채우기
        int limitDiff = totalNum - sumLimit;
        if (limitDiff > 0)
        {
            int[] indices = Enumerable.Range(0, 5).ToArray();
            Array.Sort(indices, (a, b) =>
                (rawValues[b] - maxLimit[b]).CompareTo(rawValues[a] - maxLimit[a]));

            for (int i = 0; i < limitDiff; i++)
                maxLimit[indices[i % 5]]++;
        }

        // 5. 최대값 적용
        for (int i = 0; i < 5; i++)
            finalCount[i] = Mathf.Min(finalCount[i], maxLimit[i]);

        // 6. 남은 수 재분배
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

        // 7. 결과 출력
        res_grade = new List<int>();
        for (int i = 0; i < 5; i++)
        {
            Debug.Log($"{i + 1}성: {finalCount[i]}개 (최대 {maxLimit[i]}개)");
            res_grade.Add(finalCount[i]);
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
        
        for (int i = 0; i < len; i++)
        {
            int index = UnityEngine.Random.Range(0, len + 1);





            /*int ranGrade = UnityEngine.Random.Range(0, 5);

            if (ranGrade == 0)
            {
                if (finalCount[ranGrade] > grade_0_Sum)
                    grade_0_Sum++;
                else
                {
                    ranGrade = UnityEngine.Random.Range(1, 5);
                }
            }
            else if (ranGrade == 1)
            {
                if (finalCount[ranGrade] > grade_1_Sum)
                    grade_1_Sum++;
            }
            else if (ranGrade == 2) grade_2_Sum++;
            else if (ranGrade == 3) grade_3_Sum++;
            else grade_4_Sum++;

            if (grade_0_Sum < finalCount[0])*/
            res_Planets[i].GetComponent<PlanetInfomation>().ResourcePlanet_GradeSet(RandomSelected_To_Grade());
        }
    }

    int RandomSelected_To_Grade()
    {
        int ranGrade = UnityEngine.Random.Range(0, 5);

        List<int> ranGradeInt = new List<int>() { 0, 1, 2, 3, 4 };
        int ran = ranGradeInt[UnityEngine.Random.Range(0, ranGradeInt.Count)];

        if (ran == 0)
        {
            if (finalCount[ran] > grade_0_Sum)
            {
                ++grade_0_Sum;
                Debug.Log($"최댓값:{finalCount[ran]} / {grade_0_Sum}");
                
            }
            else
            {
                ranGradeInt.Remove(0);
                ran = ranGradeInt[UnityEngine.Random.Range(0, ranGradeInt.Count)];
            }
        }
        else if (ran == 1)
        {
            if (finalCount[ran] > grade_1_Sum)
            {
                ++grade_1_Sum;
                Debug.Log($"최댓값:{finalCount[ran]} / {grade_1_Sum}");
            }
                
            else
            {
                ranGradeInt.Remove(1);
                ran = ranGradeInt[UnityEngine.Random.Range(0, ranGradeInt.Count)];
            }
        }
        else if (ran == 2)
        {
            if (finalCount[ran] > grade_2_Sum)
            {
                ++grade_2_Sum;
                Debug.Log($"최댓값:{finalCount[ran]} / {grade_2_Sum}");
            }
            else
            {
                ranGradeInt.Remove(2);
                ran = ranGradeInt[UnityEngine.Random.Range(0, ranGradeInt.Count)];
            }
        }
        else if (ran == 3)
        {
            if (finalCount[ran] > grade_3_Sum)
            {
                ++grade_3_Sum;
                Debug.Log($"최댓값:{finalCount[ran]} / {grade_3_Sum}");
            }
            else
            {
                ranGradeInt.Remove(3);
                ran = ranGradeInt[UnityEngine.Random.Range(0, ranGradeInt.Count)];
            }
        }
        else
        {
            if (finalCount[ran] > grade_4_Sum)
            {
                ++grade_4_Sum;
                Debug.Log($"최댓값:{finalCount[ran]} / {grade_4_Sum}");
            }
            else
            {
                ranGradeInt.Remove(4);
                ran = ranGradeInt[UnityEngine.Random.Range(0, ranGradeInt.Count)];
            }
        }

        Debug.Log(ran);
        return ran;
    }




    private void Start()
    {
        CreatGalaxyMap(galaxyMap);
    }







    /*private void Start()
    {
        // 등급별 갯수는 설정되었지만 어떻게 할당할지는 아직 미정... 아래로직은 미완성

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
