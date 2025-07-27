using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelfEventRegister : MonoBehaviour
{
    public enum Type { TargetLocate, Timer, MissionImg, Mission }
    public enum MissionCategory { Neutral, Friendly, Hostile }
    public Type type;
    public MissionCategory missionCategory;
    EventLine eventLine;

    private void Awake()
    {
        eventLine = GetComponentInParent<EventLine>();

        switch (type)
        {
            case Type.TargetLocate:
                eventLine.targetText = GetComponent<TextMeshProUGUI>();
                break;
            case Type.Timer:
                eventLine.timerText = GetComponent<TextMeshProUGUI>();
                break;
            case Type.MissionImg:
                eventLine.missionImg = GetComponent<Image>();
                break;
            /*case Type.Mission:
                switch (missionCategory)
                {
                    case MissionCategory.Neutral:
                        
                        break;
                    case MissionCategory.Friendly:

                        break;
                    case MissionCategory.Hostile:

                        break;

                }
                break;*/
        }
    }
}
