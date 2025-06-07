using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class Titles : SerializableDictionary<string, TextMeshProUGUI> { };

public class Infomations : MonoBehaviour
{

    public enum Types { ControlCenter, Tab3 }
    public Types info_types;
    // 5.29일 예제로 우선 해보는것
    public Init_SettingScriptable init_SettingScriptable;
    public ShipBuildSlider shipBuildSlider;
    //위쪽까지가 예제임



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

    // 추후 서버 데이터를 연결한 이후 데이터 연동 확인 작업 후 본인의 계정의
    // 정보를 불러오기 앞서, 현재는 연동 데이터가 없으므로 항상 레벨 0로 초기화하는데 필요한 bool값
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

        // 추후 서버 데이터를 연결한 이후 데이터 연동 확인 작업 후 본인의 계정의
        // 정보를 불러오기 앞서, 현재는 연동 데이터가 없으므로 항상 레벨 1로 초기화하는데 필요한 bool값
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

                // 임시 추가가능대수 변수
                int addnum = 0;

                resources[3].text = $"{buildResource.AllowableBuild} (+{addnum})";
                resources[4].text = $"생산 가능 {buildResource.name}"; // 생산 가능 함선 종류


                foreach (TextMeshProUGUI tt in timeText)
                {
                    tt.text = $"{TimerTexting(buildResource.building_Time[buildResource.level])}";
                }
                titles["name"].text = $"Lv.{buildResource.level} {buildResource.name} 관제센터";
            }
            else // 함선생산 Infomations
            {
                Debug.Log("ships참조");

                timeText[0].text = $"{TimerTexting((int)ships.shipMaking_Time)}";
                titles["name"].text = $"{ships.name}";
            }
        }


        


        /*int metal = buildResource.init_Needs[0];
        int cristal = buildResource.init_Needs[1];
        int gas = buildResource.init_Needs[2];*/
        

        // 임시 추가가능대수 변수
        /*int addnum = 0;*/
        resources[0].text = $"{metal}";
        resources[1].text = $"{cristal}";
        resources[2].text = $"{gas}";
        

        btns[1].gameObject.SetActive(false);
        timeSlider[0].gameObject.SetActive(false);

        // 해금 상태 확인여부 하여 버튼 잠금 확인 로직
        UnLockCheck(unLock);
    }


    public void UnLockCheck(bool unlock) // 해금 상태에 따른 로직 처리
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

                titles["name"].text = $"Lv.{buildResource.level} {buildResource.name} 관제센터";

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
        for (int i = 0; i < buildResource.level+1; i++) // 왜 1을 더 해줘야 할까 확인할것
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

        string timeStr = TimerTexting(buildResource.building_Time[buildResource.level]);

        foreach (TextMeshProUGUI tt in timeText)
        {
            tt.text = $"{timeStr}";
        }*/
    }

    public string TimerTexting(int timer)
    {
        if (timer >= 3600)
            return $"{timer / 3600}시간 {(timer % 3600) / 60}분 {timer % 60}초";
        else if (timer >= 60)
            return $"{timer / 60}분 {timer % 60}초";
        else 
            return $"{timer}초";
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
