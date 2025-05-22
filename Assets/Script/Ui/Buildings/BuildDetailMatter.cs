using UnityEngine;
using System.Collections;
using TMPro;

[System.Serializable]
public class values : SerializableDictionary<string, GameObject> { };
public class BuildDetailMatter : MonoBehaviour
{
    public Infomations infos;
    ImageSlide imgSlide;
    public float buildTimer;
    public float targetTimer;
    public bool confirm;

    Coroutine coroutine;

    void Awake()
    {
        infos = transform.GetChild(2).GetComponent<Infomations>();
        imgSlide = GetComponent<ImageSlide>();
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
            StopCoroutine(coroutine);

            for (int i = 0; i < 2; i++)
            {
                infos.slider[i].value = 0f;
                infos.timeText[i].text = $"{infos.buildResource.building_Time[infos.buildResource.level]}초";
            }
            
            buildTimer = 0f;
        }
        //이미지 흑백 처리 및 해당 버튼 외의 다른 버튼 비활성화 처리
        imgSlide.ImageChange_toUpgrade();

    }


    IEnumerator Timer()
    {
        infos.slider[0].maxValue = targetTimer;
        infos.slider[1].maxValue = targetTimer;
        buildTimer = 0f;

        while (buildTimer < targetTimer)
        {
            buildTimer += Time.deltaTime;
            float remainingTime = Mathf.Clamp(targetTimer - buildTimer, 0f, targetTimer);
            infos.slider[1].value = buildTimer;
            infos.slider[0].value = buildTimer; // 중복이 필요할까... 나중에 보고 지워버리던지.....


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

            foreach (TextMeshProUGUI tt in infos.timeText)
            {
                tt.text = timeStr;
            }

            yield return null;

        }
        // 업그레이드 완료시 호출할 목록
        // 취소버튼 비활성화 및 업그레이드 버튼 활성화
        // 해당 버튼 외 버튼 컬러처리 및 모두활성화
        // 다음 레벨 필요 자원 표시
        infos.Upgrade();
        imgSlide.ImageChange_toUpgrade();
        // 업그레이드 완료
        confirm = false;
        infos.slider[0].value = 0f;
        infos.slider[1].value = 0f;
        infos.slider[0].gameObject.SetActive(false);
        infos.btns[1].gameObject.SetActive(false);
        infos.btns[0].gameObject.SetActive(true);
    }



}
