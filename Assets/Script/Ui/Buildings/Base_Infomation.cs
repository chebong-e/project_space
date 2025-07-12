using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Base_Infomation;

[System.Serializable]
public class Title_Text : SerializableDictionary<string, TextMeshProUGUI> { };
public class Base_Infomation : MonoBehaviour
{
    public enum Tabs { Tab1, Tab2, Tab3, Tab4, Tab5 }
    public Tabs tabs;

    public ShipBuildSlider shipBuildSlider; // tab4
    public Title_Text title_Text;
    public TextMeshProUGUI[] resources; // 자원텍스트관련
    public TextMeshProUGUI[] timeText; // 경과시간 텍스트
    public TextMeshProUGUI[] amount_Text; // 생산 가능 수량
    public TextMeshProUGUI[] production; // 자원건물의 생산량, 전력소비량
    public TextMeshProUGUI haveShipCount;
    public TextMeshProUGUI making_Targets_Text;
    public BuildResource buildResource;
    public Research research;
    public Ship ship;
    public Slider[] timeSlider; // 경과시간에 따른 표시할 슬라이더
    public Slider amountSlider; // 생산할 함선 수량 관련
    public Button[] btns; // 업그레이드, 업그레이드 취소 버튼
    public GameObject child_InfoContainer;
    public ContainerSlide containerSlide;

    public bool data = false;
    public bool unLock;
    /*public bool controlCenter_confirm;
    public bool shipMaking_confirm;
    public bool tab1_Confirm, tab2_Confirm, tab3_Confirm, tab4_Confirm, tab5_Confirm;*/
    public bool confirm;

