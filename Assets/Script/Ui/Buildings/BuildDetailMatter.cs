using UnityEngine;
using System.Collections;
using TMPro;

[System.Serializable]
public class values : SerializableDictionary<string, GameObject> { };
public class BuildDetailMatter : MonoBehaviour
{
    public Infomations infos;
    ImageSlide imgSlide;
    public float buildTimer;
    public float targetTimer;
    public bool confirm;

    Coroutine coroutine;

    void Awake()
    {
        infos = transform.GetChild(2).GetComponent<Infomations>();
        imgSlide = GetComponent<ImageSlide>();
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
            StopCoroutine(coroutine);

            for (int i = 0; i < 2; i++)
            {
                infos.slider[i].value = 0f;
                infos.timeText[i].text = $"{infos.buildResource.building_Time[infos.buildResource.level]}��";
            }
            
            buildTimer = 0f;
        }
        //�̹��� ��� ó�� �� �ش� ��ư ���� �ٸ� ��ư ��Ȱ��ȭ ó��
        imgSlide.ImageChange_toUpgrade();

    }


    IEnumerator Timer()
    {
        infos.slider[0].maxValue = targetTimer;
        infos.slider[1].maxValue = targetTimer;
        buildTimer = 0f;

        while (buildTimer < targetTimer)
        {
            buildTimer += Time.deltaTime;
            float remainingTime = Mathf.Clamp(targetTimer - buildTimer, 0f, targetTimer);
            infos.slider[1].value = buildTimer;
            infos.slider[0].value = buildTimer; // �ߺ��� �ʿ��ұ�... ���߿� ���� ������������.....


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

            foreach (TextMeshProUGUI tt in infos.timeText)
            {
                tt.text = timeStr;
            }

            yield return null;

        }
        // ���׷��̵� �Ϸ�� ȣ���� ���
        // ��ҹ�ư ��Ȱ��ȭ �� ���׷��̵� ��ư Ȱ��ȭ
        // �ش� ��ư �� ��ư �÷�ó�� �� ���Ȱ��ȭ
        // ���� ���� �ʿ� �ڿ� ǥ��
        infos.Upgrade();
        imgSlide.ImageChange_toUpgrade();
        // ���׷��̵� �Ϸ�
        confirm = false;
        infos.slider[0].value = 0f;
        infos.slider[1].value = 0f;
        infos.slider[0].gameObject.SetActive(false);
        infos.btns[1].gameObject.SetActive(false);
        infos.btns[0].gameObject.SetActive(true);
    }



}
