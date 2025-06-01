using System;
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
    }

    void Start()
    {
        for (int i = 1; i < shipGrade_Window.Length; i++)
        {
            shipGrade_Window[i].SetActive(false);
        }
    }


    void OnEnable()
    {
        int index = shipGrade_Window.Length;
        for (int i = 0; i < index; i++)
        {
            if (shipGrade_Window[i].activeInHierarchy)
            {
                Button_SelectedColor(i);
                EventSystem.current.SetSelectedGameObject(shipGrade_Btns[i].gameObject);
                break;
            }
        }

        
    }

    void SelfObject_Check(GameObject caller)
    {
        Debug.Log(caller.transform.GetSiblingIndex());
    }


    public void GradeSelect(int num)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (num == i)
            {
                shipGrade_Window[i].gameObject.SetActive(true);
                if (BuildManager.instance.upgrading) // 업그레이드가 진행중이라면
                {
                    //업그레이드 진행중인 화면 외의 다른 윈도우의 하위 그룹들의 이미지를 모두 흑백처리 및 버튼의 enable을 false처리 해야 함

                    //업그레이드 진행 중인 카테고리 인덱스 가져오기

                    //그리고 업그레이드가 비진행 중이라면 모든 윈도우의 하위 그룹의 이미지를 컬러전환 하고 버튼의 enable를 true처리
                }
            }
            else
            {
                shipGrade_Window[i].gameObject.SetActive(false);
                ScrollRect scroll = shipGrade_Window[i].GetComponent<ScrollRect>();
                int index = scroll.content.transform.childCount;
                for (int ii = 0; ii < index; ii++)
                {
                    ImageSlide imgsl = scroll.content.GetChild(ii).GetComponent<ImageSlide>();
                    if (imgsl.open)
                    {
                        imgsl.open = false;
                        imgsl.SliderOn_Off();
                    }
                }
                
            }
        }
        Button_SelectedColor(num);
    }

    public void Button_SelectedColor(int num)
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
}
