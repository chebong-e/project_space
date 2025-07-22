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

    //확인용
    public GameObject[] Ex_contentsCheck;
    public GameObject canvas;


    public GameObject[] TabContainer;


    public float metal_RefillTimer;
    public float cristal_RefillTimer;
    public float gas_RefillTimer;
    public float total_Timer;

    public Sprite[] imgs;

    public Event_Triggered[] events;

    Coroutine[] eventCoroutine;
    Coroutine[] miningSlot;


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
        miningSlot = new Coroutine[1];
    }

    public void Add_Event()
    {
        eventTriggered++;

        dropDown.triggered_event = eventTriggered;
    }

    public void Add_Event(Event_Triggered event_, int index)
    {
        eventTriggered++;
        dropDown.triggered_event = eventTriggered;

        /*int index = 0;
        for (int i = 0; i < dropDown.dropDown_List.Length; i++)
        {
            if (dropDown.dropDown_List[i].GetComponent<EventLine>().event_Triggered.isUsed)
            {
                continue;
            }

            index = i;
            Debug.Log($"비어있는 슬롯: {index}");
            break;
        }*/

        EventLine e_Line = dropDown.dropDown_List[index].GetComponent<EventLine>();
        e_Line.event_Triggered.isUsed = true;
        e_Line.missionImg.sprite = imgs[(int)event_.mission.event_Type];
        e_Line.targetText.text = $"{event_.mission.coordinate}";
        
        if (!dropDown.dropBtns[0].gameObject.activeSelf)
        {
            dropDown.Add_LineDropDown(index, event_.mission.distance / event_.mission.fleetSpeed);
        }
        else
        {
            dropDown.EventWindowActivate();
        }


        eventCoroutine[index] = StartCoroutine(MobilizeAFleet(event_, e_Line.timerText));
        /*StartCoroutine(MobilizeAFleet(mission, e_Line.timerText));*/
    }

    public void FleetReturnToBase(int index)
    {
        StopCoroutine(eventCoroutine[index]);


        EventLine eLine = dropDown.dropDown_List[index].GetComponentInParent<EventLine>();

        Debug.Log($"함대 귀환합니다.");
        StartCoroutine(ReturnToBase(eLine.event_Triggered, eLine.timerText));
    }
    
    IEnumerator ReturnToBase(Event_Triggered event_, TextMeshProUGUI timeText)
    {
        /*float sec = event_.mission.distance / event_.mission.fleetSpeed;*/
        float timer = 0f;
        float sec = event_.timer;
        while (sec > 0)
        {
            timer = Time.deltaTime;
            sec -= timer;
            TimeSpan timespan = TimeSpan.FromSeconds(sec);
            timeText.text = timespan.ToString(@"hh\:mm\:ss");

            yield return null;
        }

        Debug.Log($"함대 귀환 완료!");
    }

    IEnumerator MobilizeAFleet(Event_Triggered event_, TextMeshProUGUI timeText)
    {
        Debug.Log("함대 출동");
        float timer = 0f;
        event_.timer = 0;

        // 실험적인 목표거리 계산법
        // 거리 = 1000일때 속도가 1이면 1000초가 걸린다는 단순계산 적용
        // 즉 거리 / 속도
        // 연료 소모는 거리 1당
        // 연소엔진 1, 램제트 1.5, 핵 추진 2, 힉스입자 3   => 실험하기 위한 단순 수치
        // 속도는 위의 엔진 나열 순서대로 1, 2, 2.5, 3

        float distancePerSecond = event_.mission.distance / event_.mission.fleetSpeed;

        while (distancePerSecond > 0)
        {
            timer = Time.deltaTime;
            distancePerSecond -= timer;
            event_.timer += timer;
            TimeSpan timespan = TimeSpan.FromSeconds(distancePerSecond);
            timeText.text = timespan.ToString(@"hh\:mm\:ss");

            yield return null;
        }

        switch (event_.mission.event_Type)
        {
            case Event_Triggered.Event_Type.Attack:
                MissionToAttack();
                break;
            case Event_Triggered.Event_Type.Missions:
                /*miningSlot = StartCoroutine(MissionToMining(mission, timeText));*/
                yield return miningSlot[0] = StartCoroutine(MissionToMining(event_.mission, timeText));
                Debug.Log("채굴완료. 귀환!");
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

        // 해당 지점의 자원량 확인 로직
        // 자원확인로직();
        // 우선 로직 작성전 예시로 1000의 자원이 있다는 가정
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
                Debug.Log($"초당 광물을 {Build_Manager.instance.scriptable_Group.shipGroups[0].ships[mission.fleetTypeToCount[0][0]].miningRate} 채굴중입니다.");
                Debug.Log($"총 광물 {mineVolume} 남아 있습니다.");
                Debug.Log($"남은 시간:{perSec}");
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
    // key: 함선그레이드, value: [0]인덱스 = 해당그레이드의 순번, [1]인덱스 = 해당 함선의 수 
    public Dictionary<int, int[]> fleetTypeToCount;
    public float fleetSpeed;
    public float distance;
}