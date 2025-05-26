using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;
    public ControlCenter controlCenter;
    Coroutine coroutine;

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


    }

    public int Check()
    {
        int index = 0;
        for (int i = 0; i < EventManager.instance.imageSliderGroup.Count; i++)
        {
            List<ImageSlide> img_s = EventManager.instance.imageSliderGroup[i].imageSlide;
            foreach (ImageSlide img in img_s)
            {
               if (img.gameObject.activeInHierarchy)
                {
                    index = i;
                    break;
                }
            }
        }
        return index;
    }

    public void ExClose()
    {
        int num = Check();
        List<ImageSlide> img_s = EventManager.instance.imageSliderGroup[num].imageSlide;

        foreach (ImageSlide img in img_s)
        {
            if (img.open)
            {
                img.open = false;
                img.SliderOn_Off();
                
            }
        }
    }





    // image, slider, Infomation, ImageSlide, bool(���׷��̵� ���� �� ���� ���� Ȯ����)
    public void ControlCenter_Upgrade(Sprite img, Infomations info, ImageSlide imgSlide, bool upgrade)
    {
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
                info.slider[i].value = 0f;
                info.timeText[i].text = $"{info.buildResource.building_Time[info.buildResource.level]}��";
            }
        }
        imgSlide.ImageChange_toUpgrade();
    }


    IEnumerator ControlCenter_BuildingTimer(Sprite img, Infomations info, ImageSlide imgSlide, float targetTimer)
    {
        GameObject con = controlCenter.Upgrading(img, true);
        Slider slder = con.GetComponentInChildren<Slider>();
        TextMeshProUGUI[] ExText = con.GetComponentsInChildren<TextMeshProUGUI>();
        ExText[0].text = info.titles["name"].text;


        slder.maxValue = targetTimer;


        info.slider[0].maxValue = targetTimer;
        info.slider[1].maxValue = targetTimer;
        float buildTimer = 0f;

        while (buildTimer < targetTimer)
        {
            buildTimer += Time.deltaTime;
            float remainingTime = Mathf.Clamp(targetTimer - buildTimer, 0f, targetTimer);
            info.slider[1].value = buildTimer;
            info.slider[0].value = buildTimer; // �ߺ��� �ʿ��ұ�... ���߿� ���� ������������.....
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
        info.Upgrade();
        imgSlide.ImageChange_toUpgrade();

        // ���׷��̵� �Ϸ�
        info.buildDetailMatter.confirm = false;
        info.slider[0].value = 0f;
        info.slider[1].value = 0f;
        info.slider[0].gameObject.SetActive(false);
        info.btns[1].gameObject.SetActive(false);
        info.btns[0].gameObject.SetActive(true);
        controlCenter.Upgrading(null, false);
    }




}
