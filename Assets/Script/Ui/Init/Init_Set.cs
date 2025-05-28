using UnityEngine;

public class Init_Set : MonoBehaviour
{
    public GameObject[] category_Windows;
    public GameObject[] build_tab1;
    public GameObject[] build_tab2;
    public GameObject[] research_tab;
    public GameObject[] controlCenter_tab;

    void Awake()
    {
        foreach(GameObject obj in category_Windows)
        {
            obj.SetActive(true);
        }

        foreach (GameObject obj in controlCenter_tab)
        {
            obj.SetActive(true);
        }
    }

    void Start()
    {
        
        foreach (GameObject obj in category_Windows)
        {
            obj.SetActive(false);
        }

        for (int i = 1; i < controlCenter_tab.Length; i++)
        {
            controlCenter_tab[i].SetActive(false);
        }
        
    }
}
