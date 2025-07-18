using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShipGrade_Container : MonoBehaviour
{
    public GameObject[] shipGrade_Window;
    public Button[] shipGrade_Btns;

    void Awake()
    {
        shipGrade_Btns = new Button[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            shipGrade_Btns[i] = transform.GetChild(i).GetComponent<Button>();
            /*// 스크립트로 버튼의 인덱스 값을 알아내는 방법
            int capturedIndex = i;
            shipGrade[i].GetComponent<Button>().onClick.AddListener(() => SelfObject_Check(shipGrade[capturedIndex]));*/
        }

        shipGrade_Window = transform.parent.GetComponentsInChildren<ScrollRect>(true)
            .Select(sr => sr.gameObject).ToArray();
    }

    void OnEnable()
    {
        for (int i = 0; i < shipGrade_Window.Length; i++)
        {
            if (shipGrade_Window[i].activeInHierarchy)
            {
                Button_SelectedColor(i);
                EventSystem.current.SetSelectedGameObject(shipGrade_Btns[i].gameObject);
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
                shipGrade_Window[i].gameObject.SetActive(true);
                shipGrade_Window[i].GetComponent<Scriptable_Matching>().Infomation_UpdateSet();
                if (Build_Manager.instance.tab4)
                {
                    Build_Manager.instance.KeepOpenState(3);
                }
            }
            else
            {
                shipGrade_Window[i].gameObject.SetActive(false);
                ScrollRect scroll = shipGrade_Window[i].GetComponent<ScrollRect>();
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
            Button btn = shipGrade_Btns[i];
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

    /*IEnumerator Init_WindowClose()
    {
        yield return new WaitForSeconds(0.01f);
        for (int i = 1; i < shipGrade_Window.Length; i++)
        {
            shipGrade_Window[i].SetActive(false);
        }
        yield return new WaitForSeconds(0.01f);
        Build_Manager.instance.Tab_Close();
    }*/
}
