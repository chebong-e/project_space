using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class DropDown : MonoBehaviour
{
    public GameObject[] dropDown_List;
    public GameObject[] sorting_DropDownList;
    public GameObject[] frame_Lines;
    public AnimationClip[] clip;
    public int triggered_event;
    public List<GameObject> eventList;
    public TextMeshProUGUI[] targetTexts;
    public Button[] dropBtns;

    RectTransform rectTransform;
    GridLayoutGroup dropDown_Box;
    Animator anim;
    AnimationClip[] reusableClip;
    int windowSize;

    private int dropdown_Box_Y_Size = 130;
    private int dropdown_Frame_Size = 3;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        dropDown_Box = transform.GetChild(0).GetComponent<GridLayoutGroup>();
        anim = GetComponent<Animator>();
        targetTexts = new TextMeshProUGUI[2];
        reusableClip = new AnimationClip[1];
        sorting_DropDownList = new GameObject[10];
    }

    void Init()
    {
        int active_child_list = GetActiveChildCount(transform.GetChild(0));
        int dropdownList = transform.GetChild(0).childCount;

        // DropDown ��� ������Ʈ ����
        dropDown_List = new GameObject[dropdownList];
        frame_Lines = new GameObject[dropdownList];
        for (int i = 0; i < dropdownList; i++)
        {
            dropDown_List[i] = transform.GetChild(0).GetChild(i).gameObject;
            sorting_DropDownList[i] = dropDown_List[i];
        }
        for (int i = 0; i < frame_Lines.Length; i++)
        {
            frame_Lines[i] = dropDown_List[i].transform.GetChild(0).gameObject;
        }

        // �׻� ������ ������ �������� ��Ȱ��ȭ �Ǿ�� ��.
        frame_Lines[active_child_list - 1].gameObject.SetActive(false);

        // �ʱ� Box ũ�� ����
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 0);
        dropDown_Box.cellSize = new Vector2(rectTransform.rect.width, dropdown_Box_Y_Size);

        // �̺�Ʈ �ڽ��� ���� ������Ʈ �ʱ�ȭ
        /*FrameLineActiveReset();*/

        for (int i = 0; i < dropDown_List.Length; i++)
        {
            GridLayoutGroup grid = dropDown_List[i].transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<GridLayoutGroup>();
            grid.cellSize = new Vector2(600, 90);
        }
    }
    void Start()
    {
        // Ȱ��ȭ �ڽ� ���� Ȯ��(���� �������� ����� �������ų� ����� �����͸� �ҷ��ͼ� Ȯ���ϴ� ������� �ٲ������)
        // �̺�Ʈ ���� Ȯ�� ���� 


        // �ʱ� ����
        Init();

    }

    public int GetActiveChildCount(Transform parent)
    {
        int count = 0;

        foreach(Transform child in parent)
        {
            if (child.gameObject.activeInHierarchy)
            {
                count++;
            }
        }
        return count;
    }

    void FrameLineActiveReset() // ��� �����Ӷ����� ������Ʈ�� Ȱ��ȭ
    {
        foreach(GameObject line in frame_Lines)
        {
            if (!line.activeSelf)
            {
                line.SetActive(true);
            }
        }
    }

    public void EventWindowActivate() // �̺�Ʈ �߻��� â�� ���
    {
        // �ð������� ����ġ�� �ξ� ���� ���� �Ͼ �̺�Ʈ�� �׻� �������� ǥ���ϵ��� �ؾ� ��.

        foreach (GameObject window in dropDown_List)
        {
            window.SetActive(false);
        }

        for (int i = 0; i < triggered_event; i++)
        {
            /*// 7-18Ȯ��
            dropDown_List[i].GetComponent<EventLine>().event_Triggered.isUsed = true;*/

            sorting_DropDownList[i].gameObject.SetActive(true);
        }
        
    }

    public void SlideEventImplement(int index)
    {

        windowSize = (triggered_event * dropdown_Box_Y_Size) + ((triggered_event - 1) * dropdown_Frame_Size);
        EventWindowActivate();

        // 1. Clip ����
        if (reusableClip[0] == null)
        {
            reusableClip[0] = Instantiate(clip[0]);
            reusableClip[0].name = "ModifiedClip";
        }

        // 2. ���ε� ��������
        var bindings = AnimationUtility.GetCurveBindings(reusableClip[0]);

        foreach (var binding in bindings)
        {
            if (binding.propertyName == "m_SizeDelta.y")
            {
                AnimationCurve curve = AnimationUtility.GetEditorCurve(reusableClip[0], binding);

                Keyframe firstKey = curve[0];
                Keyframe lastKey = curve[curve.length - 1];

                if (index == 0) // down
                {
                    firstKey.value = 0;
                    lastKey.value = windowSize;
                }
                else if (index == 1) // up
                {
                    firstKey.value = windowSize;
                    lastKey.value = 0;
                }
                else if (index == 2) // Add_LineDropDown
                {
                    firstKey.value = ((triggered_event - 1) * dropdown_Box_Y_Size) + ((triggered_event - 2) * dropdown_Frame_Size);
                    lastKey.value = (triggered_event * dropdown_Box_Y_Size) + ((triggered_event - 1) * dropdown_Frame_Size);
                }
                else // ��ȯ������ �̺�Ʈ ����
                {
                    firstKey.value = ((triggered_event + 1) * dropdown_Box_Y_Size) + ((triggered_event - 2) * dropdown_Frame_Size);
                    lastKey.value = ((triggered_event) * dropdown_Box_Y_Size) + ((triggered_event - 2) * dropdown_Frame_Size);
                }

                curve.MoveKey(0, firstKey);
                curve.MoveKey(curve.length - 1, lastKey);

                AnimationUtility.SetEditorCurve(reusableClip[0], binding, curve);     
            }
        }

        // 3. Animator Override Controller ���
        AnimatorOverrideController overrideController = new AnimatorOverrideController(anim.runtimeAnimatorController);
        overrideController["DropDownClip"] = reusableClip[0];
        anim.runtimeAnimatorController = overrideController;

        // 4. Ʈ���� ����
        anim.SetTrigger("drop");
    }

    /*public void Btn_DropUp()
    {
        if (triggered_event < 1)
        {
            return;
        }
        // 1. Clip ����
        if (reusableClip[1] == null)
        {
            reusableClip[1] = Instantiate(clip[1]);
            reusableClip[1].name = "ModifiedClip_1";
        }

        // 2. ���ε� ��������
        var bindings = AnimationUtility.GetCurveBindings(reusableClip[1]);

        foreach (var binding in bindings)
        {
            if (binding.propertyName == "m_SizeDelta.y")
            {
                AnimationCurve curve = AnimationUtility.GetEditorCurve(reusableClip[1], binding);

                Keyframe firstKey = curve[0];
                firstKey.value = windowSize;
                curve.MoveKey(0, firstKey);
                AnimationUtility.SetEditorCurve(reusableClip[1], binding, curve);
            }
        }

        // 3. Animator Override Controller ���
        AnimatorOverrideController overrideController = new AnimatorOverrideController(anim.runtimeAnimatorController);
        overrideController["DropUpClip"] = reusableClip[1];
        anim.runtimeAnimatorController = overrideController;

        // 4. Ʈ���� ����
        anim.SetTrigger("up");
    }*/


}
