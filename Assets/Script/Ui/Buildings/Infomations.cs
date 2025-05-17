using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class Titles : SerializableDictionary<string, TextMeshProUGUI> { };

public class Infomations : MonoBehaviour
{
    public Titles titles;
    public TextMeshProUGUI[] infos;
    /*  0: lv,
        1: upgrading time,
        2:
    
     */
    public TextMeshProUGUI[] resources = new TextMeshProUGUI[3];
    public BuildResource buildResource;
    public Slider slider;
    public TextMeshProUGUI timeText;
    public BuildDetailMatter buildDetailMatter;

    void Start()
    {
        
    }

    public void Upgrade()
    {
        int curLv = buildResource.level;

        int metal = Mathf.FloorToInt(buildResource.init_Needs[0] * buildResource.build_require[1]);
        int cristal = Mathf.FloorToInt(buildResource.init_Needs[1] * buildResource.build_require[1]);
        int gas = Mathf.FloorToInt(buildResource.init_Needs[2] * buildResource.build_require[1]);

        resources[0].text = metal.ToString();
        resources[1].text = cristal.ToString();
        resources[2].text = gas.ToString();

        curLv += 1;
        buildResource.level = curLv;
        titles["name"].text = $"Lv. {curLv} {buildResource.name}";
    }


}
