using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Containers : SerializableDictionary<GameObject, Groups> { };

[System.Serializable]
public class Groups
{
    public GameObject[] objs;
}
public class Init_Matching : MonoBehaviour
{
    public Containers containers;
    public GameObject[] controlCenter_Content;
    public GameObject[] buildShip_Content;

    // ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ
    public Infomations[] control_Container;
    public ShipBuildSlider[] ship_Container;





    void Start()
    {

        // 인포메이션 init을 해준 뒤에 여기 init을 실행해야함.


        Init();

    }


    public void Init()
    {
        for (int i = 0; i < controlCenter_Content.Length; i++)
        {
            ScrollRect control_scr = controlCenter_Content[i].GetComponent<ScrollRect>();
            ScrollRect build_scr = buildShip_Content[i].GetComponent<ScrollRect>();
            control_Container = control_scr.content.GetComponentsInChildren<Infomations>(true);
            ship_Container = build_scr.content.GetComponentsInChildren<ShipBuildSlider>(true);
            int index = control_scr.content.childCount;
            for (int ii = 0; ii < index; ii++)
            {
                control_Container[ii].shipBuildSlider = ship_Container[ii];
            }
        }
    }
}
