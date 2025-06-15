using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Build_Manager : MonoBehaviour
{
    public static Build_Manager instance;
    public ImageSetting_Group ImageSetting_Group;
    public MainTabCategory mainTabCategory;
    public GameObject[] TabContainer, mainTab_Container;

    Coroutine controllCenter_coroutine, makingShip_coroutine;

    public bool upgrading, makingShips, building, reserching;

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
        for (int i = 0; i < TabContainer.Length; i++)
        {
            List<ContainerSlide> targetContainer = GetTargetListByIndex(i);

            if (targetContainer == null) continue;

            ScrollRect[] scrollRects = TabContainer[i].GetComponentsInChildren<ScrollRect>(true);
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
    }

    public List<ContainerSlide> GetTargetListByIndex(int index)
    {
        switch (index)
        {
            case 0: return ImageSetting_Group.build_Tab1;
            case 1: return ImageSetting_Group.build_Tab2;
            case 2: return ImageSetting_Group.build_Tab3;
            case 3: return ImageSetting_Group.reserch_Tab;
            case 4: return ImageSetting_Group.controlCenter_Tab;
            default: return null;
        }
    }

    // Ȱ��ȭ ���� ������ ���� ����
    // Ȱ��ȭ ���� �����Ͽ� �ش� ī�װ��� �̹��� ��ȯ ����
    public int Active_TabContainerIndex()
    {
        int index = 0;
        for (int i = 0; i < TabContainer.Length; i++)
        {
            if (TabContainer[i].activeInHierarchy)
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

    public void BuildTab3_MakingShips(Sprite img, float totalTime, Con_Infomation info, bool upgrade)
    {
        mainTabCategory = mainTab_Container[Active_TabContainerIndex()].GetComponent<MainTabCategory>();

        if (upgrade)
        {
            makingShip_coroutine = StartCoroutine(
                MakingShips_Timer(
                    img,
                    info,
                    totalTime));
        }
        else
        {
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
    public void Example_Confirm(Sprite img, float totalTime, Con_Infomation info, bool upgrade)
    {
        mainTabCategory = mainTab_Container[Active_TabContainerIndex()].GetComponent<MainTabCategory>();
        if (upgrade)
        {
            switch (info.info_types)
            {
                case Con_Infomation.Types.Tab3:
                    makingShip_coroutine = StartCoroutine(
                        MakingShips_Timer(
                            img,
                            info,
                            totalTime));
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
                case Con_Infomation.Types.Tab3:
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
            info.containerSlide.ColorChange_To_Upgrade(Active_TabContainerIndex());
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

    IEnumerator MakingShips_Timer(Sprite img, Con_Infomation info, float targetTimer)
    {
        GameObject maintab_container = mainTabCategory.Upgrading(img, true);
        Slider maintab_slider = maintab_container.GetComponentInChildren<Slider>();
        TextMeshProUGUI[] maintab_texts = maintab_container.GetComponentsInChildren<TextMeshProUGUI>();
        maintab_texts[0].text = info.title_Text["name"].text;

        maintab_slider.maxValue = targetTimer;

        foreach (Slider _slider in info.timeSlider)
        {
            _slider.maxValue = targetTimer;
        }

        info.child_InfoContainer.transform.GetChild(0).gameObject.SetActive(true);
        info.child_InfoContainer.transform.GetChild(1).gameObject.SetActive(false);

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
        info.Upgrade_To_Infomation(info.info_types);

        // �̹��� �÷���ȯ �� ��ư Ȱ��ȭ ó��
        info.containerSlide.ColorChange_To_Upgrade(Active_TabContainerIndex());

        info.shipMaking_confirm = false;

        foreach (Slider slide in info.timeSlider)
        {
            slide.value = 0f;
        }
        info.timeSlider[0].gameObject.SetActive(false);
        info.btns[1].gameObject.SetActive(false);
        info.btns[0].gameObject.SetActive(true);

        // �������� �̹��� �⺻�������� ����
        mainTabCategory.Upgrading(null, false);
    }

    IEnumerator ControlCenter_BuildingTimer(Sprite img, Con_Infomation info, float targetTimer)
    {
        // 6.7���ڱ��� ������ �����̰� ���ĺ��� ��� �����۾� ����
        // Con_Infomation�� 160�� �� ����ó�� �ٽ� �����ϰ� ���� ��������
        int activeIndex = Active_TabContainerIndex();
        GameObject maintab_container = mainTabCategory.Upgrading(img, true);
        Slider maintab_slider = maintab_container.GetComponentInChildren<Slider>();
        TextMeshProUGUI[] maintab_texts = maintab_container.GetComponentsInChildren<TextMeshProUGUI>();
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
        info.Upgrade_To_Infomation(info.info_types);

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
        mainTabCategory.Upgrading(null, false);

        /*�������� ���׷��̵� �� ���갡�� ���� ������Ʈ ������
            �Լ����� ���� ������ �Ѱ��ֱ�(����� infomation�� ships ������ �����ϴ� ����)*/
        info.ships.maxHaveShip_Amount = info.buildResource.AllowableBuild;
    }


    /*IEnumerator BuildTimerCoroutine(Sprite img, Con_Infomation info, float targetTimer, Action onComplete)
    {
        GameObject maintab_container = mainTabCategory.Upgrading(img, true);
        Slider maintab_slider = maintab_container.GetComponentInChildren<Slider>();
        TextMeshProUGUI[] maintab_texts = maintab_container.GetComponentsInChildren<TextMeshProUGUI>();
        maintab_texts[0].text = info.title_Text["name"].text;

        maintab_slider.maxValue = targetTimer;

        foreach (Slider s in info.timeSlider)
            s.maxValue = targetTimer;

        if (info.child_InfoContainer != null)
        {
            info.child_InfoContainer.transform.GetChild(0).gameObject.SetActive(true);
            info.child_InfoContainer.transform.GetChild(1).gameObject.SetActive(false);
        }

        float timer = 0f;

        while (timer < targetTimer)
        {
            timer += Time.deltaTime;

            foreach (Slider s in info.timeSlider)
                s.value = timer;

            maintab_slider.value = timer;

            int remaining = Mathf.CeilToInt(Mathf.Clamp(targetTimer - timer, 0f, targetTimer));
            string timeStr = TimerTexting(remaining);

            foreach (TextMeshProUGUI tt in info.timeText)
                tt.text = timeStr;

            maintab_texts[1].text = timeStr;

            yield return null;
        }

        info.Upgrade_To_Infomation(info.info_types);
        info.containerSlide.ColorChange_To_Upgrade(Active_TabContainerIndex());

        // ���� ��ó��
        foreach (Slider s in info.timeSlider)
            s.value = 0f;

        info.timeSlider[0].gameObject.SetActive(false);
        info.btns[1].gameObject.SetActive(false);
        info.btns[0].gameObject.SetActive(true);

        mainTabCategory.Upgrading(null, false);

        onComplete?.Invoke(); // Ÿ�Կ� ���� ����Ǵ� ��ó�� �ݹ�
    }

    IEnumerator MakingShips_Timer(Sprite img, Con_Infomation info, float timer)
    {
        yield return StartCoroutine(BuildTimerCoroutine(img, info, timer, () =>
        {
            info.shipMaking_confirm = false;
        }));
    }

    IEnumerator ControlCenter_BuildingTimer(Sprite img, Con_Infomation info, float timer)
    {
        yield return StartCoroutine(BuildTimerCoroutine(img, info, timer, () =>
        {
            info.controlCenter_confirm = false;
            info.ships.maxHaveShip_Amount = info.buildResource.AllowableBuild;
        }));
    }*/


    public string TimerTexting(int timer)
    {
        if (timer >= 3600)
            return $"{timer / 3600}�ð� {(timer % 3600) / 60}�� {timer % 60}��";
        else if (timer >= 60)
            return $"{timer / 60}�� {timer % 60}��";
        else
            return $"{timer}��";
    }
}

[System.Serializable]
public class ImageSetting_Group
{
    public List<ContainerSlide> build_Tab1;
    public List<ContainerSlide> build_Tab2;
    public List<ContainerSlide> build_Tab3;
    public List<ContainerSlide> reserch_Tab;
    public List<ContainerSlide> controlCenter_Tab;

}
