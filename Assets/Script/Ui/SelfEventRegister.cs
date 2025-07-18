using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelfEventRegister : MonoBehaviour
{
    public enum Type { TargetLocate, Timer, MissionImg }
    public Type type;
    EventLine eventLine;

    private void Awake()
    {
        eventLine = GetComponentInParent<EventLine>();
        if (type == Type.TargetLocate)
        {
            eventLine.targetText = GetComponent<TextMeshProUGUI>();
        }
        else if (type == Type.Timer)
        {
            eventLine.timerText = GetComponent<TextMeshProUGUI>();
        }
        else
        {
            eventLine.missionImg = GetComponent<Image>();
        }
    }
}
