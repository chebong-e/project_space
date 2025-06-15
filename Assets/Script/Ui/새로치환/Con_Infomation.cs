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
    public TextMeshProUGUI[] resources; // 자원텍스트관련
    public TextMeshProUGUI[] timeText; // 경과시간 텍스트
    public TextMeshProUGUI[] amount_Text; // 생산 가능 수량
    public BuildResource buildResource;
    public Ships ships;
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

                case Types.Tab3:
                    Debug.Log("ships참조");

                    timeText[0].text = $"{TimerTexting((int)ships.shipMaking_Time)}";
                    title_Text["name"].text = $"{ships.name}";
                    child_InfoContainer.transform.GetChild(0).gameObject.SetActive(false);
                    break;
            }
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
                ships.maxHaveShip_Amount = allowableBuild;
                // 해당 인포메이션 스크립트에는 쉽빌드슬라이더 스크립트가 없으므로 참조 되지 않음
                // 어차피 탭의 인덱스 값은 같으니 인덱스 값을 활용하여 업데이트 해주도록 하자.
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
            return $"{timer / 3600}시간 {(timer % 3600) / 60}분 {timer % 60}초";
        else if (timer >= 60)
            return $"{timer / 60}분 {timer % 60}초";
        else
            return $"{timer}초";
    }
}
