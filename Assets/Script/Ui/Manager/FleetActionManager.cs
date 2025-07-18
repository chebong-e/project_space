using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleetActionManager : MonoBehaviour
{
    public float miningspeed;
    public Ship[] ships;

    public void Ex_FleetMission()
    {
        Mission_Infomation mission_info = new Mission_Infomation();

        mission_info.event_Type = Event_Triggered.Event_Type.Missions;
        mission_info.coordinate = "콜로니 (999 , 999)";
        mission_info.fleetCount = 10;
        mission_info.fleetTypeToCount = new Dictionary<int, int[]>(); 
        mission_info.fleetTypeToCount[0] = new int[2] { 4, 10 };
        mission_info.fleetSpeed = 2;
        mission_info.distance = 30;
        EventManager.instance.Add_Event(mission_info);
    }
    public void MiningFleetAction()
    {
        float spd = 1f;

        int index = 0;
        StartCoroutine(FleetMobilized(10, spd, 10f));
        for (int i = 0; i < EventManager.instance.dropDown.dropDown_List.Length; i++)
        {
            if (EventManager.instance.dropDown.dropDown_List[i].GetComponent<EventLine>().event_Triggered.isUsed)
                continue;

            index = i;
            Debug.Log($"비어있는 슬롯: {index}");

            break;
        }

        DropDown drop = EventManager.instance.dropDown;
        drop.dropDown_List[index].GetComponent<EventLine>().event_Triggered.isUsed = true;

        // 이벤트 생성 또는 추가
        EventManager.instance.Add_Event();
        // 이벤트 슬롯에 좌표 및 타이머 정보 전달하기
        drop.dropDown_List[index].GetComponent<EventLine>().targetText.text = $"적대행성 (123,456)";
        /*drop.dropDown_List[index].GetComponent<EventLine>().timer = 10 / spd;*/

        // 이벤트 슬롯 활성화
        if (!drop.dropBtns[0].gameObject.activeSelf)
        {
            drop.Add_LineDropDown(index, 10 / spd);
        }
        else
        {
            drop.EventWindowActivate();
        }

    }

    // 함대수량, 함대속도(함대의 최저 이동속도로 계산), 목표 거리
    IEnumerator FleetMobilized(int fleetcount, float speed, float distance)
    {
        Debug.Log("함대 출동");
        float timer = 0;

        // 실험적인 목표거리 계산법
        // 거리 = 1000일때 속도가 1이면 1000초가 걸린다는 단순계산 적용
        // 즉 거리 / 속도
        // 연료 소모는 거리 1당
        // 연소엔진 1, 램제트 1.5, 핵 추진 2, 힉스입자 3   => 실험하기 위한 단순 수치
        // 속도는 위의 엔진 나열 순서대로 1, 2, 2.5, 3

        float distancePerSecond = distance / speed;
        while (timer < distancePerSecond)
        {
            timer += Time.deltaTime;

            if (timer >= 1f)
            {
                timer -= 1f;
                distancePerSecond -= 1f;
                Debug.Log("함대 출격중...");
            }
           

            yield return null;
        }

        timer = 0;
        // 채굴시간 계산은 매장량 / 채굴량 (1000 / 5 = 200초)

        float miningTime = 1000 / miningspeed;
        int deposit = 1000;
        Debug.Log("함대 지정 장소 도착! 임무를 수행합니다.");
        float totalMining = 0f;
        while (timer < deposit / miningspeed)
        {
            timer += Time.deltaTime;

            if (timer >= 1f)
            {
                timer -= 1f;
                deposit -= Mathf.RoundToInt(miningspeed);
                totalMining += miningspeed;
                Debug.Log($"초당 광물을 {miningspeed} 채굴중입니다.");
                Debug.Log($"총 광물 {totalMining} 채굴 하였습니다..");
            }

            yield return null;
        }

        Debug.Log("채굴완료! 귀환합니다");


    }
}
