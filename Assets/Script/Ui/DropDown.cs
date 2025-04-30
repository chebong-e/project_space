using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class DropDown : MonoBehaviour
{
    public GameObject[] dropDown_List;
    public GameObject[] frame_Lines;
    public AnimationClip[] clip;
    public int triggered_event;
    public List<GameObject> eventList;

    RectTransform rectTransform;
    GridLayoutGroup dropDown_Box;
    Animator anim;
    int windowSize;

    private int dropdown_Box_Y_Size = 130;
    private int dropdown_Frame_Size = 3;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        dropDown_Box = transform.GetChild(0).GetComponent<GridLayoutGroup>();
        anim = GetComponent<Animator>();
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
        FrameLineActiveReset();

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

    void EventWindowActivate() // �̺�Ʈ �߻��� â�� ���
    {
        // �ð������� ����ġ�� �ξ� ���� ���� �Ͼ �̺�Ʈ�� �׻� �������� ǥ���ϵ��� �ؾ� ��.

        foreach (GameObject window in dropDown_List)
        {
            window.SetActive(false);
        }

        for (int i = 0; i < triggered_event; i++)
        {
            dropDown_List[i].SetActive(true);
        }

        FrameLineActiveReset();
        frame_Lines[triggered_event - 1].SetActive(false);
        
    }

    void Event_Trigger() // �̺�Ʈ �߻��� �׿� ���� ������
    {
        // ����ġ�� Ȯ���Ͽ� ���� ����
        if (triggered_event > 1)
        {

        }
    }

    public void Btn_DropDown()
    {
        if (triggered_event < 1)
        {
            return;
        }
        // �̺�Ʈ ���� �°� ������ ũ�� �����ϰ� �̺�Ʈ ���

        EventWindowActivate();
        windowSize = (triggered_event * dropdown_Box_Y_Size) + ((triggered_event - 1) * dropdown_Frame_Size);


        // 1. Clip ����
        AnimationClip newClip = Instantiate(clip[0]);
        newClip.name = "ModifiedClip";

        // 2. ���ε� ��������
        var bindings = AnimationUtility.GetCurveBindings(newClip);

        foreach (var binding in bindings)
        {
            if (binding.propertyName == "m_SizeDelta.y")
            {
                AnimationCurve curve = AnimationUtility.GetEditorCurve(newClip, binding);

                if (curve.length > 1)
                {
                    Keyframe lastKey = curve[curve.length - 1];
                    lastKey.value = windowSize;
                    curve.MoveKey(curve.length - 1, lastKey);
                    AnimationUtility.SetEditorCurve(newClip, binding, curve);
                }
            }
        }

        // 3. Animator Override Controller ���
        AnimatorOverrideController overrideController = new AnimatorOverrideController(anim.runtimeAnimatorController);
        overrideController["DropDownClip"] = newClip;
        anim.runtimeAnimatorController = overrideController;

        // 4. Ʈ���� ����
        anim.SetTrigger("drop");
    }

    public void Btn_DropUp()
    {
        // 1. Clip ����
        AnimationClip newClip = Instantiate(clip[1]);
        newClip.name = "ModifiedClip_1";

        // 2. ���ε� ��������
        var bindings = AnimationUtility.GetCurveBindings(newClip);

        foreach (var binding in bindings)
        {
            if (binding.propertyName == "m_SizeDelta.y")
            {
                AnimationCurve curve = AnimationUtility.GetEditorCurve(newClip, binding);

                Keyframe firstKey = curve[0];
                firstKey.value = windowSize;
                curve.MoveKey(0, firstKey);
                AnimationUtility.SetEditorCurve(newClip, binding, curve);
            }
        }

        // 3. Animator Override Controller ���
        AnimatorOverrideController overrideController = new AnimatorOverrideController(anim.runtimeAnimatorController);
        overrideController["DropUpClip"] = newClip;
        anim.runtimeAnimatorController = overrideController;

        // 4. Ʈ���� ����
        anim.SetTrigger("up");
    }
}
