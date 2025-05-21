using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using static UnityEditor.PlayerSettings;

[System.Serializable]
public class values : SerializableDictionary<string, GameObject> { };
public class BuildDetailMatter : MonoBehaviour
{
    public Infomations infos;
    public float buildTimer;
    public bool ccBuild;
    public float targetTimer;
    bool confirm;

    Coroutine coroutine;

    void Awake()
    {
        infos = transform.GetChild(2).GetComponent<Infomations>();
    }

    IEnumerator Timer()
    {
        infos.slider.maxValue = targetTimer;
        buildTimer = 0f;

        while (buildTimer < targetTimer)
        {
            buildTimer += Time.deltaTime;
            float remainingTime = Mathf.Clamp(targetTimer - buildTimer, 0f, targetTimer);
            infos.slider.value = buildTimer;

            int curTime = Mathf.CeilToInt(remainingTime);

            string timeStr = "";

            if (curTime >= 3600)
            {
                int hours = curTime / 3600;
                int minutes = (curTime % 3600) / 60;
                int seconds = curTime % 60;
                timeStr = string.Format("{0}시간 {1}분 {2}초", hours, minutes, seconds);
            }
            else if (curTime >= 60)
            {
                int minutes = curTime / 60;
                int seconds = curTime % 60;
                timeStr = string.Format("{0}분 {1}초", minutes, seconds);
            }
            else
            {
                timeStr = string.Format("{0}초", curTime);
            }

            infos.timeText.text = timeStr;

            yield return null;

        }
        // 업그레이드 완료시 호출할 목록
        // 취소버튼 비활성화 및 업그레이드 버튼 활성화
        // 해당 버튼 외 버튼 컬러처리 및 모두활성화
        // 다음 레벨 필요 자원 표시
        infos.Upgrade();

        Upgrade();
        infos.btns[1].gameObject.SetActive(false);
        infos.btns[0].gameObject.SetActive(true);
    }


    public void Upgrade()
    {
        confirm = !confirm;
        if (confirm) // 업그레이드 시작
        {
            targetTimer = infos.buildResource.building_Time[infos.buildResource.level];


            if (infos.unLock)
            {

            }
            
            //이미지 흑백 처리 및 해당 버튼 외의 다른 버튼 비활성화 처리
            GetComponent<ImageSlide>().ImageChange_toUpgrade();

            // 슬라이드 시간표시 처리
            coroutine = StartCoroutine(Timer());

            // 업그레이드 정보 표시
            // 이것은 업그레이드 완료시 처리되어야 함.
        }
        else // 업그레이드 중지
        {
            if (infos.unLock)
            {
                
            }
            GetComponent<ImageSlide>().ImageChange_toUpgrade();
            StopCoroutine(coroutine);
            infos.slider.value = 0f;
            infos.timeText.text = $"{infos.buildResource.building_Time[infos.buildResource.level]}초";
            buildTimer = 0f;
        }
        
    }





}
