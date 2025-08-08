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


    // �ڿ��༺�� ��� ���� ������� 08.07
    public List<GameObject> res_Planets = new List<GameObject>();


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
        ResourcePlanetGradeSet();
    }
    public void ResourcePlanetGradeSet()
    {
        int totalNum = 10;
        int[] baseCount = new int[5] { 1, 1, 1, 1, 1 };
        int[] weight = new int[5] { 40, 30, 15, 10, 5 };
        int sumBase = baseCount.Sum(); // = 5
        int remain = totalNum - sumBase;     // = 5

        // ����ȭ�� weight
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
            // 3. �սǷ� ���� ���� (index ����)
            int[] indices = Enumerable.Range(0, 5).ToArray();

            if (diff > 0)
            {
                // �ʰ��� ���: (expected - rounded)�� ���� ���� �ͺ��� ���̱�
                Array.Sort(indices, (a, b) =>
                    (expected[a] - rounded[a]).CompareTo(expected[b] - rounded[b]));

                for (int i = 0; i < diff; i++)
                    rounded[indices[i]] -= 1;
            }
            else
            {
                // ������ ���: (expected - rounded)�� ���� ū �ͺ��� �ø���
                Array.Sort(indices, (a, b) =>
                    (expected[b] - rounded[b]).CompareTo(expected[a] - rounded[a]));

                for (int i = 0; i < -diff; i++)
                    rounded[indices[i]] += 1;
            }
        }

        // 4. ���� ��� ���
        Debug.Log("���� ���:");

        grade = new int[5];
        for (int i = 0; i < 5; i++)
        {
            int totalPerGrade = baseCount[i] + rounded[i];
            Debug.Log($"{i + 1}��: {totalPerGrade}��");
            grade[i] = totalPerGrade;
        }

        


    }


    private void Start()
    {
        // ��޺� ������ �����Ǿ����� ��� �Ҵ������� ���� ����... �Ʒ������� �̿ϼ�

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
