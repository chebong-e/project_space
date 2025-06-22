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
    public TextMeshProUGUI[] resources; // 자원텍스트관련
    public TextMeshProUGUI[] timeText; // 경과시간 텍스트
    public TextMeshProUGUI[] amount_Text; // 생산 가능 수량
    public TextMeshProUGUI haveShipCount;
    public BuildResource buildResource;
    public Ship ship;
    public Slider[] timeSlider; // 경과시간에 따른 표시할 슬라이더
    public Slider amountSlider; // 생산할 함선 수량 관련
    public Button[] btns; // 업그레이드, 업그레이드 취소 버튼
    public GameObject child_InfoContainer;

    public ContainerSlide containerSlide;

    /* 추후 서버 데이터를 연결한 이후 데이터 연동 확인 작업 후 본인의 계정의 정보를 불러오기 앞서,
     * 현재는 연동 데이터가 없으므로 항상 레벨 0로 초기화하는데 필요한 bool값*/
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

        // 서버데이터가 없음으로 처리로 완전 0부터 시작으로 초기화
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

                // 추가가능대수(보너스 및 연구 등) = 임시
                int plusAllowable = 0;
                resources[3].text = $"{buildResource.AllowableBuild} (+{plusAllowable})";
                resources[4].text = $"생산 가능 {buildResource.name}"; // 생산 가능 함선 종류

                foreach (TextMeshProUGUI tt in timeText)
                {
                    tt.text = $"{TimerTexting(buildResource.building_Time[buildResource.level])}";
                }
                title_Text["name"].text = $"Lv.{buildResource.level} {buildResource.name} 관제센터";
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

        // 업그레이드 중이 아니라면 아래 두 오브젝트는 변수할당 후 비활성화 처리가 default
        btns[1].gameObject.SetActive(false); 
        timeSlider[0].gameObject.SetActive(false);

        // 해금 상태 확인여부 하여 버튼 잠금 확인 로직
        UnLockCheck(unLock);
    }

    
    void UnLockCheck(bool unlock) // 해금 상태에 따른 로직 처리
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
                haveShipCount.text = $"보유 함선 수 : {ship.currentHave_Ship}";
                shipBuildSlider.slider.maxValue = ship.maxHaveShip_Amount - ship.currentHave_Ship;
                shipBuildSlider.slider.value = 0;
                shipBuildSlider.building_Amount.text = $"<       {shipBuildSlider.slider.value}     /     {shipBuildSlider.slider.maxValue}       >";
                break;
            case Types.ControlCenter:
                buildResource.level++;
                title_Text["name"].text = $"Lv.{buildResource.level} {buildResource.name} 관제센터";

                int metal = buildResource.init_Needs[0];
                int cristal = buildResource.init_Needs[1];
                int gas = buildResource.init_Needs[2];
                int allowableBuild = 0;
                for (int i = 0; i < buildResource.level + 1; i++) // 왜 1을 더 해줘야 할까 확인할것
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

                // 임시 사항. 나중에 로직으로 빼두던지 해야할듯
                // 윗열 슬라이더 시간 텍스트 표시 관련임 
                foreach (TextMeshProUGUI tt in timeText)
                {
                    tt.text = $"{Build_Manager.instance.TimerTexting(buildResource.building_Time[buildResource.level])}";
                }

                // 함선생산 탭의 최대 함선 생산 수량 업데이트
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
}
