using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ResearchCategory_Container : MonoBehaviour
{
    public GameObject[] researchCategory_Window;
    public Button[] researchCategory_Btns;

    void Awake()
    {
        researchCategory_Btns = new Button[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            researchCategory_Btns[i] = transform.GetChild(i).GetComponent<Button>();
        }

        researchCategory_Window = transform.parent.GetComponentsInChildren<ScrollRect>(true)
            .Select(sr => sr.gameObject).ToArray();
    }

    void OnEnable()
    {
        for (int i = 0; i < researchCategory_Window.Length; i++)
        {
            if (researchCategory_Window[i].activeInHierarchy)
            {
                Button_SelectedColor(i);
                EventSystem.current.SetSelectedGameObject(researchCategory_Btns[i].gameObject);
                break;
            }
        }
    }

    public void ResearchCategorySelect(int num)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (num == i)
            {
                researchCategory_Window[i].gameObject.SetActive(true);
            }
            else
            {
                researchCategory_Window[i].gameObject.SetActive(false);
                ScrollRect scroll = researchCategory_Window[i].GetComponent<ScrollRect>();
                int index = scroll.content.transform.childCount;
                for (int ii = 0; ii < index; ii++)
                {
                    ContainerSlide imgsl = scroll.content.GetChild(ii).GetComponent<ContainerSlide>();
                    if (imgsl.imgOpen && imgsl.confirm == false)
                    {
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
