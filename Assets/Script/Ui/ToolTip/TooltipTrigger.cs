using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public enum ResourceType { 메탈, 크리스탈, 가스 }
    public ResourceType resourceType;
    public string tooltipText;

    Coroutine tooltipCoroutine;

    public void OnPointerDown(PointerEventData eventData)
    {
        tooltipCoroutine = StartCoroutine(UpdateTooltipRoutine(eventData));



        /*ResourceManager resourceManager = ResourceManager.instance;
        
        tooltipText = $"{(ResourceType)(int)resourceType} 생산량: {resourceManager.ResourceMarkChange((int)resourceManager.total_Productions[(int)resourceType])}";
        TooltipUI.instance.ShowTooltip(tooltipText, eventData.position);*/
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (tooltipCoroutine != null)
        {
            StopCoroutine(tooltipCoroutine);
            tooltipCoroutine = null;
            TooltipUI.instance.HideTooltip(); // 툴팁 종료
        }


        /*TooltipUI.instance.HideTooltip();*/
    }

    IEnumerator UpdateTooltipRoutine(PointerEventData eventData)
    {
        ResourceManager resourceManager = ResourceManager.instance;
        while (true)
        {
            

            tooltipText = $"{(ResourceType)(int)resourceType} 생산량: " +
                $"{resourceManager.ResourceMarkChange((int)resourceManager.calcultateProduction((int)resourceType))}";

            TooltipUI.instance.ShowTooltip(tooltipText, eventData.position); // ← 여기는 '실시간 갱신'을 반영하는 TooltipUI 메서드가 있어야 함
            yield return new WaitForSeconds(0.1f); // 0.1초마다 업데이트
        }
    }
}
