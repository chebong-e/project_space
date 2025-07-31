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
        mining_Resource = new Dictionary<int, int>(); //임시로 채광슬롯 2개로 지정
    }

    public void Add_Event(Event_Triggered event_, int index)
    {
        eventCount++;
        dropDown.triggered_event = eventCount;


        // 0: 중립 미션
        // 1: 우호 미션
        // 2: 적대 미션
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

        // 순차정렬 확인
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
            Debug.Log($"총 {miningSlot.Count}채굴함대 중 {index}번호의 함대 귀환시작");
            StopCoroutine(miningSlot[index]);
            miningSlot.Remove(index);
        }

        Debug.Log("귀환 합니다!");
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
        // 이벤트 슬롯 슬라이드 처리 해야함.
        dropDown.SlideEventImplement(3);
    }

    IEnumerator MobilizeAFleet(EventLine eventLine, int index)
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
                MissionToAttack(index);
                break;
            case Event_Triggered.Event_Type.Nuetral_Missions:
                // 중립 미션에서 단순 이동을 위함인지 채굴인지 구분해야함.
                // 채굴 미션이라면

                yield return miningSlot[index] = StartCoroutine(MissionToMining(eventLine, index));
                Debug.Log("채굴완료. 귀환!");
                break;
            case Event_Triggered.Event_Type.UnionSupport:
                MissionToUnionSupport();
                break;
        }
 

    }

    // 조우한 적대 함선과 공수 로직 후 귀환
    void MissionToAttack(int index)
    {
        Debug.Log("전투개시!");
        // 총 4라운드까지 전투
        // 전투로직 작성해야함


        //전투 종료 후 귀환로직
        FleetReturnToBase(index);
    }

    IEnumerator MissionToMining(EventLine eventLine, int index)
    {
        eventLine.timerText.text = " - ";


        int cago = 0; // 임시로 카고 지정
        float timer = 0;

        // 해당 지점의 자원량 확인 로직
        // 자원확인로직();
        // 우선 로직 작성전 예시로 1000의 자원이 있다는 가정
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
                Debug.Log($"초당 광물을 {miningVolumePerSecond} 채굴중입니다.");
                Debug.Log($"총 광물 {mineVolume} 남아 있습니다.");
                Debug.Log($"남은 시간:{perSec}");
                mining_Resource[index] = cago;
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
    public Event_Triggered.NuetralCategory nuetralCategory;
    public int miningNumber;
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

[System.Serializable]
public class Missions
{
    public int neutral_Mission; // 중립미션
    public int friendly_Mission; // 우호미션
    public int hostile_Mission; // 적대미션
}