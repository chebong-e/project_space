using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public enum ResourceType { ��Ż, ũ����Ż, ���� }
    public ResourceType resourceType;
    public string tooltipText;

    public void OnPointerDown(PointerEventData eventData)
    {
        ResourceManager resourceManager = ResourceManager.instance;
        
        tooltipText = $"{(ResourceType)(int)resourceType} ���귮: {resourceManager.ResourceMarkChange(resourceManager.resource_Productions[(int)resourceType] + resourceManager.basicProductions[(int)resourceType])}";
        TooltipUI.instance.ShowTooltip(tooltipText, eventData.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        TooltipUI.instance.HideTooltip();
    }
}
