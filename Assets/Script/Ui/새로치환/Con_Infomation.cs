using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Title_Text : SerializableDictionary<string, TextMeshProUGUI> { };
public class Con_Infomation : MonoBehaviour
{
    public enum Types { ControlCenter, Tab3 }
    public Types info_types;

    public ShipBuildSlider shipBuildSlider;
    public Title_Text title_Text;
    public TextMeshProUGUI[] resources; // �ڿ��ؽ�Ʈ����
    public TextMeshProUGUI[] timeText; // ����ð� �ؽ�Ʈ
    public TextMeshProUGUI[] amount_Text; // ���� ���� ����
    public BuildResource buildResource;
    public Ships ships;
    public Slider[] timeSlider; // ����ð��� ���� ǥ���� �����̴�
    public Slider amountSlider; // ������ �Լ� ���� ����
    public Button[] btns; // ���׷��̵�, ���׷��̵� ��� ��ư
    public GameObject child_InfoContainer;

    public ContainerSlide containerSlide;

    /* ���� ���� �����͸� ������ ���� ������ ���� Ȯ�� �۾� �� ������ ������ ������ �ҷ����� �ռ�,
     * ����� ���� �����Ͱ� �����Ƿ� �׻� ���� 0�� �ʱ�ȭ�ϴµ� �ʿ��� bool��*/
    public bool data = false;
    public bool unLock;
    public bool controlCenter_confirm;
    public bool shipMaking_confirm;

    void Awake()
    {
        btns = new Button[2];
        resources = new TextMeshProUGUI[5];
        timeText = new TextMeshProUGUI[2];
        timeSlider = new Slider[2];
        containerSlide = GetComponent<ContainerSlide>();

        foreach (SelfRegistration self in GetComponentsInChildren<SelfRegistration>())
        {
            self.Init_Setting();
        }

        containerSlide.Init_Setting();
        Init_Setting();
    }

    public void Init_Setting()
    {
        int[] costArray = info_types == Types.ControlCenter
            ? buildResource.init_Needs
            : ships.shipMake_Cost;

        var (metal, cristal, gas) = (costArray[0], costArray[1], costArray[2]);

        if (!data)
        {
            switch (info_types)
            {
                case Types.ControlCenter:
                    buildResource.level = 0;
                    buildResource.AllowableBuild = (int)buildResource.build_result[buildResource.level];
                    for (int i = 0; i < buildResource.level; i++)
                    {
                        metal = Mathf.FloorToInt(metal * buildResource.build_require[i]);
                        cristal = Mathf.FloorToInt(cristal * buildResource.build_require[i]);
                        gas = Mathf.FloorToInt(gas * buildResource.build_require[i]);
                    }

                    // �߰����ɴ��(���ʽ� �� ���� ��) = �ӽ�
                    int plusAllowable = 0;
                    resources[3].text = $"{buildResource.AllowableBuild} (+{plusAllowable})";
                    resources[4].text = $"���� ���� {buildResource.name}"; // ���� ���� �Լ� ����

                    foreach (TextMeshProUGUI tt in timeText)
                    {
                        tt.text = $"{TimerTexting(buildResource.building_Time[buildResource.level])}";
                    }
                    title_Text["name"].text = $"Lv.{buildResource.level} {buildResource.name} ��������";
                    break;

                case Types.Tab3:
                    Debug.Log("ships����");

                    timeText[0].text = $"{TimerTexting((int)ships.shipMaking_Time)}";
                    title_Text["name"].text = $"{ships.name}";
                    child_InfoContainer.transform.GetChild(0).gameObject.SetActive(false);
                    break;
            }
        }

        resources[0].text = $"{metal}";
        resources[1].text = $"{cristal}";
        resources[2].text = $"{gas}";

        // ���׷��̵� ���� �ƴ϶�� �Ʒ� �� ������Ʈ�� �����Ҵ� �� ��Ȱ��ȭ ó���� default
        btns[1].gameObject.SetActive(false); 
        timeSlider[0].gameObject.SetActive(false);

        // �ر� ���� Ȯ�ο��� �Ͽ� ��ư ��� Ȯ�� ����
        UnLockCheck(unLock);
    }

    
    void UnLockCheck(bool unlock) // �ر� ���¿� ���� ���� ó��
    {
        unLock = unlock;
        containerSlide.Init_ColorSetting(unLock);
        btns[0].enabled = unLock;
    }

    public void Upgrade_To_Infomation(Types types)
    {
        switch (types)
        {
            case Types.Tab3:
                
                break;
            case Types.ControlCenter:
                buildResource.level++;
                title_Text["name"].text = $"Lv.{buildResource.level} {buildResource.name} ��������";

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
                foreach (TextMeshProUGUI tt in timeText)
                {
                    tt.text = $"{Build_Manager.instance.TimerTexting(buildResource.building_Time[buildResource.level])}";
                }

                // �Լ����� ���� �ִ� �Լ� ���� ���� ������Ʈ
                ships.maxHaveShip_Amount = allowableBuild;
                // �ش� �������̼� ��ũ��Ʈ���� �����彽���̴� ��ũ��Ʈ�� �����Ƿ� ���� ���� ����
                // ������ ���� �ε��� ���� ������ �ε��� ���� Ȱ���Ͽ� ������Ʈ ���ֵ��� ����.
                shipBuildSlider.building_Amount.text = $"{ships.maxHaveShip_Amount}";
                shipBuildSlider.slider.maxValue = ships.maxHaveShip_Amount;
                break;
        }
    }

    public void UpgradeConfirm_Or_Cancle()
    {
        switch (info_types)
        {
            case Types.Tab3:
                if (shipBuildSlider.slider.value < 1)
                    return;
                shipMaking_confirm = !shipMaking_confirm;
                Build_Manager.instance.makingShips = shipMaking_confirm;

                Build_Manager.instance.BuildTab3_MakingShips(
                    transform.GetChild(1).GetComponent<Image>().sprite,
                    shipBuildSlider.slider.value * ships.shipMaking_Time,
                    this,
                    shipMaking_confirm);
                break;

            case Types.ControlCenter:
                controlCenter_confirm = !controlCenter_confirm;
                Build_Manager.instance.upgrading = controlCenter_confirm;

                Build_Manager.instance.ControlCenter_Upgrade(
                    transform.GetChild(1).GetComponent<Image>().sprite,
                    this,
                    controlCenter_confirm);

                break;
        }
    }

    string TimerTexting(int timer)
    {
        if (timer >= 3600)
            return $"{timer / 3600}�ð� {(timer % 3600) / 60}�� {timer % 60}��";
        else if (timer >= 60)
            return $"{timer / 60}�� {timer % 60}��";
        else
            return $"{timer}��";
    }
}
