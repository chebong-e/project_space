using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ValueChangeToText : MonoBehaviour
{
    public TextMeshProUGUI valueText;
    Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        valueText = GetComponentInChildren<TextMeshProUGUI>();
    }
    public void ValueChanged()
    {
        valueText.text = $"{slider.value}";
    }
}
