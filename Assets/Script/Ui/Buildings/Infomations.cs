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
    public Button[] btns = new Button[2];

    void Start()
    {
        if (buildResource != null)
            titles["name"].text = string.Format(buildResource.Lv_Title, buildResource.level);
    }

    public void Upgrade()
    {
        int curLv = buildResource.level;

        for (int i = 0; i < buildResource.cur_Needs.Length; i++)
        {
            buildResource.cur_Needs[i] = Mathf.FloorToInt(buildResource.cur_Needs[i] * buildResource.build_require[buildResource.level]);
            resources[i].text = buildResource.cur_Needs[i].ToString();
        }

        curLv += 1;
        buildResource.level = curLv;
        titles["name"].text = $"Lv. {curLv} {buildResource.name}";
    }


}
