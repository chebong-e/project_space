using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public enum ResourceType { ��Ż, ũ����Ż, ���� }
    public ResourceType resourceType;
    public string tooltipText;

    Coroutine tooltipCoroutine;

    public void OnPointerDown(PointerEventData eventData)
    {
        tooltipCoroutine = StartCoroutine(UpdateTooltipRoutine(eventData));



        /*ResourceManager resourceManager = ResourceManager.instance;
        
        tooltipText = $"{(ResourceType)(int)resourceType} ���귮: {resourceManager.ResourceMarkChange((int)resourceManager.total_Productions[(int)resourceType])}";
        TooltipUI.instance.ShowTooltip(tooltipText, eventData.position);*/
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (tooltipCoroutine != null)
        {
            StopCoroutine(tooltipCoroutine);
            tooltipCoroutine = null;
            TooltipUI.instance.HideTooltip(); // ���� ����
        }


        /*TooltipUI.instance.HideTooltip();*/
    }

    IEnumerator UpdateTooltipRoutine(PointerEventData eventData)
    {
        ResourceManager resourceManager = ResourceManager.instance;
        while (true)
        {
            

            tooltipText = $"{(ResourceType)(int)resourceType} ���귮: " +
                $"{resourceManager.ResourceMarkChange((int)resourceManager.calcultateProduction((int)resourceType))}";

            TooltipUI.instance.ShowTooltip(tooltipText, eventData.position); // �� ����� '�ǽð� ����'�� �ݿ��ϴ� TooltipUI �޼��尡 �־�� ��
            yield return new WaitForSeconds(0.1f); // 0.1�ʸ��� ������Ʈ
        }
    }
}
