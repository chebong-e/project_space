using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using static UnityEditor.PlayerSettings;

[System.Serializable]
public class values : SerializableDictionary<string, GameObject> { };
public class BuildDetailMatter : MonoBehaviour
{
    public Infomations infos;
    public float buildTimer;
    public bool ccBuild;
    public float targetTimer;
    bool confirm;

    Coroutine coroutine;

    void Awake()
    {
        infos = transform.GetChild(2).GetComponent<Infomations>();
    }

    IEnumerator Timer()
    {
        infos.slider.maxValue = targetTimer;
        buildTimer = 0f;

        while (buildTimer < targetTimer)
        {
            buildTimer += Time.deltaTime;
            float remainingTime = Mathf.Clamp(targetTimer - buildTimer, 0f, targetTimer);
            infos.slider.value = buildTimer;

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

            infos.timeText.text = timeStr;

            yield return null;

        }
        // ���׷��̵� �Ϸ�� ȣ���� ���
        // ��ҹ�ư ��Ȱ��ȭ �� ���׷��̵� ��ư Ȱ��ȭ
        // �ش� ��ư �� ��ư �÷�ó�� �� ���Ȱ��ȭ
        // ���� ���� �ʿ� �ڿ� ǥ��
        infos.Upgrade();

        Upgrade();
        infos.btns[1].gameObject.SetActive(false);
        infos.btns[0].gameObject.SetActive(true);
    }


    public void Upgrade()
    {
        confirm = !confirm;
        if (confirm) // ���׷��̵� ����
        {
            targetTimer = infos.buildResource.building_Time[infos.buildResource.level];


            if (infos.unLock)
            {

            }
            
            //�̹��� ��� ó�� �� �ش� ��ư ���� �ٸ� ��ư ��Ȱ��ȭ ó��
            GetComponent<ImageSlide>().ImageChange_toUpgrade();

            // �����̵� �ð�ǥ�� ó��
            coroutine = StartCoroutine(Timer());

            // ���׷��̵� ���� ǥ��
            // �̰��� ���׷��̵� �Ϸ�� ó���Ǿ�� ��.
        }
        else // ���׷��̵� ����
        {
            if (infos.unLock)
            {
                
            }
            GetComponent<ImageSlide>().ImageChange_toUpgrade();
            StopCoroutine(coroutine);
            infos.slider.value = 0f;
            infos.timeText.text = $"{infos.buildResource.building_Time[infos.buildResource.level]}��";
            buildTimer = 0f;
        }
        
    }





}
