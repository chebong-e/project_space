using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Build_Manager : MonoBehaviour
{
    public static Build_Manager instance;
    
    
    public PlayerInfomation playerInfomation;


    public ContainerSlide_Group containerSlide_Group;
    public MainTabCategory mainTabCategory;
    public GameObject[] tabContainer, mainTab_Container;

    Coroutine tab1_coroutine, tab2_coroutine, tab3_coroutine, tab4_coroutine, tab5_coroutine;
    Scriptable_Group scriptable_Group;

    public bool upgrading, makingShips, building, reserching;
    bool tab1, tab2, tab3, tab4, tab5;

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
        for (int i = 0; i < tabContainer.Length; i++)
        {
            if (i >= 3)
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

            else
            {
                tabContainer[i].GetComponentInChildren<Scriptable_Matching>()?.Init();
            } 
        }

        foreach (GameObject obj in tabContainer)
        {
            obj.SetActive(false);
        }


        /*// 수치를 얻기위한 실험
        float value = 100;
        for (int i = 0; i < 10; i++)
        {
            value *= 1.6f;
            Debug.Log($"기본값:{value},  정수형:{(int)value},  절삭값:{((int)value / 10) * 10}");
        }*/

        /*// 플레이어 인포 할당 실험 (06-30)
        playerInfomation.build_Levels[0] = new int[5, 16];

        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 16; j++)
            {
                playerInfomation.build_Levels[0][i, j] = j + 1;
            }
        }*/

        //for (int i = 0; i < 2; i++)
        //{
        //    for (int j = 0; j < 16; j++)
        //    {
        //        Debug.Log($"홈플래닛의 건설레벨들은 = tab{i + 1} : {j}\n" +
        //            $"{playerInfomation.build_Levels[0][i, j]}");

        //    }
        //}



        /*playerInfomation.planets = new BuildLevels[2]; // 가정(홈플래닛과 콜로니1)
        for (int i = 0; i < playerInfomation.planets.Length; i++)
        {
            playerInfomation.planets[i].tabs = new TabWindows[5];
            
            for (int j = 0; j < playerInfomation.planets[i].tabs.Length; j++)
            {
                int num = scriptable_Group.GetTargetListByBuildResource(j, 0).Count;
                playerInfomation.planets[i].tabs[j].lv = new int[num];
                for (int ii = 0; ii < num; ii++)
                {
                    playerInfomation.planets[i].tabs[j].lv[ii] =
                        scriptable_Group.GetTargetListByBuildResource(j, 0)[ii].level;
                }
            }
        }*/
        


        
            
    }

    // 함선과 관제센터는 그레이드가 5로 더 세분화 되므로 이것의 값을 세분화하여 저장하는게 필요
    public void btnExex()
    {
        playerInfomation.planets = new BuildLevels[2]; // 가정(홈플래닛과 콜로니1)
        for (int i = 0; i < playerInfomation.planets.Length; i++) // 2
        {
            playerInfomation.planets[i] = new BuildLevels();
            playerInfomation.planets[i].tabs = new TabWindows[5];

            for (int j = 0; j < playerInfomation.planets[i].tabs.Length; j++) // 5
            {
                playerInfomation.planets[i].tabs[j] = new TabWindows();
                int num = scriptable_Group.GetTargetListByBuildResource(j, 0).Count; // 0일때 8
                playerInfomation.planets[i].tabs[j].lv = new int[num];

                if (j == 0 || j == 1)
                {
                    for (int ii = 0; ii < num; ii++)
                    {
                        playerInfomation.planets[i].tabs[j].lv[ii] =
                            scriptable_Group.GetTargetListByBuildResource(j, 0)[ii].level;
                    }
                }
                else if (j == 2)
                {
                    playerInfomation.planets[i].tabs[j].gradeLv = new Int_Grade[3];
                    for (int aa = 0; aa < 3; aa++)
                    {
                        num = scriptable_Group.GetTargetListByResearch(aa).Count;
                        Debug.Log($"tab{j + 1}:{aa}의 길이는 {num}");

                        playerInfomation.planets[i].tabs[j].gradeLv[aa] = new Int_Grade();
                        playerInfomation.planets[i].tabs[j].gradeLv[aa].lv =
                            new int[scriptable_Group.GetTargetListByResearch(aa).Count];
                        for (int ii = 0; ii < num; ii++)
                        {
                            playerInfomation.planets[i].tabs[j].gradeLv[aa].lv[ii] =
                                    scriptable_Group.GetTargetListByResearch(aa)[ii].research_Level;
                        }
                    }                  
                }
                else if (j == 3)
                {
                    playerInfomation.planets[i].tabs[j].gradeLv = new Int_Grade[5];
                    for (int aa = 0; aa < 5; aa++)
                    {
                        num = scriptable_Group.GetTargetListByShips(aa).Count;

                        playerInfomation.planets[i].tabs[j].gradeLv[aa] = new Int_Grade();
                        playerInfomation.planets[i].tabs[j].gradeLv[aa].lv =
                            new int[scriptable_Group.GetTargetListByShips(aa).Count];

                        for (int ii = 0; ii < num; ii++)
                        {
                            playerInfomation.planets[i].tabs[j].gradeLv[aa].lv[ii] =
                                    scriptable_Group.GetTargetListByShips(aa)[ii].currentHave_Ship;
                        }
                    }  
                }
                else
                {
                    playerInfomation.planets[i].tabs[j].gradeLv = new Int_Grade[5];
                    for (int aa = 0; aa < 5; aa++)
                    {
                        num = scriptable_Group.GetTargetListByBuildResource(j, aa).Count;

                        playerInfomation.planets[i].tabs[j].gradeLv[aa] = new Int_Grade();
                        playerInfomation.planets[i].tabs[j].gradeLv[aa].lv =
                            new int[scriptable_Group.GetTargetListByBuildResource(j, aa).Count];


                        for (int ii = 0; ii < num; ii++)
                        {
                            playerInfomation.planets[i].tabs[j].gradeLv[aa].lv[ii] =
                                    scriptable_Group.GetTargetListByBuildResource(j, aa)[ii].level;
                        }
                    }
                }
            }
        }
    }

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

    //함선생산과 관제센터 업그레이드 함수 중복 정리 하여 간결하게 하기 위한 코드 정리 실험
    public void Example_Confirm(Sprite img, int makeCount, Base_Infomation info, bool upgrade)
    {
        mainTabCategory = mainTab_Container[Active_TabContainerIndex()].GetComponent<MainTabCategory>();
        if (upgrade)
        {
            if (info.tabs == Base_Infomation.Tabs.Tab1)
            {
                tab1 = true;
                tab1_coroutine = StartCoroutine(Tab1_Building(
                    img,
                    info,
                    info.buildResource.building_Time[info.buildResource.level]));
            }
            else if (info.tabs == Base_Infomation.Tabs.Tab2)
            {
                tab2 = true;
                tab2_coroutine = StartCoroutine(Tab1_Building(
                    img,
                    info,
                    info.buildResource.building_Time[info.buildResource.level]));
            }
            else if (info.tabs == Base_Infomation.Tabs.Tab4)
            {
                info.containerSlide.imgBtn.enabled = false;
                tab4_coroutine = StartCoroutine(
                    MakingShips_Timer(
                        img,
                        info,
                        makeCount));
            }
            else
            {
                tab5_coroutine = StartCoroutine(
                            ControlCenter_BuildingTimer(
                                img,
                                info,
                                info.buildResource.building_Time[info.buildResource.level]));
            }
        }
        else
        {
            if (info.tabs == Base_Infomation.Tabs.Tab1)
            {
                tab1 = false;
                StopCoroutine(tab1_coroutine);
            }
            else if (info.tabs == Base_Infomation.Tabs.Tab2)
            {
                tab2 = false;
                StopCoroutine(tab2_coroutine);
            }
            else if (info.tabs == Base_Infomation.Tabs.Tab4)
            {
                info.containerSlide.imgBtn.enabled = true;
                StopCoroutine(tab4_coroutine);
                info.child_InfoContainer.transform.GetChild(0).gameObject.SetActive(upgrade);
                info.child_InfoContainer.transform.GetChild(1).gameObject.SetActive(!upgrade);
            }
            else
            {
                StopCoroutine(tab5_coroutine);
                /*mainTabCategory.Upgrading(null, false);*/
            }

            for (int i = 0; i < 2; i++)
            {
                info.timeSlider[i].value = 0f;
                info.timeText[i].text = $"{info.buildResource.building_Time[info.buildResource.level]}초";
            }
            mainTabCategory.Upgrading(null, false, false);
        }

        info.containerSlide.ColorChange_To_Upgrade(Active_TabContainerIndex());
        if (info.tabs == Base_Infomation.Tabs.Tab4)
        {
            info.child_InfoContainer.transform.GetChild(0).gameObject.SetActive(upgrade);
            info.child_InfoContainer.transform.GetChild(1).gameObject.SetActive(!upgrade);
        }
    }

    IEnumerator MakingShips_Timer(Sprite img, Base_Infomation info, int makeCount)
    {
        int activeIndex = Active_TabContainerIndex();
        GameObject maintab_container = mainTabCategory.Upgrading(img, true, false);
        Slider maintab_slider = mainTabCategory.sliderContainer.GetComponentInChildren<Slider>();
        TextMeshProUGUI[] maintab_texts = mainTabCategory.sliderContainer.GetComponentsInChildren<TextMeshProUGUI>();

        maintab_texts[0].text = $"{info.ship.name} {makeCount}기";

        info.making_Targets_Text.text = $"{info.ship.name} {makeCount}기 생산 중.";
        float totalMakeTime = makeCount * info.ship.shipMaking_Time;
        maintab_slider.maxValue = totalMakeTime;

        foreach (Slider _slider in info.timeSlider)
        {
            _slider.maxValue = totalMakeTime;
        }

        info.child_InfoContainer.transform.GetChild(0).gameObject.SetActive(true);
        info.child_InfoContainer.transform.GetChild(1).gameObject.SetActive(false);

        float timer = 0f;

        float textTimer = 0f;
        int dotCount = 0;
        string dots;
        
        while (timer < totalMakeTime)
        {
            textTimer += Time.deltaTime;
            
            if (textTimer >= 0.5f)
            {
                dotCount = (dotCount % 3) + 1;
                dots = dotCount == 1 ? "." : (dotCount == 2) ? ".." : "...";
                info.making_Targets_Text.text = $"{info.ship.name} {makeCount}기 생산 중{dots}";
                textTimer = 0f;
                
            }

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

        // 6.28 수정 실험 사항 (확인 후 지속할지 삭제할지 판단)
        //info.shipMaking_confirm = false;
        info.confirm = false;


        foreach (Slider slide in info.timeSlider)
        {
            slide.value = 0f;
        }


        info.child_InfoContainer.transform.GetChild(0).gameObject.SetActive(false);
        info.child_InfoContainer.transform.GetChild(1).gameObject.SetActive(true);

        info.btns[1].gameObject.SetActive(false);
        info.btns[0].gameObject.SetActive(true);

        // 메인탭의 이미지 기본사진으로 변경
        maintab_container.GetComponent<MainTabCategory>().Upgrading(null, false, false);
    }

    IEnumerator ControlCenter_BuildingTimer(Sprite img, Base_Infomation info, float targetTimer)
    {
        int activeIndex = Active_TabContainerIndex();
        GameObject maintab_container = mainTabCategory.Upgrading(img, true, false);
        Slider maintab_slider = mainTabCategory.sliderContainer.GetComponentInChildren<Slider>();
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

        //info.controlCenter_confirm = false;
        info.confirm = false;
        foreach (Slider slide in info.timeSlider)
        {
            slide.value = 0f;
        }
        info.timeSlider[0].gameObject.SetActive(false);
        info.btns[1].gameObject.SetActive(false);
        info.btns[0].gameObject.SetActive(true);

        // 메인탭의 이미지 기본사진으로 변경
        maintab_container.GetComponent<MainTabCategory>().Upgrading(null, false, false);

        /*관제센터 업그레이드 후 생산가능 수량 업데이트 정보를
            함선생산 탭의 정보로 넘겨주기(현재는 infomation의 ships 정보를 수정하는 방향)*/
        info.ship.maxHaveShip_Amount = info.buildResource.AllowableBuild;

        // 항상 업그레이드 완료 또는 생산 완료 후에는 유저 정보를 업데이트 하여 취합하는 곳이 필요
        // (서버에 통신을 용이하게 하기 위함)
    }

    IEnumerator Tab1_Building(Sprite img, Base_Infomation info, float targetTimer)
    {
        int activeIndex = Active_TabContainerIndex();
        GameObject maintab_container = mainTabCategory.Upgrading(img, true, false);
        Slider maintab_slider = mainTabCategory.sliderContainer.GetComponentInChildren<Slider>();
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

        info.confirm = false;
        foreach (Slider slide in info.timeSlider)
        {
            slide.value = 0f;
        }
        info.timeSlider[0].gameObject.SetActive(false);
        info.btns[1].gameObject.SetActive(false);
        info.btns[0].gameObject.SetActive(true);


        if (info.tabs == Base_Infomation.Tabs.Tab1) tab1 = false;
        else tab2 = false;

        // 메인탭의 이미지 기본사진으로 변경
        maintab_container.GetComponent<MainTabCategory>().Upgrading(null, false, info.tabs == Base_Infomation.Tabs.Tab1 ? tab2 : tab1);

        
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
