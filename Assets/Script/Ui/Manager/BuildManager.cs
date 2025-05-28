using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;
    public ControlCenter controlCenter;
    Coroutine coroutine;

    public bool upgraing;
    public ShipGrade_Container shipGrade_Container;


    public GameObject[] controlCenter_contents;
    public List<ImageSliderGroup> controlCenter_ImageSliderGroup;




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

        //5.27
        controlCenter_ImageSliderGroup = new List<ImageSliderGroup>();
        for (int i = 0; i < controlCenter_contents.Length; i++)
        {
            ImageSliderGroup group = new ImageSliderGroup();
            controlCenter_ImageSliderGroup.Add(group);

            for (int sec_i = 0; sec_i < controlCenter_contents[i].transform.childCount; sec_i++)
            {
                controlCenter_ImageSliderGroup[i].imageSlide.Add(controlCenter_contents[i].transform.GetChild(sec_i).GetComponent<ImageSlide>());
            }
        }
    }

    // 이미지슬라이드의 AllWindow_Active_IndexCheck로직과 중복, 통일 필요
    public int AllWindow_Active_IndexCheck()
    {
        int index = 0;
        for (int i = 0; i < EventManager.instance.imageSliderGroup.Count; i++)
        {
            List<ImageSlide> img_s = EventManager.instance.imageSliderGroup[i].imageSlide;
            if (img_s[0].gameObject.activeInHierarchy)
            {
                index = i;
                break;
            }
        }
        return index;
    }

    // 현재는 관제센터의 값만 가져와서 표시되도록 되어 있음
    // 각 컨테이너의 업그레이드 유무를 확인하여 업그레이드 중이라면 각 업그레이드 정보를 가져와 표시하는 로직 필요
    public void ExClose() 
    {
        int num = AllWindow_Active_IndexCheck();
        List<ImageSlide> img_s = EventManager.instance.imageSliderGroup[num].imageSlide;

        foreach (ImageSlide img in img_s)
        {
            if (img.open)
            {
                img.open = false;
                img.SliderOn_Off();
                
            }
        }
    }

    // image, slider, Infomation, ImageSlide, bool(업그레이드 진행 및 중지 유무 확인차)
    public void ControlCenter_Upgrade(Sprite img, Infomations info, ImageSlide imgSlide, bool upgrade)
    {
        if (upgrade)
        {
            float targetTimer = info.buildResource.building_Time[info.buildResource.level];
            coroutine = StartCoroutine(ControlCenter_BuildingTimer(img, info, imgSlide, targetTimer));
        }
        else
        {
            StopCoroutine(coroutine);
            for (int i = 0; i < 2; i++)
            {
                info.timeSlider[i].value = 0f;
                info.timeText[i].text = $"{info.buildResource.building_Time[info.buildResource.level]}초";
            }
            controlCenter.Upgrading(null, false, null);
        }
        //업그레이드 중인 버튼 외 흑백처리
        imgSlide.ImageChange_toUpgrade();
    }


    IEnumerator ControlCenter_BuildingTimer(Sprite img, Infomations info, ImageSlide imgSlide, float targetTimer)
    {
        GameObject con = controlCenter.Upgrading(img, true, null);
        Slider slder = con.GetComponentInChildren<Slider>();
        TextMeshProUGUI[] ExText = con.GetComponentsInChildren<TextMeshProUGUI>();
        ExText[0].text = info.titles["name"].text;


        slder.maxValue = targetTimer;


        info.timeSlider[0].maxValue = targetTimer;
        info.timeSlider[1].maxValue = targetTimer;
        float buildTimer = 0f;

        while (buildTimer < targetTimer)
        {
            buildTimer += Time.deltaTime;
            float remainingTime = Mathf.Clamp(targetTimer - buildTimer, 0f, targetTimer);
            info.timeSlider[1].value = buildTimer;
            info.timeSlider[0].value = buildTimer; // 중복이 필요할까... 나중에 보고 지워버리던지.....
            slder.value = buildTimer;

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

            foreach (TextMeshProUGUI tt in info.timeText)
            {
                tt.text = timeStr;
                ExText[1].text = timeStr;
            }

            yield return null;

        }
        // 업그레이드 완료시 호출할 목록
        // 취소버튼 비활성화 및 업그레이드 버튼 활성화
        // 해당 버튼 외 버튼 컬러처리 및 모두활성화
        // 다음 레벨 필요 자원 표시
        info.Upgrade();
        imgSlide.ImageChange_toUpgrade();

        // 업그레이드 완료
        info.buildDetailMatter.confirm = false;
        info.timeSlider[0].value = 0f;
        info.timeSlider[1].value = 0f;
        info.timeSlider[0].gameObject.SetActive(false);
        info.btns[1].gameObject.SetActive(false);
        info.btns[0].gameObject.SetActive(true);
        controlCenter.Upgrading(null, false, null);
    }

}

[System.Serializable]
public class ImageSliderGroup
{
    public List<ImageSlide> imageSlide = new List<ImageSlide>();
}
