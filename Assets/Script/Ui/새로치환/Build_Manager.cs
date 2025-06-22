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
                info.timeText[i].text = $"{info.buildResource.building_Time[info.buildResource.level]}��";
            }
            mainTabCategory.Upgrading(null, false);
        }

        info.containerSlide.ColorChange_To_Upgrade(Active_TabContainerIndex());
        info.child_InfoContainer.transform.GetChild(0).gameObject.SetActive(upgrade);
        info.child_InfoContainer.transform.GetChild(1).gameObject.SetActive(!upgrade);
    }

    //�Լ������ �������� ���׷��̵� �Լ� �ߺ� ���� �Ͽ� �����ϰ� �ϱ� ���� �ڵ� ���� ����
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
                info.timeText[i].text = $"{info.buildResource.building_Time[info.buildResource.level]}��";
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
                info.timeText[i].text = $"{info.buildResource.building_Time[info.buildResource.level]}��";
            }
            mainTabCategory.Upgrading(null, false);
        }
        //���׷��̵� ���� ��ư �� ���ó��
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

        // ���׷��̵� ���� �� �ݿ��� ������ ����ȭ
        info.ship.currentHave_Ship += makeCount;
        info.Upgrade_To_Infomation(info);
        

        // �̹��� �÷���ȯ �� ��ư Ȱ��ȭ ó��
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

        // �������� �̹��� �⺻�������� ����
        maintab_container.GetComponent<MainTabCategory>().Upgrading(null, false);
    }

    IEnumerator ControlCenter_BuildingTimer(Sprite img, Con_Infomation info, float targetTimer)
    {
        // 6.7���ڱ��� ������ �����̰� ���ĺ��� ��� �����۾� ����
        // Con_Infomation�� 160�� �� ����ó�� �ٽ� �����ϰ� ���� ��������
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

        // ���׷��̵� ���� �� �ݿ��� ������ ����ȭ
        info.Upgrade_To_Infomation(info);

        // �̹��� �÷���ȯ �� ��ư Ȱ��ȭ ó��
        info.containerSlide.ColorChange_To_Upgrade(activeIndex);

        info.controlCenter_confirm = false;
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
