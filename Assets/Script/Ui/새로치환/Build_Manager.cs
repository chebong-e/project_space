using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Build_Manager : MonoBehaviour
{
    public static Build_Manager instance;
    public ContainerSlide_Group containerSlide_Group;
    public MainTabCategory mainTabCategory;
    public GameObject[] tabContainer, mainTab_Container;

    Coroutine controllCenter_coroutine, makingShip_coroutine;
    Scriptable_Group scriptable_Group;

    public bool upgrading, makingShips, building, reserching;
    bool datas;

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

        // 컨테이너슬라이드를 메인탭 별로 분류하여 변수에 할당
        for (int i = 0; i < tabContainer.Length; i++)
        {
            List<ContainerSlide> targetContainer = GetTargetListByIndex(i);

            if (targetContainer == null) continue;

            ScrollRect[] scrollRects = tabContainer[i].GetComponentsInChildren<ScrollRect>(true);
            foreach (ScrollRect scroll in scrollRects)
            {
                for (int j = 0; j < scroll.content.childCount; j++)
                {
                    ContainerSlide cont = scroll.content.GetChild(j).GetComponent<ContainerSlide>();
                    if (cont != null)
                        targetContainer.Add(cont);
                }
            }
        }

        scriptable_Group = GetComponent<Scriptable_Group>();


        foreach (GameObject obj in tabContainer)
        {
            obj.SetActive(true);
        }

        // 탭 별로 스크립터블매칭의 초기화를 한뒤 첫번째 화면만 놔두고 다른 탭 비활성화
        for (int i = 3; i < tabContainer.Length; i++)
        {
            ShipGrade_Container shipGrade_Container = tabContainer[i].GetComponentInChildren<ShipGrade_Container>();

            for (int j = 0; j < shipGrade_Container.shipGrade_Window.Length; j++)
            {
                shipGrade_Container.shipGrade_Window[j].transform
                    .GetComponentInChildren<Scriptable_Matching>().Init();
                if (j > 0)
                {
                    shipGrade_Container.shipGrade_Window[j].gameObject.SetActive(false);
                }
            }
        }

        /*Data_CheckInit(datas);*/
        foreach (GameObject obj in tabContainer)
        {
            obj.SetActive(false);
        }

    }

    // 서버에서 계정의 데이터 확인하여 반영해주기 위한 초기 로직(임시)
    /*void Data_CheckInit(bool data)
    {
        if (!data)
        {
            for (int i = 3; i < tabContainer.Length; i++)
            {
                Con_Infomation[] con_ = tabContainer[i].GetComponentsInChildren<Con_Infomation>(true);
                for (int j = 0; j < con_.Length; j++)
                {
                    con_[j]
                }
            }
        }


    }*/


    public void KeepOpenState(int index)
    {
        List<ContainerSlide> slide = GetTargetListByIndex(index);
        foreach (ContainerSlide container in slide)
        {
            if (container.imgOpen)
            {
                foreach (Animator anim in container.anims)
                {
                    anim.SetTrigger("OpenDefault");
                }
            }
        }
    }

    public List<ContainerSlide> GetTargetListByIndex(int index)
    {
        switch (index)
        {
            case 0: return containerSlide_Group.build_Tab1;
            case 1: return containerSlide_Group.build_Tab2;
            case 2: return containerSlide_Group.build_Tab3; // 연구
            case 3: return containerSlide_Group.build_Tab4; // 함선생산
            case 4: return containerSlide_Group.controlCenter_Tab;
            default: return null;
        }
    }

    // 활성화 상태 추적을 위한 로직
    // 활성화 상태 추적하여 해당 카테고리별 이미지 전환 유도
    public int Active_TabContainerIndex()
    {
        int index = 0;
        for (int i = 0; i < tabContainer.Length; i++)
        {
            if (tabContainer[i].activeInHierarchy)
            {
                index = i;
                break;
            }
        }
        return index;
    }

    // 열려있는 탭을 확인하여 그 중 선택되어 슬라이드 되어있는 컨테이너가 있다면
    // 컨테이너를 슬라이드 닫기처리
    public void TabWindow_Close()
    {
        Debug.Log(Active_TabContainerIndex());
        if (3 == Active_TabContainerIndex())
        {
            
            return;
        }


        foreach (ContainerSlide img in GetTargetListByIndex(Active_TabContainerIndex()))
        {
            if (img.imgOpen)
            {
                img.imgOpen = false;
                img.Slide_Close();
                img.Slider_On_Off();
            }
        }
    }

    public void BuildTab4_MakingShips(Sprite img, int makeCount, Con_Infomation info, bool upgrade)
    {
        mainTabCategory = mainTab_Container[Active_TabContainerIndex()].GetComponent<MainTabCategory>();

        if (upgrade)
        {
            info.containerSlide.imgBtn.enabled = false;
            makingShip_coroutine = StartCoroutine(
                MakingShips_Timer(
                    img,
                    info,
                    makeCount));
        }
        else
        {
            info.containerSlide.imgBtn.enabled = true;
            StopCoroutine(makingShip_coroutine);
            for (int i = 0; i < 2; i++)
            {
                info.timeSlider[i].value = 0f;
                info.timeText[i].text = $"{info.buildResource.building_Time[info.buildResource.level]}초";
            }
            mainTabCategory.Upgrading(null, false);
        }

        info.containerSlide.ColorChange_To_Upgrade(Active_TabContainerIndex());
        info.child_InfoContainer.transform.GetChild(0).gameObject.SetActive(upgrade);
        info.child_InfoContainer.transform.GetChild(1).gameObject.SetActive(!upgrade);
    }

    //함선생산과 관제센터 업그레이드 함수 중복 정리 하여 간결하게 하기 위한 코드 정리 실험
    public void Example_Confirm(Sprite img, int makeCount, Con_Infomation info, bool upgrade)
    {
        mainTabCategory = mainTab_Container[Active_TabContainerIndex()].GetComponent<MainTabCategory>();
        if (upgrade)
        {
            switch (info.info_types)
            {
                case Con_Infomation.Types.Tab4:
                    info.containerSlide.imgBtn.enabled = false;
                    makingShip_coroutine = StartCoroutine(
                        MakingShips_Timer(
                            img,
                            info,
                            makeCount));
                    break;
                case Con_Infomation.Types.ControlCenter:
                    controllCenter_coroutine = StartCoroutine(
                        ControlCenter_BuildingTimer(
                            img,
                            info,
                            info.buildResource.building_Time[info.buildResource.level]));
                    break;
            }
        }
        else
        {
            switch (info.info_types)
            {
                case Con_Infomation.Types.Tab4:
                    info.containerSlide.imgBtn.enabled = true;
                    StopCoroutine(makingShip_coroutine);
                    info.child_InfoContainer.transform.GetChild(0).gameObject.SetActive(upgrade);
                    info.child_InfoContainer.transform.GetChild(1).gameObject.SetActive(!upgrade);
                    break;
                case Con_Infomation.Types.ControlCenter:
                    StopCoroutine(controllCenter_coroutine);
                    mainTabCategory.Upgrading(null, false);
                    break;
            }
            for (int i = 0; i < 2; i++)
            {
                info.timeSlider[i].value = 0f;
                info.timeText[i].text = $"{info.buildResource.building_Time[info.buildResource.level]}초";
            }
            mainTabCategory.Upgrading(null, false);
        }
        info.containerSlide.ColorChange_To_Upgrade(Active_TabContainerIndex());
        if (info.info_types == Con_Infomation.Types.Tab4)
        {
            info.child_InfoContainer.transform.GetChild(0).gameObject.SetActive(upgrade);
            info.child_InfoContainer.transform.GetChild(1).gameObject.SetActive(!upgrade);
        }
    }


    public void ControlCenter_Upgrade(Sprite img, Con_Infomation info, bool upgrade)
    {
        mainTabCategory = mainTab_Container[Active_TabContainerIndex()].GetComponent<MainTabCategory>();

        if (upgrade)
        {
            float targetTimer = info.buildResource.building_Time[info.buildResource.level];
            controllCenter_coroutine = StartCoroutine(
                ControlCenter_BuildingTimer(
                    img, 
                    info, 
                    targetTimer));
        }
        else
        {
            StopCoroutine(controllCenter_coroutine);
            for (int i = 0; i < 2; i++)
            {
                info.timeSlider[i].value = 0f;
                info.timeText[i].text = $"{info.buildResource.building_Time[info.buildResource.level]}초";
            }
            mainTabCategory.Upgrading(null, false);
        }
        //업그레이드 중인 버튼 외 흑백처리
        info.containerSlide.ColorChange_To_Upgrade(Active_TabContainerIndex());
    }

    IEnumerator MakingShips_Timer(Sprite img, Con_Infomation info, int makeCount)
    {
        int activeIndex = Active_TabContainerIndex();
        GameObject maintab_container = mainTabCategory.Upgrading(img, true);
        Slider maintab_slider = mainTabCategory.sliderContainer.GetComponentInChildren<Slider>();
        TextMeshProUGUI[] maintab_texts = mainTabCategory.sliderContainer.GetComponentsInChildren<TextMeshProUGUI>();


        maintab_texts[0].text = info.title_Text["name"].text;

        float totalMakeTime = makeCount * info.ship.shipMaking_Time;
        maintab_slider.maxValue = totalMakeTime;

        foreach (Slider _slider in info.timeSlider)
        {
            _slider.maxValue = totalMakeTime;
        }

        info.child_InfoContainer.transform.GetChild(0).gameObject.SetActive(true);
        info.child_InfoContainer.transform.GetChild(1).gameObject.SetActive(false);

        float timer = 0f;

        while (timer < totalMakeTime)
        {
            timer += Time.deltaTime;
            foreach (Slider slide in info.timeSlider)
            {
                slide.value = timer;
            }
            maintab_slider.value = timer;

            int remaining_curTime = Mathf.CeilToInt(
                Mathf.Clamp(totalMakeTime - timer, 0f, totalMakeTime));

            foreach (TextMeshProUGUI tt in info.timeText)
            {
                tt.text = TimerTexting(remaining_curTime);
            }
            maintab_texts[1].text = TimerTexting(remaining_curTime);
            yield return null;
        }

        // 업그레이드 성공 후 반영될 정보들 가시화
        info.ship.currentHave_Ship += makeCount;
        info.Upgrade_To_Infomation(info);
        

        // 이미지 컬러전환 및 버튼 활성화 처리
        info.containerSlide.ColorChange_To_Upgrade(activeIndex);
        info.containerSlide.imgBtn.enabled = true;


        info.shipMaking_confirm = false;

        foreach (Slider slide in info.timeSlider)
        {
            slide.value = 0f;
        }


        info.child_InfoContainer.transform.GetChild(0).gameObject.SetActive(false);
        info.child_InfoContainer.transform.GetChild(1).gameObject.SetActive(true);

        info.btns[1].gameObject.SetActive(false);
        info.btns[0].gameObject.SetActive(true);

        // 메인탭의 이미지 기본사진으로 변경
        maintab_container.GetComponent<MainTabCategory>().Upgrading(null, false);
    }

    IEnumerator ControlCenter_BuildingTimer(Sprite img, Con_Infomation info, float targetTimer)
    {
        // 6.7일자까지 수정한 사항이고 이후부터 계속 수정작업 시행
        // Con_Infomation의 160번 줄 예외처리 다시 복귀하고 같이 수정진행
        int activeIndex = Active_TabContainerIndex();
        GameObject maintab_container = mainTabCategory.Upgrading(img, true);

        /*Slider maintab_slider = maintab_container.GetComponentInChildren<Slider>();*/
        Slider maintab_slider = mainTabCategory.sliderContainer.GetComponentInChildren<Slider>();
        /*TextMeshProUGUI[] maintab_texts = maintab_container.GetComponentsInChildren<TextMeshProUGUI>();*/
        TextMeshProUGUI[] maintab_texts = mainTabCategory.sliderContainer.GetComponentsInChildren<TextMeshProUGUI>();
        maintab_texts[0].text = info.title_Text["name"].text;

        maintab_slider.maxValue = targetTimer;

        foreach (Slider _slider in info.timeSlider)
        {
            _slider.maxValue = targetTimer;
        }
        float timer = 0f;

        while (timer < targetTimer)
        {
            timer += Time.deltaTime;
            foreach (Slider slide in info.timeSlider)
            {
                slide.value = timer;
            }
            maintab_slider.value = timer;

            int remaining_curTime = Mathf.CeilToInt(
                Mathf.Clamp(targetTimer - timer, 0f, targetTimer));

            foreach (TextMeshProUGUI tt in info.timeText)
            {
                tt.text = TimerTexting(remaining_curTime);
            }
            maintab_texts[1].text = TimerTexting(remaining_curTime);

            yield return null;
        }

        // 업그레이드 성공 후 반영될 정보들 가시화
        info.Upgrade_To_Infomation(info);

        // 이미지 컬러전환 및 버튼 활성화 처리
        info.containerSlide.ColorChange_To_Upgrade(activeIndex);

        info.controlCenter_confirm = false;
        foreach (Slider slide in info.timeSlider)
        {
            slide.value = 0f;
        }
        info.timeSlider[0].gameObject.SetActive(false);
        info.btns[1].gameObject.SetActive(false);
        info.btns[0].gameObject.SetActive(true);

        // 메인탭의 이미지 기본사진으로 변경
        maintab_container.GetComponent<MainTabCategory>().Upgrading(null, false);

        /*관제센터 업그레이드 후 생산가능 수량 업데이트 정보를
            함선생산 탭의 정보로 넘겨주기(현재는 infomation의 ships 정보를 수정하는 방향)*/
        info.ship.maxHaveShip_Amount = info.buildResource.AllowableBuild;

        // 항상 업그레이드 완료 또는 생산 완료 후에는 유저 정보를 업데이트 하여 취합하는 곳이 필요
        // (서버에 통신을 용이하게 하기 위함)
    }

    public string TimerTexting(int timer)
    {
        if (timer >= 3600)
            return $"{timer / 3600}시간 {(timer % 3600) / 60}분 {timer % 60}초";
        else if (timer >= 60)
            return $"{timer / 60}분 {timer % 60}초";
        else
            return $"{timer}초";
    }

    // init과 관련(모든 탭 윈도우 비활성화)
    public void Tab_Close()
    {
        foreach (GameObject tab in tabContainer)
        {
            tab.SetActive(false);
        }
    }
}

[System.Serializable]
public class ContainerSlide_Group
{
    public List<ContainerSlide> build_Tab1;
    public List<ContainerSlide> build_Tab2;
    public List<ContainerSlide> build_Tab3;
    public List<ContainerSlide> build_Tab4;
    public List<ContainerSlide> controlCenter_Tab;

}
