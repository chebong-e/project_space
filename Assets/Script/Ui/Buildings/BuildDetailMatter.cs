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

    // ���� ���� �����͸� ������ ���� ������ ���� Ȯ�� �۾� �� ������ ������
    // ������ �ҷ����� �ռ�, ����� ���� �����Ͱ� �����Ƿ� �׻� ���� 1�� �ʱ�ȭ�ϴµ� �ʿ��� bool��
    public bool data = false;


    void Awake()
    {
        infos = transform.GetChild(2).GetComponent<Infomations>();
    }

    void Start()
    {
        
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
                timeStr = string.Format("{0:D2}�ð� {1:D2}�� {2:D2}��", hours, minutes, seconds);
            }
            else if (curTime >= 60)
            {
                int minutes = curTime / 60;
                int seconds = curTime % 60;
                timeStr = string.Format("{0:D2}�� {1:D2}��", minutes, seconds);
            }
            else
            {
                timeStr = string.Format("{0:D2}��", curTime);
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
            targetTimer = infos.buildResource.building_Time[infos.buildResource.level - 1];

            //�̹��� ��� ó�� �� �ش� ��ư ���� �ٸ� ��ư ��Ȱ��ȭ ó��
            GetComponent<ImageSlide>().ImageChange_toUpgrade();

            // �����̵� �ð�ǥ�� ó��
            coroutine = StartCoroutine(Timer());

            // ���׷��̵� ���� ǥ��
            // �̰��� ���׷��̵� �Ϸ�� ó���Ǿ�� ��.
        }
        else // ���׷��̵� ����
        {
            GetComponent<ImageSlide>().ImageChange_toUpgrade();
            StopCoroutine(coroutine);
            infos.slider.value = 0f;
            infos.timeText.text = $"{infos.buildResource.building_Time[infos.buildResource.level - 1]}��";
            buildTimer = 0f;
        }
        
    }





}
