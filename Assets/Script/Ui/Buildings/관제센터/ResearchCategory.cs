using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ResearchCategory : MonoBehaviour
{
    public GameObject[] research_Window;
    public Button[] researchCategory_Btns;

    void Awake()
    {
        researchCategory_Btns = new Button[transform.childCount];
        for (int i = 0; i < researchCategory_Btns.Length; i++)
        {
            researchCategory_Btns[i] = transform.GetChild(i).GetComponent<Button>();
        }

        research_Window = transform.parent.GetComponentsInChildren<ScrollRect>(true)
            .Select(sr => sr.gameObject).ToArray();

    }

    void OnEnable()
    {
        for (int i = 0; i < research_Window.Length; i++)
        {
            if (research_Window[i].activeInHierarchy)
            {
                Button_SelectedColor(i);
                EventSystem.current.SetSelectedGameObject(research_Window[i].gameObject);
                break;
            }
        }
    }

    public void GradeSelect(int num)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (num == i)
            {
                research_Window[i].gameObject.SetActive(true);
                //if (Build_Manager.instance.tab4)
                //{
                //    Build_Manager.instance.KeepOpenState(3);
                //}
            }
            else
            {
                research_Window[i].gameObject.SetActive(false);
                ScrollRect scroll = research_Window[i].GetComponent<ScrollRect>();
                int index = scroll.content.transform.childCount;
                for (int ii = 0; ii < index; ii++)
                {
                    ContainerSlide imgsl = scroll.content.GetChild(ii).GetComponent<ContainerSlide>();
                    if (imgsl.imgOpen && imgsl.confirm == false)
                    {
                        Debug.Log(imgsl.name);
                        imgsl.imgOpen = false;
                        imgsl.Slider_On_Off();
                    }
                }

            }
        }
        Button_SelectedColor(num);
    }




    void Button_SelectedColor(int num)
    {
        Color32 yellow = new Color32(255, 255, 0, 255);
        Color32 white = new Color32(255, 255, 255, 255);

        for (int i = 0; i < transform.childCount; i++)
        {
            Button btn = researchCategory_Btns[i];
            ColorBlock cb = btn.colors;

            if (num == i)
            {
                cb.normalColor = Color.yellow;
                cb.highlightedColor = Color.white;
                cb.selectedColor = Color.yellow;
                cb.pressedColor = Color.gray;
                cb.disabledColor = Color.white;
                cb.selectedColor = yellow;
                btn.colors = cb;
            }
            else
            {
                cb.normalColor = white;
                cb.highlightedColor = white;
                cb.selectedColor = white;
                cb.pressedColor = Color.gray;
                cb.disabledColor = white;
                cb.selectedColor = white;
                btn.colors = cb;
            }
        }

    }
}
