using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

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
        BuildManager.instance.ControlCenter_Upgrade(transform.GetChild(1).GetComponent<Image>().sprite, infos, imgSlide, confirm);
    }


    IEnumerator Timer()
    {
        Sprite sp = transform.GetChild(1).GetComponent<Image>().sprite;
        GameObject con = EventManager.instance.controlCenter.Upgrading(sp, true);
        Slider sl = con.GetComponentInChildren<Slider>();
        TextMeshProUGUI ExText = con.GetComponentInChildren<TextMeshProUGUI>();
        sl.maxValue = targetTimer;


        infos.slider[0].maxValue = targetTimer;
        infos.slider[1].maxValue = targetTimer;
        buildTimer = 0f;

        while (buildTimer < targetTimer)
        {
            buildTimer += Time.deltaTime;
            float remainingTime = Mathf.Clamp(targetTimer - buildTimer, 0f, targetTimer);
            infos.slider[1].value = buildTimer;
            infos.slider[0].value = buildTimer; // 중복이 필요할까... 나중에 보고 지워버리던지.....
            sl.value = buildTimer;

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
                ExText.text = timeStr;
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
