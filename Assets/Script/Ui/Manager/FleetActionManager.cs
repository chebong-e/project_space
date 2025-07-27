using System;
using System.Collections;
using UnityEngine;

public class FleetActionManager : MonoBehaviour
{
    public float miningspeed;
    public Ship[] ships;

    public void Ex_FleetMission()
    {
        // �Դ��� �̼��� ����
        Mission_Infomation mission_info = new Mission_Infomation();

        mission_info.event_Type = Event_Triggered.Event_Type.Nuetral_Missions;
        mission_info.coordinate = "�ݷδ� (999 , 999)";
        /*mission_info.fleetTypeAndCount = new Dictionary<int, int[]>(); */
        mission_info.fleetTypeAndCount = new FleetTypeAndCount();
        mission_info.fleetTypeAndCount[0] = new int[3] { 0, 4, 10 };
        mission_info.fleetTypeAndCount[1] = new int[3] { 0, 0, 30 };

        int shipsCount = 0;
        /*foreach (int key in mission_info.fleetTypeAndCount.Keys)
        {
            Debug.Log(Build_Manager.instance.scriptable_Group
                .shipGroups[mission_info.fleetTypeAndCount[key][0]]
                .ships[mission_info.fleetTypeAndCount[key][1]].name + $"�� �Լ����� {mission_info.fleetTypeAndCount[key][2]}");
            shipsCount += mission_info.fleetTypeAndCount[key][2];
        }
        Debug.Log($"�� �Լ��� : {shipsCount}");*/
        mission_info.fleetCount = shipsCount;
        mission_info.fleetSpeed = 2;
        mission_info.distance = 30;

        System.Random rnd = new System.Random();
        int val = rnd.Next(30, 100);
        mission_info.distance = val;

        // ������ �̼� ���� ����
        int index = 0;
        for (int i = 0; i < EventManager.instance.dropDown.dropDown_List.Length; i++)
        {
            if (EventManager.instance.dropDown.dropDown_List[i].GetComponent<EventLine>().event_Triggered.isUsed)
            {
                continue;
            }

            index = i;
            /*Debug.Log($"����ִ� ����: {index}");*/
            break;
        }
        Event_Triggered event_Triggered = EventManager.instance.dropDown.dropDown_List[index].GetComponent<EventLine>().event_Triggered;

        // �Դ� ��� ���� 
        event_Triggered.baseLocate = $"HomePlanet(111,222)";

        // �Ʒ��� ����
        event_Triggered.mission = mission_info;
        event_Triggered.timer = mission_info.distance / mission_info.fleetSpeed;
        EventManager.instance.Add_Event(event_Triggered, index);
    }

    public void FleetReturnToBase(int index)
    {
        

    }


    /*public void MiningFleetAction()
    {
        float spd = 1f;

        int index = 0;
        StartCoroutine(FleetMobilized(10, spd, 10f));
        for (int i = 0; i < EventManager.instance.dropDown.dropDown_List.Length; i++)
        {
            if (EventManager.instance.dropDown.dropDown_List[i].GetComponent<EventLine>().event_Triggered.isUsed)
                continue;

            index = i;
            Debug.Log($"����ִ� ����: {index}");

            break;
        }

        DropDown drop = EventManager.instance.dropDown;
        drop.dropDown_List[index].GetComponent<EventLine>().event_Triggered.isUsed = true;

        // �̺�Ʈ ���� �Ǵ� �߰�
        EventManager.instance.Add_Event();
        // �̺�Ʈ ���Կ� ��ǥ �� Ÿ�̸� ���� �����ϱ�
        drop.dropDown_List[index].GetComponent<EventLine>().targetText.text = $"�����༺ (123,456)";
        *//*drop.dropDown_List[index].GetComponent<EventLine>().timer = 10 / spd;*//*

        // �̺�Ʈ ���� Ȱ��ȭ
        if (!drop.dropBtns[0].gameObject.activeSelf)
        {
            drop.Add_LineDropDown(index, 10 / spd);
        }
        else
        {
            drop.EventWindowActivate();
        }

    }*/

    // �Դ����, �Դ�ӵ�(�Դ��� ���� �̵��ӵ��� ���), ��ǥ �Ÿ�
    IEnumerator FleetMobilized(int fleetcount, float speed, float distance)
    {
        Debug.Log("�Դ� �⵿");
        float timer = 0;

        // �������� ��ǥ�Ÿ� ����
        // �Ÿ� = 1000�϶� �ӵ��� 1�̸� 1000�ʰ� �ɸ��ٴ� �ܼ���� ����
        // �� �Ÿ� / �ӵ�
        // ���� �Ҹ�� �Ÿ� 1��
        // ���ҿ��� 1, ����Ʈ 1.5, �� ���� 2, �������� 3   => �����ϱ� ���� �ܼ� ��ġ
        // �ӵ��� ���� ���� ���� ������� 1, 2, 2.5, 3

        float distancePerSecond = distance / speed;
        while (timer < distancePerSecond)
        {
            timer += Time.deltaTime;

            if (timer >= 1f)
            {
                timer -= 1f;
                distancePerSecond -= 1f;
                Debug.Log("�Դ� �����...");
            }
           

            yield return null;
        }

        timer = 0;
        // ä���ð� ����� ���差 / ä���� (1000 / 5 = 200��)

        float miningTime = 1000 / miningspeed;
        int deposit = 1000;
        Debug.Log("�Դ� ���� ��� ����! �ӹ��� �����մϴ�.");
        float totalMining = 0f;
        while (timer < deposit / miningspeed)
        {
            timer += Time.deltaTime;

            if (timer >= 1f)
            {
                timer -= 1f;
                deposit -= Mathf.RoundToInt(miningspeed);
                totalMining += miningspeed;
                Debug.Log($"�ʴ� ������ {miningspeed} ä�����Դϴ�.");
                Debug.Log($"�� ���� {totalMining} ä�� �Ͽ����ϴ�..");
            }

            yield return null;
        }

        Debug.Log("ä���Ϸ�! ��ȯ�մϴ�");


    }
}
