using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;
    public int eventTriggered;
    public DropDown dropDown;

    //Ȯ�ο�
    public GameObject[] Ex_contentsCheck;
    public GameObject canvas;


    public GameObject[] TabContainer;


    public float metal_RefillTimer;
    public float cristal_RefillTimer;
    public float gas_RefillTimer;
    public float total_Timer;

    public Sprite[] imgs;

    public Event_Triggered[] events;


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
    }

    public void Add_Event()
    {
        eventTriggered++;

        dropDown.triggered_event = eventTriggered;
    }

    public void Add_Event(Mission_Infomation mission)
    {
        eventTriggered++;
        dropDown.triggered_event = eventTriggered;

        int index = 0;
        for (int i = 0; i < dropDown.dropDown_List.Length; i++)
        {
            if (dropDown.dropDown_List[i].GetComponent<EventLine>().event_Triggered.isUsed)
            {
                continue;
            }

            index = i;
            Debug.Log($"����ִ� ����: {index}");
            break;
        }

        EventLine e_Line = dropDown.dropDown_List[index].GetComponent<EventLine>();
        e_Line.event_Triggered.isUsed = true;
        e_Line.missionImg.sprite = imgs[(int)mission.event_Type];
        e_Line.targetText.text = $"{mission.coordinate}";
        
        if (!dropDown.dropBtns[0].gameObject.activeSelf)
        {
            dropDown.Add_LineDropDown(index, mission.distance / mission.fleetSpeed);
        }
        else
        {
            dropDown.EventWindowActivate();
        }

        StartCoroutine(MobilizeAFleet(mission, e_Line.timerText));
    }

    IEnumerator MobilizeAFleet(Mission_Infomation mission, TextMeshProUGUI timeText)
    {
        Debug.Log("�Դ� �⵿");
        float timer = 0;

        // �������� ��ǥ�Ÿ� ����
        // �Ÿ� = 1000�϶� �ӵ��� 1�̸� 1000�ʰ� �ɸ��ٴ� �ܼ���� ����
        // �� �Ÿ� / �ӵ�
        // ���� �Ҹ�� �Ÿ� 1��
        // ���ҿ��� 1, ����Ʈ 1.5, �� ���� 2, �������� 3   => �����ϱ� ���� �ܼ� ��ġ
        // �ӵ��� ���� ���� ���� ������� 1, 2, 2.5, 3

        float distancePerSecond = mission.distance / mission.fleetSpeed;

        while (distancePerSecond > 0)
        {
            distancePerSecond -= Time.deltaTime;

            TimeSpan timespan = TimeSpan.FromSeconds(distancePerSecond);
            timeText.text = timespan.ToString(@"hh\:mm\:ss");

            yield return null;
        }

        switch (mission.event_Type)
        {
            case Event_Triggered.Event_Type.Attack:
                MissionToAttack();
                break;
            case Event_Triggered.Event_Type.Missions:
                yield return StartCoroutine(MissionToMining(mission, timeText));
                Debug.Log("ä���Ϸ�. ��ȯ!");
                break;
            case Event_Triggered.Event_Type.UnionSupport:
                MissionToUnionSupport();
                break;
        }
 

    }

    
    void MissionToAttack()
    {

    }

    IEnumerator MissionToMining(Mission_Infomation mission, TextMeshProUGUI timeText)
    {
        Debug.Log(mission.fleetTypeToCount[0][0]);
        timeText.text = " - ";

        float timer = 0;

        // �ش� ������ �ڿ��� Ȯ�� ����
        // �ڿ�Ȯ�η���();
        // �켱 ���� �ۼ��� ���÷� 1000�� �ڿ��� �ִٴ� ����
        float mineVolume = 1000;
        Debug.Log(mission.fleetTypeToCount.Keys);
        float perSec = mineVolume / Build_Manager.instance.scriptable_Group.shipGroups[0].ships[mission.fleetTypeToCount[0][0]].miningRate;
        while (timer < perSec)
        {
            timer += Time.deltaTime;

            if (timer > 1f)
            {
                timer = 0f;
                perSec -= 1f;
                mineVolume -= Build_Manager.instance.scriptable_Group.shipGroups[0].ships[mission.fleetTypeToCount[0][0]].miningRate;
                Debug.Log($"�ʴ� ������ {Build_Manager.instance.scriptable_Group.shipGroups[0].ships[mission.fleetTypeToCount[0][0]].miningRate} ä�����Դϴ�.");
                Debug.Log($"�� ���� {mineVolume} ���� �ֽ��ϴ�.");
                Debug.Log($"���� �ð�:{perSec}");
            }

            
            yield return null;
        }

    }

    void MissionToUnionSupport()
    {

    }


    void OnDestroy()
    {
        foreach (Event_Triggered even in events)
        {
            even.isUsed = false;
        }
    }

}

public struct Mission_Infomation
{
    public Event_Triggered.Event_Type event_Type;
    public string coordinate;
    public int fleetCount;
    // key: �Լ��׷��̵�, value: [0]�ε��� = �ش�׷��̵��� ����, [1]�ε��� = �ش� �Լ��� �� 
    public Dictionary<int, int[]> fleetTypeToCount;
    public float fleetSpeed;
    public float distance;
}