    public virtual void Init_Setting()
    {
        btns = new Button[2];
        resources = new TextMeshProUGUI[5];
        timeText = new TextMeshProUGUI[2];
        production = new TextMeshProUGUI[3];
        timeSlider = new Slider[2];

        SelectedTabs();

        CommonResetData();

        /*if (info_types == Types.Tab4)
        {
            shipBuildSlider = GetComponentInChildren<ShipBuildSlider>();
            shipBuildSlider.Init();
        }*/

        Init_ContainerSlide();
        Init_SelfRegistrations();

        Init_CostInfo();

        
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
        // Build_Manager.instance.userData가 true라면 우선 레벨은 모두 0으로 초기화 해주기로 바꿔놓음
        
        /*if (Build_Manager.instance.userData)
        {
            switch (tabs)
            {
                case Tabs.Tab1:
                case Tabs.Tab2:
                case Tabs.Tab5:
                    buildResource.level = 0;
                    break;
            }
            
        }


        // 임시로직
        if (!data)
        {
            //buildResource.level = 0;
            if (buildResource != null)
            {
                buildResource.AllowableBuild = (int)buildResource.build_result[buildResource.level];
            }
                
            if (ship != null)
            {
                ship.maxHaveShip_Amount = buildResource.AllowableBuild;
                ship.currentHave_Ship = 0;
            }
        }*/
        
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

    protected virtual void Init_CostInfo()
    {
        
    }

    protected virtual void SelectedTabs()
    {

    }

    protected virtual void ApplyResourceText()
    {
        // 비워두거나 공통 처리
    }

    protected virtual void UnLockCheck(bool unlock)
    {
        // 버튼 잠금 로직 등
        unLock = unlock;
        containerSlide.Init_ColorSetting(unLock);
        btns[0].interactable = unLock;
    }

    public void Upgrade_To_Infomation(Base_Infomation info)
    {
        switch (info.tabs)
        {
            case Tabs.Tab1:
            case Tabs.Tab2:
            case Tabs.Tab3:
            case Tabs.Tab5:
                OverlapTextCode(info);
                break;
            case Tabs.Tab4:
                haveShipCount.text = $"보유 함선 수 : {ship.currentHave_Ship}";
                shipBuildSlider.slider.maxValue = ship.maxHaveShip_Amount - ship.currentHave_Ship;
                shipBuildSlider.slider.value = 0;
                shipBuildSlider.building_Amount.text = $"<       {shipBuildSlider.slider.value}     /     {shipBuildSlider.slider.maxValue}       >";
                break;
        }
    }

    public void UpgradeConfirm_Or_Cancle()
    {
        confirm = !confirm;

        Build_Manager.instance.Building_Confirm(
            transform.GetChild(1).GetComponent<Image>().sprite,
            tabs == Tabs.Tab4 ? (int)shipBuildSlider.slider.value : 0,
            this,
            confirm);
    }

    //건물 건설시간 감소 로직 (적용확인 중)
    public void Ability_Apply()
    {
        int timer = Mathf.CeilToInt(Build_Manager.instance.TimerCalculation(tabs, buildResource.building_Time[buildResource.level]));
        foreach (var tt in timeText)
        {
            tt.text = $"{TimerTexting(timer)}";
        }

    }

    void OverlapTextCode(Base_Infomation info)
    {
        string nameText = "";
        int metal = 0;
        int cristal = 0;
        int gas = 0;
        int allowableBuild = 0;
        switch (info.tabs)
        {
            case Tabs.Tab1:
            case Tabs.Tab2:
            case Tabs.Tab5:
                buildResource.level++;
                nameText = $"Lv.{buildResource.level} {buildResource.name.Split('.')[1]}";

                metal = buildResource.init_Needs[0];
                cristal = buildResource.init_Needs[1];
                gas = buildResource.init_Needs[2];

                for (int i = 0; i < buildResource.level + 1; i++) // 왜 1을 더 해줘야 할까 확인할것
                {
                    metal = Mathf.FloorToInt(metal * buildResource.build_require[i]);
                    cristal = Mathf.FloorToInt(cristal * buildResource.build_require[i]);
                    gas = Mathf.FloorToInt(gas * buildResource.build_require[i]);
                    if (info.tabs == Tabs.Tab5)
                    {
                        allowableBuild = allowableBuild + (int)buildResource.build_result[i];
                        resources[3].text = $"{allowableBuild}";
                        buildResource.AllowableBuild = allowableBuild;
                    }
                }

                foreach (TextMeshProUGUI tt in timeText)
                {
                    /*tt.text = $"{TimerTexting(buildResource.building_Time[buildResource.level])}";*/
                    /*tt.text = $"{TimerTexting((int)Build_Manager.instance.TimerCalculation(tabs, buildResource.building_Time[buildResource.level]))}";
                    Debug.Log($"시간표기: {TimerTexting((int)Build_Manager.instance.TimerCalculation(tabs, buildResource.building_Time[buildResource.level]))}");*/
                }
                break;
            case Tabs.Tab3:
                research.level++;
                nameText = $"Lv.{research.level} {research.name.Split('.')[1]}";

                metal = research.research_Cost[0];
                cristal = research.research_Cost[1];
                gas = research.research_Cost[2];
                for (int i = 0; i < research.level + 1; i++) // 왜 1을 더 해줘야 할까 확인할것
                {
                    metal = Mathf.FloorToInt(metal * research.upgrade_Cost_Require[i]);
                    cristal = Mathf.FloorToInt(cristal * research.upgrade_Cost_Require[i]);
                    gas = Mathf.FloorToInt(gas * research.upgrade_Cost_Require[i]);
                }

                foreach (TextMeshProUGUI tt in timeText)
                {
                    tt.text = $"{TimerTexting(research.research_Time[research.level])}";
                }
                break;
        }

        title_Text["name"].text = info.tabs == Tabs.Tab1 || info.tabs == Tabs.Tab2 || info.tabs == Tabs.Tab3 ? nameText : nameText + " 관제센터";

        resources[0].text = $"{metal}";
        resources[1].text = $"{cristal}";
        resources[2].text = $"{gas}";

        buildResource.cur_Needs[0] = metal;
        buildResource.cur_Needs[1] = cristal;
        buildResource.cur_Needs[2] = gas;

        if (info.tabs == Tabs.Tab1)
        {
            switch (buildResource.resource_Factory)
            {
                case BuildResource.Resource_Factory.Metal:
                case BuildResource.Resource_Factory.Cristal:
                case BuildResource.Resource_Factory.Gas:
                    buildResource.electricity_Consumption = Manufacture_Conversion(buildResource) / 10;

                    production[0].text = $"{Manufacture_Conversion(buildResource)}";
                    production[1].text = $"{buildResource.electricity_Consumption}";

                    // 리소스 매니져에 자원 생성량 전달 해야함.
                    ResourceManager.instance.resource_Productions[(int)buildResource.resource_Factory]
                        = Manufacture_Conversion(buildResource);
                    break;
            }
        }
        else if (info.tabs == Tabs.Tab2)
        {
            production[0].text = $"{buildResource.buildAbility * buildResource.level}%";
            production[1].text = $"{buildResource.electricity_Consumption * buildResource.level}";
        }
        else if (info.tabs == Tabs.Tab3)
        {
            production[0].text = $"{research.research_Ability * research.level}%";
        }
        else if (info.tabs == Tabs.Tab5)
        {
            ship.maxHaveShip_Amount = allowableBuild;
            var imgGroup = Build_Manager.instance.containerSlide_Group;
            for (int i = 0; i < imgGroup.controlCenter_Tab.Count; i++)
            {
                if (imgGroup.controlCenter_Tab[i] == info.containerSlide)
                {
                    imgGroup.build_Tab4[i].Infomation.shipBuildSlider.
                        building_Amount.text =
                        $"<       {imgGroup.build_Tab4[i].Infomation.shipBuildSlider.slider.value}     /     {ship.maxHaveShip_Amount - ship.currentHave_Ship}       >";
                    imgGroup.build_Tab4[i].Infomation.shipBuildSlider.slider.maxValue
                        = ship.maxHaveShip_Amount - ship.currentHave_Ship;
                    imgGroup.build_Tab4[i].Infomation.shipBuildSlider.slider.value
                        = ship.maxHaveShip_Amount - ship.currentHave_Ship >= 1 ? 1 : 0;
                    break;
                }
            }
        }

    }



    public virtual int Supplement_Allowable()
    {
        // 추가 능력치 관련
        return 0;
    }

    internal string TimerTexting(int timer)
    {
        if (timer >= 3600)
            return $"{timer / 3600}시간 {(timer % 3600) / 60}분 {timer % 60}초";
        else if (timer >= 60)
            return $"{timer / 60}분 {timer % 60}초";
        else
            return $"{timer}초";
    }
    public void Init_DataReset()
    {
        buildResource.AllowableBuild = 0;
        buildResource.level = 0;
    }

    // 자원 생산량 1자릿수는 버림처리하기 위한 로직
    internal int Manufacture_Conversion(BuildResource build)
    {
        // build.manufacture[0]은 basic과 같은 기능으로 항상 1번 인덱스의 값만 덮어씌어지도록 설계
        build.manufacture[1] = build.manufacture[0];

        for (int i = 0; i < build.level; i++)
        {
            build.manufacture[1] = (int)(build.manufacture[1] * build.magnification);
        }
        int remainder = build.manufacture[1] % 10;

        int value = 0;
        if (build.level > 1)
        {
            value = build.manufacture[1] - remainder;
        }
        else if (build.level == 1)
        {
            value = build.basic_manufacture;
        }

        return value;
    }
}
