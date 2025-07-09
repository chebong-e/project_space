using TMPro;
using UnityEngine;

public class TooltipUI : MonoBehaviour
{
    public static TooltipUI instance;

    public GameObject tooltipObject;
    public TextMeshProUGUI tooltipText;

    private void Awake()
    {
        instance = this;
        HideTooltip();
    }

    public void ShowTooltip(string content, Vector3 position)
    {
        tooltipText.text = content;
        tooltipObject.SetActive(true);
        tooltipObject.transform.position = position + new Vector3(10f, -10f, 0f); // 약간 오프셋
    }

    public void HideTooltip()
    {
        tooltipObject.SetActive(false);
    }
}
