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


    // �̰͵� 6.2 Ȯ�ο�, Ȯ���� ���ʿ� �Ǵ� �Ǹ� ����ó��
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



    // �̹��������̵��� AllWindow_Active_IndexCheck������ �ߺ�, ���� �ʿ�
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
        Debug.Log($"�ε�����:{index}, Ȱ�������� ������Ʈ:{TabContainer[index].name}");
        return index;
    }

    // ����� ���������� ���� �����ͼ� ǥ�õǵ��� �Ǿ� ����
    // �� �����̳��� ���׷��̵� ������ Ȯ���Ͽ� ���׷��̵� ���̶�� �� ���׷��̵� ������ ������ ǥ���ϴ� ���� �ʿ�
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

    // 6.2 �ۼ���
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


        //���׷��̵� ���� ��ư �� ���ó��*/
        imgSlide.ImageChange_toUpgrade(Active_TabContainer_Index());  
    }



    // image, slider, Infomation, ImageSlide, bool(���׷��̵� ���� �� ���� ���� Ȯ����)
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
                info.timeText[i].text = $"{info.buildResource.building_Time[info.buildResource.level]}��";
            }
            mainTabCategory.Upgrading(null, false);
        }
        //���׷��̵� ���� ��ư �� ���ó��
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
            info.timeSlider[0].value = buildTimer; // �ߺ��� �ʿ��ұ�... ���߿� ���� ������������.....
            slder.value = buildTimer;

            int curTime = Mathf.CeilToInt(remainingTime);

            string timeStr = "";

            if (curTime >= 3600)
            {
                int hours = curTime / 3600;
                int minutes = (curTime % 3600) / 60;
                int seconds = curTime % 60;
                timeStr = string.Format("{0}�ð� {1}�� {2}��", hours, minutes, seconds);
            }
            else if (curTime >= 60)
            {
                int minutes = curTime / 60;
                int seconds = curTime % 60;
                timeStr = string.Format("{0}�� {1}��", minutes, seconds);
            }
            else
            {
                timeStr = string.Format("{0}��", curTime);
            }

            foreach (TextMeshProUGUI tt in info.timeText)
            {
                tt.text = timeStr;
                ExText[1].text = timeStr;
            }

            yield return null;

        }
        // ���׷��̵� �Ϸ�� ȣ���� ���
        // ��ҹ�ư ��Ȱ��ȭ �� ���׷��̵� ��ư Ȱ��ȭ
        // �ش� ��ư �� ��ư �÷�ó�� �� ���Ȱ��ȭ
        // ���� ���� �ʿ� �ڿ� ǥ��
        info.Upgrade_to_Infomation();
        imgSlide.ImageChange_toUpgrade(index);

        // ���׷��̵� �Ϸ�
        info.confirm = false;
        info.timeSlider[0].value = 0f;
        info.timeSlider[1].value = 0f;
        info.timeSlider[0].gameObject.SetActive(false);
        info.btns[1].gameObject.SetActive(false);
        info.btns[0].gameObject.SetActive(true);

        mainTabCategory.Upgrading(null, false);



        //////////////////////////////////////// �θ��� 1��° �ڽ� ������Ʈ ã�¹��
        Transform current = info.transform; ;
        Transform root = info.transform.root;

        while (current.parent != null && current.parent != root)
        {
            current = current.parent;
        }
        Debug.Log($"����: {current.name}");

        

        // �Լ����� ���� �ش� �Լ����� ���׷��̵� ���� ǥ�����־�� ��.
        // �Ʒ��� �켱 ����
        /*for (int i = 0; i < EventManager.instance.containers.Count; i++)
        {
            EventManager.instance.containers[]
        }*/

        info.shipBuildSlider.ControlCenterUpgrade_for_Ship(info.buildResource.AllowableBuild);
        // 0 �� �Ѱ��ִ°��� �����ؾ���


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
