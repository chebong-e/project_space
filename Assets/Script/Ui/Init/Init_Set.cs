using UnityEngine;
using UnityEngine.UI;

public class Init_Set : MonoBehaviour
{
    public GameObject[] category_Windows;
    public GameObject[] build_tab1;
    public GameObject[] build_tab2;
    public GameObject[] build_tab3;
    public GameObject[] research_tab;
    public GameObject[] controlCenter_tab;

    void Awake()
    {
        foreach (GameObject obj in category_Windows)
        {
            obj.SetActive(true);
        }

        foreach (GameObject obj in controlCenter_tab)
        {
            obj.SetActive(true);
        }




        /*ScrollRect[] scroll = category_Windows[4].GetComponentsInChildren<ScrollRect>();
        controlCenter_tab = new GameObject[scroll.Length];
        for (int i = 0; i < scroll.Length; i++)
        {
            controlCenter_tab[i] = scroll[i].gameObject;
            Infomations[] infos = controlCenter_tab[i].GetComponentsInChildren<Infomations>(true);
            if (infos.Length > 0)
            {
                for (int ii = 0; ii < infos.Length; ii++)
                {
                    infos[ii].Init_Setting();
                }
            }
        }*/

    }

    

}
