using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class ShipSelection : MonoBehaviour
{
    public GameObject selectedFrame;
    public GameObject extendableContainer;

    public GameObject[] childContainers;
    public FleetSelectForMission[] fleetSelect;

    public int[] shipsCount;


    GameObject curBtn, pastBtn;
    Slider slider;
    ScrollRect scrollRect;

    private void Awake()
    {
        scrollRect = GetComponentInChildren<ScrollRect>();
        int len = scrollRect.content.transform.childCount;
        childContainers = new GameObject[len];
        for (int i = 0; i < len; i++)
        {
            childContainers[i] = scrollRect.content.GetChild(i).gameObject;
        }

        fleetSelect = scrollRect.content.GetComponentsInChildren<FleetSelectForMission>();

        shipsCount = new int[fleetSelect.Length];

        for (int i = 0; i < fleetSelect.Length; i++)
        {

            fleetSelect[i].GetComponent<Image>().sprite = fleetSelect[i].ship.img;
            fleetSelect[i].transform.GetChild(0).gameObject.SetActive(false);
        }
        slider = extendableContainer.GetComponentInChildren<Slider>();
    }


    public void Btn_Selected()
    {
        curBtn = EventSystem.current.currentSelectedGameObject;
        FleetSelectForMission FSM;
        if (pastBtn == curBtn)
        {
            return;
        }
        else
        {
            if (pastBtn != null)
            {
                FSM = pastBtn.GetComponent<FleetSelectForMission>();
                for (int i = 0; i < fleetSelect.Length; i++) // 10 하드코딩 수정할것.
                {
                    if (FSM == fleetSelect[i])
                    {
                        if (slider.value > 0)
                        {
                            pastBtn.transform.GetChild(0).gameObject.SetActive(true);
                            shipsCount[i] = (int)slider.value;
                        }
                        else
                        {
                            pastBtn.transform.GetChild(0).gameObject.SetActive(false);
                            shipsCount[i] = 0;
                        }
                        break;
                    }
                }
            }   
        }

        selectedFrame.transform.SetParent(curBtn.transform);
        selectedFrame.transform.position = curBtn.transform.position;
        pastBtn = curBtn;

        int num = 0;
        for (int i = 0; i < childContainers.Length; i++)
        {
            if (curBtn.transform.parent.gameObject == childContainers[i])
            {
                num = i;
                break;
            }
        }

        FSM = curBtn.GetComponent<FleetSelectForMission>();
        slider.maxValue = FSM.ship.currentHave_Ship;
        for (int i = 0; i < 10; i++)
        {
            
            if (FSM == fleetSelect[i])
            {
                if (shipsCount[i] > 0)
                {
                    slider.value = shipsCount[i];
                }
                else
                {
                    slider.value = 0f;
                }
                break;
            }
        }

        extendableContainer.transform.SetParent(curBtn.transform.parent.parent);
        extendableContainer.transform.SetSiblingIndex(num + 1);
        extendableContainer.GetComponent<Animator>().SetTrigger("Open");

        if (num == childContainers.Length - 1)
        {
            StartCoroutine(ScrollViewExtendable());
        }
    }

    IEnumerator ScrollViewExtendable()
    {

        float start = scrollRect.verticalNormalizedPosition;
        float duration = 0.15f;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / duration;
            scrollRect.verticalNormalizedPosition = Mathf.Lerp(start, 0, t);
            yield return null;
        }
        scrollRect.verticalNormalizedPosition = 0f;
    }

}
