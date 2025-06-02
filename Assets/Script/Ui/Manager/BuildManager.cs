using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;
    public MainTabCategory mainTabCategory;
    Coroutine coroutine;

    public bool upgrading;
    public bool buildShips;
    public bool building;
    public bool reserching;
    public ShipGrade_Container shipGrade_Container;

    public GameObject canvas;
    public GameObject[] TabContainer;
    public ScrollRect[] contents;
    public List<ImageSliderGroup> imageSliderGroup;
    public Group_To_ImageSlide group_To_ImageSlide;


    // 이것도 6.2 확인용, 확인후 불필요 판단 되면 삭제처리
    Init_Set init_Set;




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

        init_Set = GetComponent<Init_Set>();

        //5.27
        imageSliderGroup = new List<ImageSliderGroup>();
        contents = canvas.GetComponentsInChildren<ScrollRect>(true);
        for (int i = 0; i < contents.Length; i++)
        {
            ImageSliderGroup group = new ImageSliderGroup();
            imageSliderGroup.Add(group);

            for (int sec_i = 0; sec_i < contents[i].content.transform.childCount; sec_i++)
            {
                imageSliderGroup[i].imageSlide.Add(contents[i].content.transform.GetChild(sec_i).GetComponent<ImageSlide>());
            }
        }

        for (int index = 0; index < TabContainer.Length; index++)
        {
            List<ImageSlide> targetList = GetTargetListByIndex(index);
            if (targetList == null) continue;

            ScrollRect[] scr = TabContainer[index].GetComponentsInChildren<ScrollRect>(true);
            foreach (ScrollRect scroll in scr)
            {
                for (int i = 0; i < scroll.content.childCount; i++)
                {
                    ImageSlide img = scroll.content.GetChild(i).GetComponent<ImageSlide>();
                    if (img != null)
                        targetList.Add(img);
                }
            }
        }
    }

    public List<ImageSlide> GetTargetListByIndex(int index)
    {
        switch (index)
        {
            case 0: return group_To_ImageSlide.build_Tab1;
            case 1: return group_To_ImageSlide.build_Tab2;
            case 2: return group_To_ImageSlide.build_Tab3;
            case 3: return group_To_ImageSlide.reserch_Tab;
            case 4: return group_To_ImageSlide.controlCenter_Tab;
            default: return null;
        }
    }



    // 이미지슬라이드의 AllWindow_Active_IndexCheck로직과 중복, 통일 필요
    public int AllWindow_Active_IndexCheck()
    {
        int index = 0;
        for (int i = 0; i < imageSliderGroup.Count; i++)
        {
            List<ImageSlide> img_s = imageSliderGroup[i].imageSlide;
            if (img_s[0] != null)
            {
                if (img_s[0].gameObject.activeInHierarchy)
                {
                    index = i;
                    break;
                }
            }
            
        }
        return index;
    }

    public int Active_TabContainer_Index()
    {
        int index = 0;
        for (int i = 0; i< TabContainer.Length; i++)
        {
            if (TabContainer[i].activeInHierarchy)
            {
                index = i;
                break;
            }
        }
        Debug.Log($"인덱스값:{index}, 활성상태의 오브젝트:{TabContainer[index].name}");
        return index;
    }

    // 현재는 관제센터의 값만 가져와서 표시되도록 되어 있음
    // 각 컨테이너의 업그레이드 유무를 확인하여 업그레이드 중이라면 각 업그레이드 정보를 가져와 표시하는 로직 필요
    public void ExClose() 
    {
        int num = AllWindow_Active_IndexCheck();
        List<ImageSlide> img_s = imageSliderGroup[num].imageSlide;

        foreach (ImageSlide img in img_s)
        {
            if (img.open)
            {
                img.open = false;
                img.SliderOn_Off();
                
            }
        }
    }

    // 6.2 작성중
    public void BuildTab3_BuildShips(Sprite img, float totalTime, Infomations info, ImageSlide imgSlide, bool upgrade)
    {
        int index = 0;
        for (int i = 0; i < init_Set.category_Windows.Length; i++)
        {
            if (init_Set.category_Windows[i].activeInHierarchy)
            {
                index = i;
                break;
            }
        }
        mainTabCategory = init_Set.mainTab_Container[index].GetComponent<MainTabCategory>();


        //업그레이드 중인 버튼 외 흑백처리*/
        imgSlide.ImageChange_toUpgrade(Active_TabContainer_Index());  
    }



    // image, slider, Infomation, ImageSlide, bool(업그레이드 진행 및 중지 유무 확인차)
    public void ControlCenter_Upgrade(Sprite img, Infomations info, ImageSlide imgSlide, bool upgrade)
    {
        int index = 0;
        for (int i = 0; i < init_Set.category_Windows.Length; i++)
        {
            if (init_Set.category_Windows[i].activeInHierarchy)
            {
                index = i;
                break;
            }
        }
        mainTabCategory = init_Set.mainTab_Container[index].GetComponent<MainTabCategory>();




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
            mainTabCategory.Upgrading(null, false);
        }
        //업그레이드 중인 버튼 외 흑백처리
        imgSlide.ImageChange_toUpgrade(Active_TabContainer_Index());
    }


    IEnumerator ControlCenter_BuildingTimer(Sprite img, Infomations info, ImageSlide imgSlide, float targetTimer)
    {
        int index = Active_TabContainer_Index();
        GameObject con = mainTabCategory.Upgrading(img, true);
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
        info.Upgrade_to_Infomation();
        imgSlide.ImageChange_toUpgrade(index);

        // 업그레이드 완료
        info.confirm = false;
        info.timeSlider[0].value = 0f;
        info.timeSlider[1].value = 0f;
        info.timeSlider[0].gameObject.SetActive(false);
        info.btns[1].gameObject.SetActive(false);
        info.btns[0].gameObject.SetActive(true);

        mainTabCategory.Upgrading(null, false);



        //////////////////////////////////////// 부모의 1번째 자식 오브젝트 찾는방법
        Transform current = info.transform; ;
        Transform root = info.transform.root;

        while (current.parent != null && current.parent != root)
        {
            current = current.parent;
        }
        Debug.Log($"네임: {current.name}");

        

        // 함선생산 탭의 해당 함선에도 업그레이드 정보 표시해주어야 함.
        // 아래는 우선 예시
        /*for (int i = 0; i < EventManager.instance.containers.Count; i++)
        {
            EventManager.instance.containers[]
        }*/

        info.shipBuildSlider.ControlCenterUpgrade_for_Ship(info.buildResource.AllowableBuild);
        // 0 을 넘겨주는것을 수정해야함


    }

}

[System.Serializable]
public class ImageSliderGroup
{
    public List<ImageSlide> imageSlide = new List<ImageSlide>();
}

[System.Serializable]
public class Group_To_ImageSlide
{
    public List<ImageSlide> build_Tab1;
    public List<ImageSlide> build_Tab2;
    public List<ImageSlide> build_Tab3;
    public List<ImageSlide> reserch_Tab;
    public List<ImageSlide> controlCenter_Tab;

}
