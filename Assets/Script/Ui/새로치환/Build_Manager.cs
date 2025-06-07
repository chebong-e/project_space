using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

            }
        }
    }

    public void ControlCenter_Upgrade(Sprite img, Con_Infomation info, bool upgrade)
    {
        mainTabCategory = mainTab_Container[Active_TabContainerIndex()].GetComponent<MainTabCategory>();



        if (upgrade)
        {
            float targetTimer = info.buildResource.building_Time[info.buildResource.level];
            controllCenter_coroutine = StartCoroutine(ControlCenter_BuildingTimer(img, info, targetTimer));
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

    IEnumerator ControlCenter_BuildingTimer(Sprite img, Con_Infomation info, float targetTimer)
    {
        // 6.7���ڱ��� ������ �����̰� ���ĺ��� ��� �����۾� ����
        // Con_Infomation�� 160�� �� ����ó�� �ٽ� �����ϰ� ���� ��������

        yield return null;
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
