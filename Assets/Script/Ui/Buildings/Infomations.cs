using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class Titles : SerializableDictionary<string, TextMeshProUGUI> { };

public class Infomations : MonoBehaviour
{
    public Titles titles;
    public TextMeshProUGUI[] infos;
    /*  0: lv,
        1: upgrading time,
        2:
    
     */
    public TextMeshProUGUI[] resources;
    public BuildResource buildResource;
    public Slider slider;
    public TextMeshProUGUI timeText;
    public BuildDetailMatter buildDetailMatter;
    public Button[] btns = new Button[2];
    ImageSlide imgSlide;

    // ���� ���� �����͸� ������ ���� ������ ���� Ȯ�� �۾� �� ������ ������
    // ������ �ҷ����� �ռ�, ����� ���� �����Ͱ� �����Ƿ� �׻� ���� 1�� �ʱ�ȭ�ϴµ� �ʿ��� bool��
    public bool data = false;
    public bool unLock = false;

    void Awake()
    {
        resources = new TextMeshProUGUI[4];

        SelfRegistration[] selfs = GetComponentsInChildren<SelfRegistration>();

        foreach (SelfRegistration self in selfs)
        {
            self.Init_Setting();
        }

        // ���⼭ �̹��������̵� ���ռ����� ���ִ°� ������
        imgSlide = GetComponentInParent<ImageSlide>();
        imgSlide.Init_Setting();

        Init_Setting();
    }

    void Init_Setting()
    {
        // ���� ���� �����͸� ������ ���� ������ ���� Ȯ�� �۾� �� ������ ������
        // ������ �ҷ����� �ռ�, ����� ���� �����Ͱ� �����Ƿ� �׻� ���� 1�� �ʱ�ȭ�ϴµ� �ʿ��� bool��
        if (!data)
        {
            buildResource.level = 0;
            buildResource.AllowableBuild = (int)buildResource.build_result[buildResource.level];
        }

        int metal = buildResource.init_Needs[0];
        int cristal = buildResource.init_Needs[1];
        int gas = buildResource.init_Needs[2];
        for (int i = 0; i < buildResource.level; i++)
        {
            metal = Mathf.FloorToInt(metal * buildResource.build_require[i]);
            cristal = Mathf.FloorToInt(cristal * buildResource.build_require[i]);
            gas = Mathf.FloorToInt(gas * buildResource.build_require[i]);
        }

        // �ӽ� �߰����ɴ�� ����
        int addnum = 0;
        resources[0].text = $"{metal}";
        resources[1].text = $"{cristal}";
        resources[2].text = $"{gas}";
        resources[3].text = $"{buildResource.AllowableBuild} (+{addnum})";

        string timeStr = "";
        int time = buildResource.building_Time[buildResource.level];
        if (time >= 3600)
        {
            int hours = time / 3600;
            int minutes = (time % 3600) / 60;
            int seconds = time % 60;
            timeStr = string.Format("{0}�ð� {1}�� {2}��", hours, minutes, seconds);
        }
        else if (time >= 60)
        {
            int minutes = time / 60;
            int seconds = time % 60;
            timeStr = string.Format("{0}�� {1}��", minutes, seconds);
        }
        else
        {
            timeStr = string.Format("{0}��", time);
        }

        timeText.text = $"{timeStr}";





        if (buildResource != null)
        {
            titles["name"].text = $"Lv.{buildResource.level} {buildResource.name}";
        }

        // �ر� ���� Ȯ�ο��� �Ͽ� ��ư ��� Ȯ�� ����
        UnLockCheck(unLock);
    }

    public void UnLockCheck(bool unlock) // �ر� ���¿� ���� ���� ó��
    {
        unLock = unlock;
        imgSlide.ColorSetting(unLock);
        btns[0].enabled = unLock;
    }


    public void Upgrade()
    {
        int curLv = buildResource.level;


        /*for (int i = 0; i < buildResource.cur_Needs.Length; i++)
        {
            buildResource.cur_Needs[i] = Mathf.FloorToInt(buildResource.cur_Needs[i] * buildResource.build_require[buildResource.level]);
            resources[i].text = buildResource.cur_Needs[i].ToString();
        }*/

        curLv += 1;
        buildResource.level = curLv;
        titles["name"].text = $"Lv. {curLv} {buildResource.name}";




        int metal = buildResource.init_Needs[0];
        int cristal = buildResource.init_Needs[1];
        int gas = buildResource.init_Needs[2];
        int allowableBuild = (int)buildResource.build_result[buildResource.level];
        for (int i = 0; i < buildResource.level; i++)
        {
            metal = Mathf.FloorToInt(metal * buildResource.build_require[i]);
            cristal = Mathf.FloorToInt(cristal * buildResource.build_require[i]);
            gas = Mathf.FloorToInt(gas * buildResource.build_require[i]);
        }

        resources[0].text = $"{metal}";
        resources[1].text = $"{cristal}";
        resources[2].text = $"{gas}";
        resources[3].text = $"{allowableBuild}";
    }


}
