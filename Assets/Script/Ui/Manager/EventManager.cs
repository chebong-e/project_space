using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using TMPro;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;
    public int eventCount;
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
    public List<GameObject> Ex_To_events;

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

    public void Add_Event(Event_Triggered event_, int index)
    {
        eventCount++;
        dropDown.triggered_event = eventCount;

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

        // 순차정렬 확인
        EventSequence_Realignment();

        eventCoroutine[index] = StartCoroutine(MobilizeAFleet(e_Line));
    }

    public void FleetReturnToBase(int index)
    {
        StopCoroutine(eventCoroutine[index]);

        EventLine eLine = dropDown.dropDown_List[index].GetComponent<EventLine>();
        eLine.missionImg.sprite = imgs[4];
        eLine.transform.GetChild(2).gameObject.SetActive(false);

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
        eventLine.event_Triggered.isUsed = false;

        eventLine.gameObject.SetActive(false);
        // 이벤트 슬롯 슬라이드 처리 해야함.
        dropDown.SlideEventImplement(3);
    }

    IEnumerator MobilizeAFleet(EventLine eventLine)
    {
        Debug.Log("함대 출동");
        float timer = 0f;
        eventLine.event_Triggered.timer = 0;

        // 실험적인 목표거리 계산법
        // 거리 = 1000일때 속도가 1이면 1000초가 걸린다는 단순계산 적용
        // 즉 거리 / 속도
        // 연료 소모는 거리 1당
        // 연소엔진 1, 램제트 1.5, 핵 추진 2, 힉스입자 3   => 실험하기 위한 단순 수치
        // 속도는 위의 엔진 나열 순서대로 1, 2, 2.5, 3

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
                MissionToAttack();
                break;
            case Event_Triggered.Event_Type.Missions:
                /*miningSlot = StartCoroutine(MissionToMining(mission, timeText));*/
                yield return miningSlot[0] = StartCoroutine(MissionToMining(eventLine));
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

    IEnumerator MissionToMining(EventLine eventLine)
    {
        eventLine.timerText.text = " - ";

        float timer = 0;

        // 해당 지점의 자원량 확인 로직
        // 자원확인로직();
        // 우선 로직 작성전 예시로 1000의 자원이 있다는 가정
        float mineVolume = 1000;
        float perSec = mineVolume / Build_Manager.instance.scriptable_Group.shipGroups[0].ships[eventLine.event_Triggered.mission.fleetTypeAndCount[0][1]].miningRate;
        while (timer < perSec)
        {
            timer += Time.deltaTime;

            if (timer > 1f)
            {
                timer = 0f;
                perSec -= 1f;
                mineVolume -= Build_Manager.instance.scriptable_Group.shipGroups[0].ships[eventLine.event_Triggered.mission.fleetTypeAndCount[0][1]].miningRate;
                Debug.Log($"초당 광물을 {Build_Manager.instance.scriptable_Group.shipGroups[0].ships[eventLine.event_Triggered.mission.fleetTypeAndCount[0][1]].miningRate} 채굴중입니다.");
                Debug.Log($"총 광물 {mineVolume} 남아 있습니다.");
                Debug.Log($"남은 시간:{perSec}");
            }

            
            yield return null;
        }

    }

    void MissionToUnionSupport()
    {

    }



    public void EventSequence_Realignment() // 이벤트 발생과 그에 따른 재정렬
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
    public string coordinate;
    public int fleetCount;
    // key: 순번,
    // value: [0] = 함선 그레이드,
    //        [1] = 해당 그레이드에서의 함선 순번
    //        [2] = 함선의 수 
    public FleetTypeAndCount fleetTypeAndCount;
    public float fleetSpeed;
    public float distance;
}