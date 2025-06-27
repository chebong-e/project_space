using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Title_Text : SerializableDictionary<string, TextMeshProUGUI> { };
public class Base_Infomation : MonoBehaviour
{
    public enum Tabs { Tab1, Tab2, Tab3, Tab4, Tab5 }
    public Tabs tabs;

    public ShipBuildSlider shipBuildSlider; // tab4
    public Title_Text title_Text;
    public TextMeshProUGUI[] resources; // �ڿ��ؽ�Ʈ����
    public TextMeshProUGUI[] timeText; // ����ð� �ؽ�Ʈ
    public TextMeshProUGUI[] amount_Text; // ���� ���� ����
    public TextMeshProUGUI[] production; // �ڿ��ǹ��� ���귮, ���¼Һ�
    public TextMeshProUGUI haveShipCount;
    public TextMeshProUGUI making_Targets_Text;
    public BuildResource buildResource;
    public Ship ship;
    public Slider[] timeSlider; // ����ð��� ���� ǥ���� �����̴�
    public Slider amountSlider; // ������ �Լ� ���� ����
    public Button[] btns; // ���׷��̵�, ���׷��̵� ��� ��ư
    public GameObject child_InfoContainer;
    public ContainerSlide containerSlide;

    public bool data = false;
    public bool unLock;
    public bool controlCenter_confirm;
    public bool shipMaking_confirm;

    public virtual void Init_Setting()
    {
        btns = new Button[2];
        resources = new TextMeshProUGUI[5];
        timeText = new TextMeshProUGUI[2];
        production = new TextMeshProUGUI[2];
        timeSlider = new Slider[2];

        CommonResetData();

        /*if (info_types == Types.Tab4)
        {
            shipBuildSlider = GetComponentInChildren<ShipBuildSlider>();
            shipBuildSlider.Init();
        }*/

        Init_ContainerSlide();
        Init_SelfRegistrations();

        Init_CostInfo();

        SelectedTabs();
        ApplyResourceText();

        foreach (Button btn in btns)
        {
            btn.onClick.AddListener(UpgradeConfirm_Or_Cancle);
        }
        btns[1].gameObject.SetActive(false);
        timeSlider[0].gameObject.SetActive(false);

        UnLockCheck(unLock);
    }

    void CommonResetData()
    {
        if (!data)
        {
            //buildResource.level = 0;
            buildResource.AllowableBuild = (int)buildResource.build_result[buildResource.level];
            if (ship != null)
            {
                ship.maxHaveShip_Amount = buildResource.AllowableBuild;
                ship.currentHave_Ship = 0;
            }
        }
    }

    protected virtual void Init_ContainerSlide()
    {
        containerSlide = GetComponent<ContainerSlide>();
        containerSlide.Init_Setting();
    }

    void Init_SelfRegistrations()
    {
        foreach (SelfRegister self in GetComponentsInChildren<SelfRegister>())
        {
            self.Init_Setting();
        }
    }

    public virtual void Init_CostInfo()
    {
        // �⺻������ ����ΰ� �ڽ� Ŭ�������� �������̵�
    }

    public virtual void SelectedTabs()
    {

    }

    protected virtual void ApplyResourceText()
    {
        // ����ΰų� ���� ó��
    }

    protected virtual void UnLockCheck(bool unlock)
    {
        // ��ư ��� ���� ��
        unLock = unlock;
        containerSlide.Init_ColorSetting(unLock);
        btns[0].interactable = unLock;
    }

    public void Upgrade_To_Infomation(Base_Infomation info)
    {
        if (info.shipBuildSlider) // Tab4
        {
            haveShipCount.text = $"���� �Լ� �� : {ship.currentHave_Ship}";
            shipBuildSlider.slider.maxValue = ship.maxHaveShip_Amount - ship.currentHave_Ship;
            shipBuildSlider.slider.value = 0;
            shipBuildSlider.building_Amount.text = $"<       {shipBuildSlider.slider.value}     /     {shipBuildSlider.slider.maxValue}       >";
        }
        else // Tab5
        {
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
                tt.text = $"{TimerTexting(buildResource.building_Time[buildResource.level])}";
            }

            // �Լ����� ���� �ִ� �Լ� ���� ���� ������Ʈ
            ship.maxHaveShip_Amount = allowableBuild;
            var imgGroup = Build_Manager.instance.containerSlide_Group;
            for (int i = 0; i < imgGroup.controlCenter_Tab.Count; i++)
            {
                if (imgGroup.controlCenter_Tab[i] == info.containerSlide)
                {
                    imgGroup.build_Tab4[i].Infomation.shipBuildSlider.
                        building_Amount.text =
                        $"<       {imgGroup.build_Tab4[i].Infomation.shipBuildSlider.slider.value}     /     {ship.maxHaveShip_Amount - ship.currentHave_Ship}       >";
                    imgGroup.build_Tab4[i].Infomation.shipBuildSlider.
                        slider.maxValue = ship.maxHaveShip_Amount - ship.currentHave_Ship;
                    imgGroup.build_Tab4[i].Infomation.shipBuildSlider.slider.value
                        = ship.maxHaveShip_Amount - ship.currentHave_Ship >= 1 ? 1 : 0;
                    break;
                }
            }
        }
    }

    public void UpgradeConfirm_Or_Cancle()
    {
        if (shipBuildSlider)
        {
            shipMaking_confirm = !shipMaking_confirm;
        }
        else
        {
            controlCenter_confirm = !controlCenter_confirm;
        }

        Build_Manager.instance.Example_Confirm(
            transform.GetChild(1).GetComponent<Image>().sprite,
            tabs == Tabs.Tab4 ? (int)shipBuildSlider.slider.value : 0,
            this,
            tabs == Tabs.Tab4 ? shipMaking_confirm : controlCenter_confirm);
    }







    public virtual int Supplement_Allowable()
    {
        // �߰� �ɷ�ġ ����
        return 0;
    }

    internal string TimerTexting(int timer)
    {
        if (timer >= 3600)
            return $"{timer / 3600}�ð� {(timer % 3600) / 60}�� {timer % 60}��";
        else if (timer >= 60)
            return $"{timer / 60}�� {timer % 60}��";
        else
            return $"{timer}��";
    }
    public void Init_DataReset()
    {
        buildResource.AllowableBuild = 0;
        buildResource.level = 0;
    }
}
