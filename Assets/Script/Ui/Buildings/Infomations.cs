using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class Titles : SerializableDictionary<string, TextMeshProUGUI> { };

public class Infomations : MonoBehaviour
{
    // 5.29일 예제로 우선 해보는것
    public Init_SettingScriptable init_SettingScriptable;
    public ShipBuildSlider shipBuildSlider;
    //위쪽까지가 예제임



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

    // 추후 서버 데이터를 연결한 이후 데이터 연동 확인 작업 후 본인의 계정의
    // 정보를 불러오기 앞서, 현재는 연동 데이터가 없으므로 항상 레벨 0로 초기화하는데 필요한 bool값
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

        // 여기서 이미지슬라이드 인잇세팅을 해주는게 좋을듯
        imgSlide = GetComponentInParent<ImageSlide>();
        imgSlide.Init_Setting();

        Init_Setting();
    }

    void Init_Setting()
    {
        // 추후 서버 데이터를 연결한 이후 데이터 연동 확인 작업 후 본인의 계정의
        // 정보를 불러오기 앞서, 현재는 연동 데이터가 없으므로 항상 레벨 1로 초기화하는데 필요한 bool값
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

        // 임시 추가가능대수 변수
        int addnum = 0;
        resources[0].text = $"{metal}";
        resources[1].text = $"{cristal}";
        resources[2].text = $"{gas}";
        if (buildResource.build_Category == BuildResource.Build_Category.ContorolCenter)
        {
            resources[3].text = $"{buildResource.AllowableBuild} (+{addnum})";

            // case로 구현하는게 좋으려나??
            resources[4].text = $"생산 가능 {buildResource.name}"; // 생산 가능 함선 종류
        }
        


        string timeStr = TimerTexting(buildResource.building_Time[buildResource.level]);
        /*int time = buildResource.building_Time[buildResource.level];
        if (time >= 3600)
        {
            int hours = time / 3600;
            int minutes = (time % 3600) / 60;
            int seconds = time % 60;
            timeStr = string.Format("{0}시간 {1}분 {2}초", hours, minutes, seconds);
        }
        else if (time >= 60)
        {
            int minutes = time / 60;
            int seconds = time % 60;
            timeStr = string.Format("{0}분 {1}초", minutes, seconds);
        }
        else
        {
            timeStr = string.Format("{0}초", time);
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
            titles["name"].text = $"Lv.{buildResource.level} {buildResource.name} 관제센터";
        }


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
        /*int time = buildResource.building_Time[buildResource.level];
        if (time >= 3600)
        {
            int hours = time / 3600;
            int minutes = (time % 3600) / 60;
            int seconds = time % 60;
            timeStr = string.Format("{0}시간 {1}분 {2}초", hours, minutes, seconds);
        }
        else if (time >= 60)
        {
            int minutes = time / 60;
            int seconds = time % 60;
            timeStr = string.Format("{0}분 {1}초", minutes, seconds);
        }
        else
        {
            timeStr = string.Format("{0}초", time);
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
            timeStr = string.Format("{0}시간 {1}분 {2}초", hours, minutes, seconds);
        }
        else if (time >= 60)
        {
            int minutes = time / 60;
            int seconds = time % 60;
            timeStr = string.Format("{0}분 {1}초", minutes, seconds);
        }
        else
        {
            timeStr = string.Format("{0}초", time);
        }

        return timeStr;
    }
}
