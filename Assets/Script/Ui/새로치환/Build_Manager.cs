using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Build_Manager : MonoBehaviour
{
    public static Build_Manager instance;
    public ImageSetting_Group ImageSetting_Group;

    public GameObject[] TabContainer;

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
