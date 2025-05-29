using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class Titles : SerializableDictionary<string, TextMeshProUGUI> { };

public class Infomations : MonoBehaviour
{
    // 5.29�� ������ �켱 �غ��°�
    public Init_SettingScriptable init_SettingScriptable;
    public ShipBuildSlider shipBuildSlider;
    //���ʱ����� ������



    public Titles titles;
    public TextMeshProUGUI[] infos;

    public TextMeshProUGUI[] resources;
    public TextMeshProUGUI[] timeText;
    public TextMeshProUGUI[] amount_Text;

    public BuildResource buildResource;
    public Slider[] timeSlider;
    public Slider amountSlider;
    
    public BuildDetailMatter buildDetailMatter;
    public Button[] btns;
    ImageSlide imgSlide;

    // ���� ���� �����͸� ������ ���� ������ ���� Ȯ�� �۾� �� ������ ������
    // ������ �ҷ����� �ռ�, ����� ���� �����Ͱ� �����Ƿ� �׻� ���� 0�� �ʱ�ȭ�ϴµ� �ʿ��� bool��
    public bool data = false;
    public bool unLock = false;

    void Awake()
    {
        btns = new Button[2];
        resources = new TextMeshProUGUI[5];
        timeText = new TextMeshProUGUI[2];
        amount_Text = new TextMeshProUGUI[2];

        timeSlider = new Slider[2];
        

        SelfRegistration[] selfs = GetComponentsInChildren<SelfRegistration>();
        buildDetailMatter = GetComponentInParent<BuildDetailMatter>();

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
        if (buildResource.build_Category == BuildResource.Build_Category.ContorolCenter)
        {
            resources[3].text = $"{buildResource.AllowableBuild} (+{addnum})";

            // case�� �����ϴ°� ��������??
            resources[4].text = $"���� ���� {buildResource.name}"; // ���� ���� �Լ� ����
        }
        


        string timeStr = TimerTexting(buildResource.building_Time[buildResource.level]);
        /*int time = buildResource.building_Time[buildResource.level];
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
        }*/

        if (buildResource.build_Category == BuildResource.Build_Category.ContorolCenter)
        {
            foreach (TextMeshProUGUI tt in timeText)
            {
                tt.text = $"{timeStr}";
            }
        }
        else
        {
            timeText[0].text = $"{timeStr}";
        }
            

        if (buildResource != null)
        {
            titles["name"].text = $"Lv.{buildResource.level} {buildResource.name} ��������";
        }


        btns[1].gameObject.SetActive(false);
        timeSlider[0].gameObject.SetActive(false);

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

        curLv += 1;
        buildResource.level = curLv;
        titles["name"].text = $"Lv. {curLv} {buildResource.name}";




        int metal = buildResource.init_Needs[0];
        int cristal = buildResource.init_Needs[1];
        int gas = buildResource.init_Needs[2];
        int allowableBuild = 0;
        for (int i = 0; i < buildResource.level+1; i++) // �� 1�� �� ����� �ұ� Ȯ���Ұ�
        {
            metal = Mathf.FloorToInt(metal * buildResource.build_require[i]);
            cristal = Mathf.FloorToInt(cristal * buildResource.build_require[i]);
            gas = Mathf.FloorToInt(gas * buildResource.build_require[i]);
            allowableBuild = allowableBuild + (int)buildResource.build_result[i];
        }

        resources[0].text = $"{metal}";
        resources[1].text = $"{cristal}";
        resources[2].text = $"{gas}";
        resources[3].text = $"{allowableBuild}";
        buildResource.AllowableBuild = allowableBuild;


        // �ӽ� ����. ���߿� �������� ���δ��� �ؾ��ҵ�
        // ���� �����̴� �ð� �ؽ�Ʈ ǥ�� ������ 

        string timeStr = TimerTexting(buildResource.building_Time[buildResource.level]);
        /*int time = buildResource.building_Time[buildResource.level];
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
        }*/

        foreach (TextMeshProUGUI tt in timeText)
        {
            tt.text = $"{timeStr}";
        }
    }

    public string TimerTexting(int timer)
    {
        string timeStr = "";
        int time = timer;
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

        return timeStr;
    }
}
