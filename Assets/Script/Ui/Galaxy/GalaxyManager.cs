using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GalaxyManager : MonoBehaviour
{
    int[,] resource_Value;
    int[,] cyon_Value;
    int[] grade;

    public List<Planet_Resources> haveResource;
    public ScrollRect[] galaxyContainer;
    public GameObject[] planet_Prefabs;


    // 자원행성의 등급 설정 실험로직 08.07
    public List<GameObject> res_Planets = new List<GameObject>();


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
        ResourcePlanetGradeSet();
    }
    public void ResourcePlanetGradeSet()
    {
        int totalNum = 10;
        int[] baseCount = new int[5] { 1, 1, 1, 1, 1 };
        int[] weight = new int[5] { 40, 30, 15, 10, 5 };
        int sumBase = baseCount.Sum(); // = 5
        int remain = totalNum - sumBase;     // = 5

        // 정규화된 weight
        float totalWeight = weight.Sum(); // = 100
        float[] expected = new float[5];
        int[] rounded = new int[5];
        for (int i = 0; i < remain; i++)
        {
            expected[i] = weight[i] / totalWeight * remain;
            rounded[i] = Mathf.RoundToInt(expected[i]);
        }

        int total = rounded.Sum();
        int diff = total - remain;

        if (diff != 0)
        {
            // 3. 손실량 기준 정렬 (index 저장)
            int[] indices = Enumerable.Range(0, 5).ToArray();

            if (diff > 0)
            {
                // 초과일 경우: (expected - rounded)가 가장 작은 것부터 줄이기
                Array.Sort(indices, (a, b) =>
                    (expected[a] - rounded[a]).CompareTo(expected[b] - rounded[b]));

                for (int i = 0; i < diff; i++)
                    rounded[indices[i]] -= 1;
            }
            else
            {
                // 부족일 경우: (expected - rounded)가 가장 큰 것부터 늘리기
                Array.Sort(indices, (a, b) =>
                    (expected[b] - rounded[b]).CompareTo(expected[a] - rounded[a]));

                for (int i = 0; i < -diff; i++)
                    rounded[indices[i]] += 1;
            }
        }

        // 4. 최종 결과 출력
        Debug.Log("최종 결과:");

        grade = new int[5];
        for (int i = 0; i < 5; i++)
        {
            int totalPerGrade = baseCount[i] + rounded[i];
            Debug.Log($"{i + 1}성: {totalPerGrade}개");
            grade[i] = totalPerGrade;
        }

        


    }


    private void Start()
    {
        // 등급별 갯수는 설정되었지만 어떻게 할당할지는 아직 미정... 아래로직은 미완성

        for (int i = 0; i < res_Planets.Count; i++)
        {
            res_Planets[i].GetComponent<PlanetInfomation>().ResourcePlanet_GradeSet(grade[i]);
        }
    }
}

[System.Serializable]
public class Planet_Resources
{
    public int res_amount;
    public int cyon_amount;
    public float fractionalCarry = 0f;
}
