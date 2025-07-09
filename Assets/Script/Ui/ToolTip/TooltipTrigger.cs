using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public enum ResourceType { 메탈, 크리스탈, 가스 }
    public ResourceType resourceType;
    public string tooltipText;

    public void OnPointerDown(PointerEventData eventData)
    {
        ResourceManager resourceManager = ResourceManager.instance;
        
        tooltipText = $"{(ResourceType)(int)resourceType} 생산량: {resourceManager.ResourceMarkChange(resourceManager.resource_Productions[(int)resourceType] + resourceManager.basicProductions[(int)resourceType])}";
        TooltipUI.instance.ShowTooltip(tooltipText, eventData.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        TooltipUI.instance.HideTooltip();
    }
}
