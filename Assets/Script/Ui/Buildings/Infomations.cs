using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class Titles : SerializableDictionary<string, TextMeshProUGUI> { };

public class Infomations : MonoBehaviour
{

    public enum Types { ControlCenter, Tab3 }
    public Types info_types;
    // 5.29�� ������ �켱 �غ��°�
    public Init_SettingScriptable init_SettingScriptable;
    public ShipBuildSlider shipBuildSlider;
    //���ʱ����� ������



    public Titles titles;
    /*public TextMeshProUGUI[] infos;*/

    public TextMeshProUGUI[] resources;
    public TextMeshProUGUI[] timeText;
    public TextMeshProUGUI[] amount_Text;

    public BuildResource buildResource;
    public Ships ships;
    public Slider[] timeSlider;
    public Slider amountSlider;
    
    public Button[] btns;
    ImageSlide imgSlide;

    // ���� ���� �����͸� ������ ���� ������ ���� Ȯ�� �۾� �� ������ ������
    // ������ �ҷ����� �ռ�, ����� ���� �����Ͱ� �����Ƿ� �׻� ���� 0�� �ʱ�ȭ�ϴµ� �ʿ��� bool��
    public bool data = false;
    public bool unLock = false;
    public bool confirm;
    public bool ship_confirm;

    public GameObject infoContainer;
    void Awake()
    {
        btns = new Button[2];
        resources = new TextMeshProUGUI[5];
        timeText = new TextMeshProUGUI[2];
        /*amount_Text = new TextMeshProUGUI[2];*/
        timeSlider = new Slider[2];
        imgSlide = GetComponentInParent<ImageSlide>();
        

        foreach (SelfRegistration self in GetComponentsInChildren<SelfRegistration>())
        {
            self.Init_Setting();
        }

        imgSlide.Init_Setting();

        Init_Setting();

        
    }

    void Start()
    {
        if (info_types == Types.Tab3)
        {
            infoContainer = shipBuildSlider.transform.parent.gameObject;
            infoContainer.transform.GetChild(0).gameObject.SetActive(false);
        }
            

        
    }

    public void Init_Setting()
    {
        int metal = info_types == Types.ControlCenter ? buildResource.init_Needs[0] : ships.shipMake_Cost[0];
        int cristal = info_types == Types.ControlCenter ? buildResource.init_Needs[1] : ships.shipMake_Cost[1];
        int gas = info_types == Types.ControlCenter ? buildResource.init_Needs[2] : ships.shipMake_Cost[2];

        // ���� ���� �����͸� ������ ���� ������ ���� Ȯ�� �۾� �� ������ ������
        // ������ �ҷ����� �ռ�, ����� ���� �����Ͱ� �����Ƿ� �׻� ���� 1�� �ʱ�ȭ�ϴµ� �ʿ��� bool��
        if (!data)
        {
            if (info_types == Types.ControlCenter)
            {
                buildResource.level = 0;
                buildResource.AllowableBuild = (int)buildResource.build_result[buildResource.level];
                for (int i = 0; i < buildResource.level; i++)
                {
                    metal = Mathf.FloorToInt(metal * buildResource.build_require[i]);
                    cristal = Mathf.FloorToInt(cristal * buildResource.build_require[i]);
                    gas = Mathf.FloorToInt(gas * buildResource.build_require[i]);
                }

                // �ӽ� �߰����ɴ�� ����
                int addnum = 0;

                resources[3].text = $"{buildResource.AllowableBuild} (+{addnum})";
                resources[4].text = $"���� ���� {buildResource.name}"; // ���� ���� �Լ� ����


                foreach (TextMeshProUGUI tt in timeText)
                {
                    tt.text = $"{TimerTexting(buildResource.building_Time[buildResource.level])}";
                }
                titles["name"].text = $"Lv.{buildResource.level} {buildResource.name} ��������";
            }
            else // �Լ����� Infomations
            {
                Debug.Log("ships����");

                timeText[0].text = $"{TimerTexting((int)ships.shipMaking_Time)}";
                titles["name"].text = $"{ships.name}";
            }
        }


        


        /*int metal = buildResource.init_Needs[0];
        int cristal = buildResource.init_Needs[1];
        int gas = buildResource.init_Needs[2];*/
        

        // �ӽ� �߰����ɴ�� ����
        /*int addnum = 0;*/
        resources[0].text = $"{metal}";
        resources[1].text = $"{cristal}";
        resources[2].text = $"{gas}";
        

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


    public void Upgrade_to_Infomation(Types types)
    {
        switch (types)
        {
            case Types.Tab3:

                break;
            case Types.ControlCenter:
                int curLv = buildResource.level;

                curLv += 1;
                buildResource.level = curLv;

                titles["name"].text = $"Lv.{buildResource.level} {buildResource.name} ��������";

                int metal = buildResource.init_Needs[0];
                int cristal = buildResource.init_Needs[1];
                int gas = buildResource.init_Needs[2];
                int allowableBuild = 0;
                for (int i = 0; i < buildResource.level + 1; i++) // �� 1�� �� ����� �ұ� Ȯ���Ұ�
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

                foreach (TextMeshProUGUI tt in timeText)
                {
                    tt.text = $"{timeStr}";
                }
                break;
        }

        /*int curLv = buildResource.level;

        curLv += 1;
        buildResource.level = curLv;*/
        /*titles["name"].text = $"Lv. {curLv} {buildResource.name}";*/


        /*int metal = buildResource.init_Needs[0];
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

        foreach (TextMeshProUGUI tt in timeText)
        {
            tt.text = $"{timeStr}";
        }*/
    }

    public string TimerTexting(int timer)
    {
        if (timer >= 3600)
            return $"{timer / 3600}�ð� {(timer % 3600) / 60}�� {timer % 60}��";
        else if (timer >= 60)
            return $"{timer / 60}�� {timer % 60}��";
        else 
            return $"{timer}��";
    }

    public void UpgradeStart_or_Cancle()
    {
        switch (info_types)
        {
            case Types.Tab3:
                ship_confirm = !ship_confirm;
                BuildManager.instance.buildShips = ship_confirm;

                BuildManager.instance.BuildTab3_BuildShips(
                    transform.GetChild(1).GetComponent<Image>().sprite,
                    shipBuildSlider.slider.value * ships.shipMaking_Time,
                    this,
                    imgSlide,
                    ship_confirm);
                break;

            case Types.ControlCenter:
                confirm = !confirm;
                BuildManager.instance.upgrading = confirm;

                BuildManager.instance.ControlCenter_Upgrade(
                    transform.GetChild(1).GetComponent<Image>().sprite,
                    this,
                    imgSlide,
                    confirm);
                break;
        }


        /*if (info_types == Types.ControlCenter)
        {
            confirm = !confirm;
            BuildManager.instance.upgrading = confirm;

            BuildManager.instance.ControlCenter_Upgrade(transform.GetChild(1).GetComponent<Image>().sprite,
                this,
                imgSlide,
                confirm);
        }
        else
        {
            ship_confirm = !ship_confirm;
            BuildManager.instance.buildShips = ship_confirm;

            BuildManager.instance.BuildTab3_BuildShips(transform.GetChild(1).GetComponent<Image>().sprite,
                shipBuildSlider.slider.value * ships.shipMaking_Time,
                this,
                imgSlide,
                ship_confirm);
        }*/
    }

}
