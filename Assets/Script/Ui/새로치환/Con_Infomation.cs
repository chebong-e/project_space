using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Title_Text : SerializableDictionary<string, TextMeshProUGUI> { };
public class Con_Infomation : MonoBehaviour
{
    public enum Types { Tab1, Tab2, Tab3, Tab4, ControlCenter }
    public Types info_types;

    public ShipBuildSlider shipBuildSlider;
    public Title_Text title_Text;
    public TextMeshProUGUI[] resources; // �ڿ��ؽ�Ʈ����
    public TextMeshProUGUI[] timeText; // ����ð� �ؽ�Ʈ
    public TextMeshProUGUI[] amount_Text; // ���� ���� ����
    public TextMeshProUGUI haveShipCount;
    public BuildResource buildResource;
    public Ship ship;
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

    public void Init_Setting()
    {
        btns = new Button[2];
        resources = new TextMeshProUGUI[5];
        timeText = new TextMeshProUGUI[2];
        timeSlider = new Slider[2];

        // ���������Ͱ� �������� ó���� ���� 0���� �������� �ʱ�ȭ
        if (!data)
        {
            buildResource.level = 0;
            buildResource.AllowableBuild = (int)buildResource.build_result[buildResource.level];
            ship.maxHaveShip_Amount = buildResource.AllowableBuild;
            ship.currentHave_Ship = 0;
        }


        if (info_types == Types.Tab4)
        {
            shipBuildSlider = GetComponentInChildren<ShipBuildSlider>();
            shipBuildSlider.Init();
        }
        
        containerSlide = GetComponent<ContainerSlide>();

        foreach (SelfRegistration self in GetComponentsInChildren<SelfRegistration>())
        {
            self.Init_Setting();
        }
        
        containerSlide.Init_Setting();


        int[] costArray = info_types == Types.ControlCenter
            ? buildResource.init_Needs
            : ship.shipMake_Cost;

        var (metal, cristal, gas) = (costArray[0], costArray[1], costArray[2]);

        switch (info_types)
        {
            case Types.ControlCenter:
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

            case Types.Tab4:
                timeText[0].text = $"{TimerTexting((int)ship.shipMaking_Time)}";
                title_Text["name"].text = $"{ship.name}";
                child_InfoContainer.transform.GetChild(0).gameObject.SetActive(false);
                break;
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
        btns[0].interactable = unLock;
    }

    public void Upgrade_To_Infomation(Con_Infomation info)
    {
        switch (info.info_types)
        {
            case Types.Tab4:
                haveShipCount.text = $"���� �Լ� �� : {ship.currentHave_Ship}";
                shipBuildSlider.slider.maxValue = ship.maxHaveShip_Amount - ship.currentHave_Ship;
                shipBuildSlider.slider.value = 0;
                shipBuildSlider.building_Amount.text = $"<       {shipBuildSlider.slider.value}     /     {shipBuildSlider.slider.maxValue}       >";
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
                ship.maxHaveShip_Amount = allowableBuild;
                var imgGroup = Build_Manager.instance.containerSlide_Group;
                for (int i = 0; i < imgGroup.controlCenter_Tab.Count; i++)
                {
                    if (imgGroup.controlCenter_Tab[i] == info.containerSlide)
                    {
                        imgGroup.build_Tab4[i].con_Infomation.shipBuildSlider.
                            building_Amount.text = 
                            $"<       {imgGroup.build_Tab4[i].con_Infomation.shipBuildSlider.slider.value}     /     {ship.maxHaveShip_Amount - ship.currentHave_Ship}       >";
                        imgGroup.build_Tab4[i].con_Infomation.shipBuildSlider.
                            slider.maxValue = ship.maxHaveShip_Amount - ship.currentHave_Ship;
                        imgGroup.build_Tab4[i].con_Infomation.shipBuildSlider.slider.value
                            = ship.maxHaveShip_Amount - ship.currentHave_Ship >= 1 ? 1 : 0;
                        break;
                    }
                }
                break;
        }
    }

    public void UpgradeConfirm_Or_Cancle()
    {
        switch (info_types)
        {
            case Types.Tab4:
                shipMaking_confirm = !shipMaking_confirm;
                break;
            case Types.ControlCenter:
                controlCenter_confirm = !controlCenter_confirm;
                break;
        }

        Build_Manager.instance.Example_Confirm(
            transform.GetChild(1).GetComponent<Image>().sprite,
            shipBuildSlider != null ? (int)shipBuildSlider.slider.value : 0,
            this,
            info_types == Types.Tab4 ? shipMaking_confirm : controlCenter_confirm);

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

    public void Init_DataReset()
    {
        buildResource.AllowableBuild = 0;
        buildResource.level = 0;
    }
}
