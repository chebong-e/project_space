using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Base_Infomation;


public class Build_Manager : MonoBehaviour
{
    public static Build_Manager instance;   
    
    public PlayerInfomation playerInfomation;

    public ContainerSlide_Group containerSlide_Group;
    public MainTabCategory mainTabCategory;
    public GameObject[] tabContainer, mainTab_Container;
    public Scriptable_Group scriptable_Group;

    Coroutine tab1_coroutine, tab2_coroutine, tab3_coroutine, tab4_coroutine, tab5_coroutine;
    

    public bool userData;
    //public bool upgrading, makingShips, building, reserching;
    public bool tab1 { get; private set; }
    public bool tab2 { get; private set; }
    public bool tab3 { get; private set; }
    public bool tab4 { get; private set; }
    public bool tab5 { get; private set; }

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

        // �����̳ʽ����̵带 ������ ���� �з��Ͽ� ������ �Ҵ�
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

        // �� ���� ��ũ���ͺ��Ī�� �ʱ�ȭ�� �ѵ� ù��° ȭ�鸸 ���ΰ� �ٸ� �� ��Ȱ��ȭ
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
                if (i == 2)
                {
                    Scriptable_Matching[] child = tabContainer[i].GetComponentsInChildren<Scriptable_Matching>();
                    for (int j = 0; j < child.Length; j++)
                    {
                        child[j].Init();
                        if (j > 0)
                        {
                            child[j].gameObject.SetActive(false);
                        }
                    }
                }
                else
                {
                    tabContainer[i].GetComponentInChildren<Scriptable_Matching>()?.Init();
                }                    
            } 
        }

        foreach (GameObject obj in tabContainer)
        {
            obj.SetActive(false);
        }

        


        /*// ��ġ�� ������� ����
        float value = 100;
        for (int i = 0; i < 10; i++)
        {
            value *= 1.6f;
            Debug.Log($"�⺻��:{value},  ������:{(int)value},  ���谪:{((int)value / 10) * 10}");
        }*/

        /*// �÷��̾� ���� �Ҵ� ���� (06-30)
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
        //        Debug.Log($"Ȩ�÷����� �Ǽ��������� = tab{i + 1} : {j}\n" +
        //            $"{playerInfomation.build_Levels[0][i, j]}");

        //    }
        //}



        /*playerInfomation.planets = new BuildLevels[2]; // ����(Ȩ�÷��ְ� �ݷδ�1)
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

    // �Լ��� �������ʹ� �׷��̵尡 5�� �� ����ȭ �ǹǷ� �̰��� ���� ����ȭ�Ͽ� �����ϴ°� �ʿ�
    public void PlayerInfoDataSave() // ������ �������
    {
        playerInfomation.planets = new BuildLevels[2]; // ����(Ȩ�÷��ְ� �ݷδ�1)
        for (int i = 0; i < playerInfomation.planets.Length; i++) // 2
        {
            playerInfomation.planets[i] = new BuildLevels();
            playerInfomation.planets[i].tabs = new TabWindows[5];

            for (int j = 0; j < playerInfomation.planets[i].tabs.Length; j++) // 5
            {
                playerInfomation.planets[i].tabs[j] = new TabWindows();
                int num = scriptable_Group.GetTargetListByBuildResource(j, 0).Count; // 0�϶� 8
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

                        playerInfomation.planets[i].tabs[j].gradeLv[aa] = new Int_Grade();
                        playerInfomation.planets[i].tabs[j].gradeLv[aa].lv =
                            new int[scriptable_Group.GetTargetListByResearch(aa).Count];
                        for (int ii = 0; ii < num; ii++)
                        {
                            playerInfomation.planets[i].tabs[j].gradeLv[aa].lv[ii] =
                                    scriptable_Group.GetTargetListByResearch(aa)[ii].level;
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
            case 2: return containerSlide_Group.build_Tab3; // ����
            case 3: return containerSlide_Group.build_Tab4; // �Լ�����
            case 4: return containerSlide_Group.controlCenter_Tab;
            default: return null;
        }
    }

    // Ȱ��ȭ ���� ������ ���� ����
    // Ȱ��ȭ ���� �����Ͽ� �ش� ī�װ��� �̹��� ��ȯ ����
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

    // �����ִ� ���� Ȯ���Ͽ� �� �� ���õǾ� �����̵� �Ǿ��ִ� �����̳ʰ� �ִٸ�
    // �����̳ʸ� �����̵� �ݱ�ó��
    public void TabWindow_Close() // ���� home ��ư�� �Ҵ� ��
    {
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

    //�Լ������ �������� ���׷��̵� �Լ� �ߺ� ���� �Ͽ� �����ϰ� �ϱ� ���� �ڵ� ���� ����
    public void Building_Confirm(Sprite img, int makeCount, Base_Infomation info, bool upgrade)
    {
        mainTabCategory = mainTab_Container[Active_TabContainerIndex()].GetComponent<MainTabCategory>();

        switch (info.tabs)
        {
            case Tabs.Tab1:
                if (upgrade)
                {
                    // ���׷��̵� ��� ����
                    if (ResourceManager.instance.UpgradePerDeduct(info.buildResource.cur_Needs, upgrade))
                    {
                        tab1 = upgrade;
                        // ���׷��̵� ���� ����
                        tab1_coroutine = StartCoroutine(Tab1_Building(
                            img,
                            info,
                            PlayerAbilityInfo.GetCalculatedTime("Build", info.buildResource.building_Time[info.buildResource.level])));

                        info.containerSlide.ColorChange_To_Upgrade(Active_TabContainerIndex());

                    }
                    // ���׷��̵� ����� �������� ������� �ʵ���, ��ư ������� �ʵ��� �ؾ���
                }
                else
                {
                    if (tab1)
                    {
                        ResourceManager.instance.UpgradePerDeduct(info.buildResource.cur_Needs, upgrade);
                        StopCoroutine(tab1_coroutine);
                        CancleForReset(info);
                        info.containerSlide.ColorChange_To_Upgrade(Active_TabContainerIndex());
                    }
                }
                break;
            case Base_Infomation.Tabs.Tab2:
                
                if (upgrade)
                {
                    if (ResourceManager.instance.UpgradePerDeduct(info.buildResource.cur_Needs, upgrade))
                    {
                        tab2 = upgrade;
                        tab2_coroutine = StartCoroutine(Tab1_Building(
                            img,
                            info,
                            PlayerAbilityInfo.GetCalculatedTime("Build", info.buildResource.building_Time[info.buildResource.level])));
                        info.containerSlide.ColorChange_To_Upgrade(Active_TabContainerIndex());
                    }
                }
                else
                {
                    if (tab2)
                    {
                        ResourceManager.instance.UpgradePerDeduct(info.buildResource.cur_Needs, upgrade);
                        StopCoroutine(tab2_coroutine);
                        CancleForReset(info);
                        info.containerSlide.ColorChange_To_Upgrade(Active_TabContainerIndex());
                    }
                }
                break;
            case Base_Infomation.Tabs.Tab3:               
                if (upgrade)
                {
                    if (ResourceManager.instance.UpgradePerDeduct(info.research.research_Cost, upgrade))
                    {
                        tab3 = upgrade;
                        tab3_coroutine = StartCoroutine(Tab3_Researching(
                            img,
                            info,
                            PlayerAbilityInfo.GetCalculatedTime("Research", info.research.research_Time[info.research.level])));
                        
                        info.containerSlide.ColorChange_To_Upgrade(Active_TabContainerIndex());
                    }
                        
                }
                else
                {
                    if (tab3)
                    {
                        ResourceManager.instance.UpgradePerDeduct(info.research.research_Cost, upgrade);
                        StopCoroutine(tab3_coroutine);
                        CancleForReset(info);
                        info.containerSlide.ColorChange_To_Upgrade(Active_TabContainerIndex());
                    }                       
                }
                break;
            case Base_Infomation.Tabs.Tab4:
                
                info.containerSlide.imgBtn.enabled = !upgrade;
                info.child_InfoContainer.transform.GetChild(0).gameObject.SetActive(upgrade);
                info.child_InfoContainer.transform.GetChild(1).gameObject.SetActive(!upgrade);

                int[] shipCost = new int[3];
                for (int i = 0; i < info.ship.shipMake_Cost.Length; i++)
                {
                    shipCost[i] = (int)info.shipBuildSlider.slider.value * info.ship.shipMake_Cost[i];
                }

                if (upgrade)
                {
                    if (ResourceManager.instance.UpgradePerDeduct(shipCost, upgrade))
                    {
                        tab4 = upgrade;
                        tab4_coroutine = StartCoroutine(MakingShips_Timer(
                            img,
                            info,
                            makeCount));
                        info.containerSlide.ColorChange_To_Upgrade(Active_TabContainerIndex());
                    }  
                }
                else
                {
                    if (tab4)
                    {
                        ResourceManager.instance.UpgradePerDeduct(shipCost, upgrade);
                        StopCoroutine(tab4_coroutine);
                        CancleForReset(info);
                        info.containerSlide.ColorChange_To_Upgrade(Active_TabContainerIndex());
                    }     
                }
                break;
            case Base_Infomation.Tabs.Tab5:
                
                if (upgrade)
                {
                    if (ResourceManager.instance.UpgradePerDeduct(info.buildResource.cur_Needs, upgrade))
                    {
                        tab5 = upgrade;
                        tab5_coroutine = StartCoroutine(ControlCenter_BuildingTimer(
                            img,
                            info,
                            PlayerAbilityInfo.GetCalculatedTime("Build", info.buildResource.building_Time[info.buildResource.level])));
                        info.containerSlide.ColorChange_To_Upgrade(Active_TabContainerIndex());
                    }                        
                }
                else
                {
                    if (tab5)
                    {
                        ResourceManager.instance.UpgradePerDeduct(info.buildResource.cur_Needs, upgrade);
                        StopCoroutine(tab5_coroutine);
                        CancleForReset(info);
                        info.containerSlide.ColorChange_To_Upgrade(Active_TabContainerIndex());
                    }                        
                }
                break;
        }
    }

    void CancleForReset(Base_Infomation info)
    {
        int timer = 0;
        if (info.tabs == Tabs.Tab3)
        {
            timer = Mathf.CeilToInt(PlayerAbilityInfo.GetCalculatedTime("Research", info.research.research_Time[info.research.level]));
        }
        else
        {
            timer = Mathf.CeilToInt(PlayerAbilityInfo.GetCalculatedTime("Build", info.buildResource.building_Time[info.buildResource.level]));
        }

        for (int i = 0; i < 2; i++)
        {
            info.timeSlider[i].value = 0f;
            info.timeText[i].text =
                $"{TimerTexting(timer)}";
        }
        
        mainTabCategory.Upgrading(null, false, false);
    }

    IEnumerator MakingShips_Timer(Sprite img, Base_Infomation info, int makeCount)
    {
        int activeIndex = Active_TabContainerIndex();
        GameObject maintab_container = mainTabCategory.Upgrading(img, true, false);
        Slider maintab_slider = mainTabCategory.sliderContainer.GetComponentInChildren<Slider>();
        TextMeshProUGUI[] maintab_texts = mainTabCategory.sliderContainer.GetComponentsInChildren<TextMeshProUGUI>();

        maintab_texts[0].text = $"{info.ship.name} {makeCount}��";

        info.making_Targets_Text.text = $"{info.ship.name} {makeCount}�� ���� ��.";

        float totalMakeTime = makeCount * PlayerAbilityInfo.GetCalculatedTime("Ship", info.ship.shipMaking_Time);
        /*float totalMakeTime = makeCount * info.ship.shipMaking_Time;*/
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
                info.making_Targets_Text.text = $"{info.ship.name} {makeCount}�� ���� ��{dots}";
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

        // ���׷��̵� ���� �� �ݿ��� ������ ����ȭ
        info.ship.currentHave_Ship += makeCount;
        info.Upgrade_To_Infomation(info);
        

        // �̹��� �÷���ȯ �� ��ư Ȱ��ȭ ó��
        info.containerSlide.ColorChange_To_Upgrade(activeIndex);
        info.containerSlide.imgBtn.enabled = true;

        // 6.28 ���� ���� ���� (Ȯ�� �� �������� �������� �Ǵ�)
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

        // �������� �̹��� �⺻�������� ����
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

        // ���׷��̵� ���� �� �ݿ��� ������ ����ȭ
        info.Upgrade_To_Infomation(info);

        // �̹��� �÷���ȯ �� ��ư Ȱ��ȭ ó��
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

        // �������� �̹��� �⺻�������� ����
        maintab_container.GetComponent<MainTabCategory>().Upgrading(null, false, false);

        /*�������� ���׷��̵� �� ���갡�� ���� ������Ʈ ������
            �Լ����� ���� ������ �Ѱ��ֱ�(����� infomation�� ships ������ �����ϴ� ����)*/
        info.ship.maxHaveShip_Amount = info.buildResource.AllowableBuild;

        Ex();
        // �׻� ���׷��̵� �Ϸ� �Ǵ� ���� �Ϸ� �Ŀ��� ���� ������ ������Ʈ �Ͽ� �����ϴ� ���� �ʿ�
        // (������ ����� �����ϰ� �ϱ� ����)
    }

    IEnumerator Tab3_Researching(Sprite img, Base_Infomation info, float targetTimer)
    {
        Debug.Log($"{info.research.name}�� ���׷��̵� �ð� : {targetTimer}");
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

            int remaining_curTime = Mathf.CeilToInt(Mathf.Clamp(targetTimer - timer, 0f, targetTimer));

            foreach (TextMeshProUGUI tt in info.timeText)
            {
                tt.text = TimerTexting(remaining_curTime);
            }
            maintab_texts[1].text = TimerTexting(remaining_curTime);

            yield return null;
        }

        // ���׷��̵� ���� �� �ݿ��� ������ ����ȭ
        info.Upgrade_To_Infomation(info);

        // �̹��� �÷���ȯ �� ��ư Ȱ��ȭ ó��
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
        else if (info.tabs == Base_Infomation.Tabs.Tab2) tab2 = false;
        else tab3 = false;

        // �������� �̹��� �⺻�������� ����
        maintab_container.GetComponent<MainTabCategory>().Upgrading(null, false, info.tabs == Base_Infomation.Tabs.Tab1 ? tab2 : tab1);

        Ex();
       
    }
    IEnumerator Tab1_Building(Sprite img, Base_Infomation info, float targetTimer)
    {
        Debug.Log($"{info.buildResource.name}�� ���׷��̵� �ð� : {targetTimer}");
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

            int remaining_curTime = Mathf.CeilToInt(Mathf.Clamp(targetTimer - timer, 0f, targetTimer));

            foreach (TextMeshProUGUI tt in info.timeText)
            {
                tt.text = TimerTexting(remaining_curTime);
            }
            maintab_texts[1].text = TimerTexting(remaining_curTime);

            yield return null;
        }

        // ���׷��̵� ���� �� �ݿ��� ������ ����ȭ
        info.Upgrade_To_Infomation(info);

        // �̹��� �÷���ȯ �� ��ư Ȱ��ȭ ó��
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
        else if (info.tabs == Base_Infomation.Tabs.Tab2) tab2 = false;
        else tab3 = false;

        // �������� �̹��� �⺻�������� ����
        maintab_container.GetComponent<MainTabCategory>().Upgrading(null, false, info.tabs == Base_Infomation.Tabs.Tab1 ? tab2 : tab1);

        Ex();
    }


    public string TimerTexting(int timer)
    {
        if (timer >= 3600)
            return $"{timer / 3600}�ð� {(timer % 3600) / 60}�� {timer % 60}��";
        else if (timer >= 60)
            return $"{timer / 60}�� {timer % 60}��";
        else
            return $"{timer}��";
    }

    // init�� ����(��� �� ������ ��Ȱ��ȭ)
    public void Tab_Close()
    {
        foreach (GameObject tab in tabContainer)
        {
            tab.SetActive(false);
        }
    }

    // �κ����� ���׷��̵忡 ���� �ð� ���� ���� ����
    public void Ex()
    {
        for (int i = 0; i < tabContainer.Length; i++)
        {
            if (i == 3)
                continue;

            tabContainer[i].GetComponentInChildren<Scriptable_Matching>().Infomation_UpdateSet();
        }
    }

    //������ cur_Init �ʱ�ȭ ���ֱ�
    void OnDestroy()
    {
        List<BuildResource> list_BR = new List<BuildResource>();
        for (int i = 0; i < 2; i++)
        {
            List<Research> research = new List<Research>();
            research = scriptable_Group.GetTargetListByResearch(i);
            foreach (Research re in research)
            {
                re.level = 0;
            }
            if (i < 2)
            {
                list_BR = scriptable_Group.GetTargetListByBuildResource(i, 0);
                foreach (BuildResource build in list_BR)
                {
                    build.level = 0;
                    if (build.resource_Factory == BuildResource.Resource_Factory.Energy)
                    {
                        build.basic_manufacture = 0;
                    }
                }
            }
        }

        List<Ship> ships = new List<Ship>();
        for (int i = 0; i < 4; i++)
        {
            list_BR = scriptable_Group.GetTargetListByBuildResource(4, i);
            foreach (BuildResource build in list_BR)
            {
                
                build.level = 0;
            }
            ships = scriptable_Group.GetTargetListByShips(i);
            foreach (Ship sp in ships)
            {
                sp.currentHave_Ship = 0;
                sp.maxHaveShip_Amount = 0;
            }
        }
        Debug.Log("�ʱ�ȭ �Ϸ�. ������");
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
