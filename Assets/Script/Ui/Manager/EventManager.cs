using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

[System.Serializable]
public class MiningDic : SerializableDictionary<int, Coroutine> { };
public class EventManager : MonoBehaviour
{
    public static EventManager instance;
    public int eventCount;
    public DropDown dropDown;

    public Missions missions;
    public GameObject canvas;


    public GameObject[] TabContainer;
    public TextMeshProUGUI[] missionsCountText;
    public Sprite[] imgs;

    public Event_Triggered[] events;
    public List<GameObject> Ex_To_events;

    Coroutine[] eventCoroutine;
    /*Coroutine[] miningSlot;*/


    public MiningDic miningSlot;
    public Dictionary<int, int> mining_Resource;



    int nuetral, friendly, hostile;



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

        eventCoroutine = new Coroutine[10];
        missions = new Missions();

        miningSlot = new MiningDic();
        mining_Resource = new Dictionary<int, int>(); //�ӽ÷� ä������ 2���� ����
    }

    public void Add_Event(Event_Triggered event_, int index)
    {
        eventCount++;
        dropDown.triggered_event = eventCount;


        // 0: �߸� �̼�
        // 1: ��ȣ �̼�
        // 2: ���� �̼�
        switch (event_.mission.event_Type)
        {
            case Event_Triggered.Event_Type.Nuetral_Missions:
                missions.neutral_Mission++;
                missionsCountText[0].text = $"[ {missions.neutral_Mission} ]";
                break;
            case Event_Triggered.Event_Type.UnionSupport:
                missions.friendly_Mission++;
                missionsCountText[1].text = $"[ {missions.friendly_Mission} ]";
                break;
            case Event_Triggered.Event_Type.Attack:
            case Event_Triggered.Event_Type.UnderAttack:
                missions.hostile_Mission++;
                missionsCountText[2].text = $"[ {missions.hostile_Mission} ]";
                break;
        }


        EventLine e_Line = dropDown.dropDown_List[index].GetComponent<EventLine>();
        e_Line.transform.GetChild(2).gameObject.SetActive(true);
        e_Line.event_Triggered.isUsed = true;
        e_Line.missionImg.sprite = imgs[(int)event_.mission.event_Type];
        e_Line.targetText.text = $"{event_.mission.coordinate}";
        
        if (!dropDown.dropBtns[0].gameObject.activeSelf)
        {
            dropDown.SlideEventImplement(2);
        }
        else
        {
            dropDown.EventWindowActivate();
        }

        // �������� Ȯ��
        EventSequence_Realignment();


        eventCoroutine[index] = StartCoroutine(MobilizeAFleet(e_Line, index));
    }

    public void FleetReturnToBase(int index)
    {
        StopCoroutine(eventCoroutine[index]);

        EventLine eLine = dropDown.dropDown_List[index].GetComponent<EventLine>();

        eLine.missionImg.sprite = imgs[4];
        eLine.transform.GetChild(2).gameObject.SetActive(false);

        if (miningSlot.ContainsKey(index))
        {
            Debug.Log($"�� {miningSlot.Count}ä���Դ� �� {index}��ȣ�� �Դ� ��ȯ����");
            StopCoroutine(miningSlot[index]);
            miningSlot.Remove(index);
        }

        Debug.Log("��ȯ �մϴ�!");
        StartCoroutine(ReturnToBase(eLine));
    }
    
    IEnumerator ReturnToBase(EventLine eventLine)
    {
        float timer = 0f;

        float sec = (eventLine.event_Triggered.mission.distance / eventLine.event_Triggered.mission.fleetSpeed)
            - eventLine.event_Triggered.timer;
        int remainingTime = 0;
        eventLine.event_Triggered.timer = sec;
        EventSequence_Realignment();
        while (sec > 0)
        {
            timer = Time.deltaTime;
            sec -= timer;
            eventLine.event_Triggered.timer = sec;
            remainingTime = Mathf.CeilToInt(Mathf.Clamp(sec - timer, 0f, sec)); ;

            TimeSpan timespan = TimeSpan.FromSeconds(remainingTime);
            eventLine.timerText.text = timespan.ToString(@"hh\:mm\:ss");

            yield return null;
        }

        eventCount--;
        dropDown.triggered_event = eventCount;

        switch (eventLine.event_Triggered.mission.event_Type)
        {
            case Event_Triggered.Event_Type.Nuetral_Missions:
                missions.neutral_Mission--;
                missionsCountText[0].text = $"[ {missions.neutral_Mission} ]";
                break;
            case Event_Triggered.Event_Type.UnionSupport:
                missions.friendly_Mission--;
                missionsCountText[1].text = $"[ {missions.friendly_Mission} ]";
                break;
            case Event_Triggered.Event_Type.Attack:
            case Event_Triggered.Event_Type.UnderAttack:
                missions.hostile_Mission--;
                missionsCountText[2].text = $"[ {missions.hostile_Mission} ]";
                break;
        }


        eventLine.event_Triggered.isUsed = false;

        eventLine.gameObject.SetActive(false);
        // �̺�Ʈ ���� �����̵� ó�� �ؾ���.
        dropDown.SlideEventImplement(3);
    }

    IEnumerator MobilizeAFleet(EventLine eventLine, int index)
    {
        Debug.Log("�Դ� �⵿");
        float timer = 0f;
        eventLine.event_Triggered.timer = 0;

        // �������� ��ǥ�Ÿ� ����
        // �Ÿ� = 1000�϶� �ӵ��� 1�̸� 1000�ʰ� �ɸ��ٴ� �ܼ���� ����
        // �� �Ÿ� / �ӵ�
        // ���� �Ҹ�� �Ÿ� 1��
        // ���ҿ��� 1, ����Ʈ 1.5, �� ���� 2, �������� 3   => �����ϱ� ���� �ܼ� ��ġ
        // �ӵ��� ���� ���� ���� ������� 1, 2, 2.5, 3

        float distancePerSecond = eventLine.event_Triggered.mission.distance / eventLine.event_Triggered.mission.fleetSpeed;
        int remainingTime = 0;
        while (distancePerSecond > 0)
        {
            timer = Time.deltaTime;
            distancePerSecond -= timer;
            eventLine.event_Triggered.timer = distancePerSecond;

            remainingTime = Mathf.CeilToInt(distancePerSecond);
            /*distancePerSecond = Mathf.CeilToInt(Mathf.Clamp(distancePerSecond - timer, 0f, distancePerSecond));*/

            TimeSpan timespan = TimeSpan.FromSeconds(remainingTime);
            eventLine.timerText.text = timespan.ToString(@"hh\:mm\:ss");
            yield return null;
        }

        switch (eventLine.event_Triggered.mission.event_Type)
        {
            case Event_Triggered.Event_Type.Attack:
                MissionToAttack(index);
                break;
            case Event_Triggered.Event_Type.Nuetral_Missions:
                // �߸� �̼ǿ��� �ܼ� �̵��� �������� ä������ �����ؾ���.
                // ä�� �̼��̶��

                yield return miningSlot[index] = StartCoroutine(MissionToMining(eventLine, index));
                Debug.Log("ä���Ϸ�. ��ȯ!");
                break;
            case Event_Triggered.Event_Type.UnionSupport:
                MissionToUnionSupport();
                break;
        }
 

    }

    // ������ ���� �Լ��� ���� ���� �� ��ȯ
    void MissionToAttack(int index)
    {
        Debug.Log("��������!");
        // �� 4������� ����
        // �������� �ۼ��ؾ���


        //���� ���� �� ��ȯ����
        FleetReturnToBase(index);
    }

    IEnumerator MissionToMining(EventLine eventLine, int index)
    {
        eventLine.timerText.text = " - ";


        int cago = 0; // �ӽ÷� ī�� ����
        float timer = 0;

        // �ش� ������ �ڿ��� Ȯ�� ����
        // �ڿ�Ȯ�η���();
        // �켱 ���� �ۼ��� ���÷� 1000�� �ڿ��� �ִٴ� ����
        float mineVolume = 1000;
        float miningVolumePerSecond = Build_Manager.instance.scriptable_Group.shipGroups[0].ships[eventLine.event_Triggered.mission.fleetTypeAndCount[0][1]].miningRate;
        float perSec = mineVolume / miningVolumePerSecond;
        while (timer < perSec)
        {
            timer += Time.deltaTime;

            if (timer > 1f)
            {
                timer = 0f;
                perSec -= 1f;
                mineVolume -= miningVolumePerSecond;
                cago += (int)miningVolumePerSecond;
                Debug.Log($"�ʴ� ������ {miningVolumePerSecond} ä�����Դϴ�.");
                Debug.Log($"�� ���� {mineVolume} ���� �ֽ��ϴ�.");
                Debug.Log($"���� �ð�:{perSec}");
                mining_Resource[index] = cago;
            }

            
            yield return null;
        }

    }

    void MissionToUnionSupport()
    {

    }



    public void EventSequence_Realignment() // �̺�Ʈ �߻��� �׿� ���� ������
    {
        Ex_To_events = new List<GameObject>();

        List<(GameObject obj, float time)> dropDownlist = new List<(GameObject obj, float time)>();

        Event_Triggered event_;



        for (int i = 0; i < dropDown.dropDown_List.Length; i++)
        {
            if (dropDown.dropDown_List[i].GetComponent<EventLine>().event_Triggered.isUsed)
            {
                event_ = dropDown.dropDown_List[i].GetComponent<EventLine>().event_Triggered;
                dropDownlist.Add((dropDown.dropDown_List[i], event_.timer));

                
            }

        }

        //
        dropDownlist.Sort((a, b) => CompareLineObject(a, b));
        //

        /*dropDownlist.Sort((a, b) => b.time.CompareTo(a.time));*/

        for (int i = 0; i < dropDownlist.Count; i++)
        {
            dropDownlist[i].obj.transform.SetSiblingIndex(i);
            
        }

        for (int i = 0; i < dropDownlist.Count; i++)
        {
            Ex_To_events.Add(dropDownlist[i].obj);
            dropDown.sorting_DropDownList[i] = dropDownlist[i].obj;
        }

    }

    int CompareLineObject((GameObject obj, float time) a, (GameObject obj, float time) b)
    {
        int a_weightValue = 0;
        int b_weightValue = 0;
        if (!a.obj.transform.GetChild(2).gameObject.activeSelf) a_weightValue = 1000;
        if (!b.obj.transform.GetChild(2).gameObject.activeSelf) b_weightValue = 1000;
        int aValue = (int)a.time - a_weightValue;
        int bValue = (int)b.time - b_weightValue;

        return bValue.CompareTo(aValue);
    }



    void OnDestroy()
    {
        foreach (Event_Triggered even in events)
        {
            even.isUsed = false;
        }
    }

}


[System.Serializable]
public class FleetTypeAndCount : SerializableDictionary<int, int[]> { };
[System.Serializable]
public struct Mission_Infomation
{
    public Event_Triggered.Event_Type event_Type;
    public Event_Triggered.NuetralCategory nuetralCategory;
    public int miningNumber;
    public string coordinate;
    public int fleetCount;
    // key: ����,
    // value: [0] = �Լ� �׷��̵�,
    //        [1] = �ش� �׷��̵忡���� �Լ� ����
    //        [2] = �Լ��� �� 
    public FleetTypeAndCount fleetTypeAndCount;
    public float fleetSpeed;
    public float distance;
}

[System.Serializable]
public class Missions
{
    public int neutral_Mission; // �߸��̼�
    public int friendly_Mission; // ��ȣ�̼�
    public int hostile_Mission; // ����̼�
}