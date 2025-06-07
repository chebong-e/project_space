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

        // 컨테이너슬라이드를 메인탭 별로 분류하여 변수에 할당
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

    // 활성화 상태 추적을 위한 로직
    // 활성화 상태 추적하여 해당 카테고리별 이미지 전환 유도
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

    // 열려있는 탭을 확인하여 그 중 선택되어 슬라이드 되어있는 컨테이너가 있다면
    // 컨테이너를 슬라이드 닫기처리
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
                info.timeText[i].text = $"{info.buildResource.building_Time[info.buildResource.level]}초";
            }
            mainTabCategory.Upgrading(null, false);
        }
        //업그레이드 중인 버튼 외 흑백처리
        info.containerSlide.ColorChange_To_Upgrade(Active_TabContainerIndex());
    }

    IEnumerator ControlCenter_BuildingTimer(Sprite img, Con_Infomation info, float targetTimer)
    {
        // 6.7일자까지 수정한 사항이고 이후부터 계속 수정작업 시행
        // Con_Infomation의 160번 줄 예외처리 다시 복귀하고 같이 수정진행

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
