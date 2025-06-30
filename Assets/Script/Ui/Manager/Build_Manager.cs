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

    Coroutine tab1_coroutine, tab2_coroutine, tab3_coroutine, tab4_coroutine, tab5_coroutine;
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
                tabContainer[i].GetComponentInChildren<Scriptable_Matching>()?.Init();
            } 
        }

        foreach (GameObject obj in tabContainer)
        {
            obj.SetActive(false);
        }


        // ��ġ�� ������� ����
        float value = 100;
        for (int i = 0; i < 10; i++)
        {
            value *= 1.6f;
            Debug.Log($"�⺻��:{value},  ������:{(int)value},  ���谪:{((int)value / 10) * 10}");
        }

    }

    // �������� ������ ������ Ȯ���Ͽ� �ݿ����ֱ� ���� �ʱ� ����(�ӽ�)
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

    //�Լ������ �������� ���׷��̵� �Լ� �ߺ� ���� �Ͽ� �����ϰ� �ϱ� ���� �ڵ� ���� ����
    public void Example_Confirm(Sprite img, int makeCount, Base_Infomation info, bool upgrade)
    {
        mainTabCategory = mainTab_Container[Active_TabContainerIndex()].GetComponent<MainTabCategory>();
        if (upgrade)
        {
            if (info.tabs == Base_Infomation.Tabs.Tab1)
            {
                tab1_coroutine = StartCoroutine(Tab1_Building(
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
                StopCoroutine(tab1_coroutine);
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
                info.timeText[i].text = $"{info.buildResource.building_Time[info.buildResource.level]}��";
            }
            mainTabCategory.Upgrading(null, false);
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
        GameObject maintab_container = mainTabCategory.Upgrading(img, true);
        Slider maintab_slider = mainTabCategory.sliderContainer.GetComponentInChildren<Slider>();
        TextMeshProUGUI[] maintab_texts = mainTabCategory.sliderContainer.GetComponentsInChildren<TextMeshProUGUI>();

        maintab_texts[0].text = $"{info.ship.name} {makeCount}��";

        info.making_Targets_Text.text = $"{info.ship.name} {makeCount}�� ���� ��.";
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
        maintab_container.GetComponent<MainTabCategory>().Upgrading(null, false);
    }

    IEnumerator ControlCenter_BuildingTimer(Sprite img, Base_Infomation info, float targetTimer)
    {
        int activeIndex = Active_TabContainerIndex();
        GameObject maintab_container = mainTabCategory.Upgrading(img, true);
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
        maintab_container.GetComponent<MainTabCategory>().Upgrading(null, false);

        /*�������� ���׷��̵� �� ���갡�� ���� ������Ʈ ������
            �Լ����� ���� ������ �Ѱ��ֱ�(����� infomation�� ships ������ �����ϴ� ����)*/
        info.ship.maxHaveShip_Amount = info.buildResource.AllowableBuild;

        // �׻� ���׷��̵� �Ϸ� �Ǵ� ���� �Ϸ� �Ŀ��� ���� ������ ������Ʈ �Ͽ� �����ϴ� ���� �ʿ�
        // (������ ����� �����ϰ� �ϱ� ����)
    }

    IEnumerator Tab1_Building(Sprite img, Base_Infomation info, float targetTimer)
    {
        int activeIndex = Active_TabContainerIndex();
        GameObject maintab_container = mainTabCategory.Upgrading(img, true);
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

        info.confirm = false;
        foreach (Slider slide in info.timeSlider)
        {
            slide.value = 0f;
        }
        info.timeSlider[0].gameObject.SetActive(false);
        info.btns[1].gameObject.SetActive(false);
        info.btns[0].gameObject.SetActive(true);

        // �������� �̹��� �⺻�������� ����
        maintab_container.GetComponent<MainTabCategory>().Upgrading(null, false);

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
