using UnityEngine;

[CreateAssetMenu(fileName = "EventOccurrence", menuName = "Scriptble Object/Event Occurrence")]
public class EventOccurrence : ScriptableObject
{
    public enum EventType { Attack, UnderAttack, Building }

    [Header("# Event")]
    public EventType eventType;
    public int timer;
    public string coordinate;

}
