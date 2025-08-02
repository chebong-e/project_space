using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Base_Planet_TabInfomations : MonoBehaviour
{
    public enum Tabs { Tab1, Tab2, Tab3, Tab4, Tab5 }
    
    public Tabs tabs;
    public TextMeshProUGUI planet_coordinate;
    public Image[] icons;
    public TextMeshProUGUI planetName;
    public GameObject[] debris; // 0Àº ÀÜÇØ ¾øÀ½, 1Àº ÀÜÇØ ¹ß»ý »óÈ²
    public TextMeshProUGUI[] debrisInResource; // 0Àº ¸ÞÅ», 1Àº Å©¸®½ºÅ»
    public GameObject[] grade;

    public virtual void Init_Set()
    {
        debris = new GameObject[2];
        debrisInResource = new TextMeshProUGUI[2];

        /*SelectedTab();*/

        VariableMatching();

    }

    /*protected virtual void SelectedTab()
    {

    }*/

    protected virtual void VariableMatching()
    {

    }

    public void StarGradeSelect(int grade)
    {


        foreach (Image img in icons)
        {
            img.gameObject.SetActive(false);
        }
        icons[0].gameObject.SetActive(true);
        for (int i = 1; i < grade + 1; i++)
        {
            icons[i].gameObject.SetActive(true);
        }

        if (grade > 1 && grade < 4)
        {
            planetName.color = Color.yellow;
        }
        else if (grade > 3)
        {
            planetName.color = Color.magenta;
        }
        else
        {
            planetName.color = Color.white;
        }

    }
}
