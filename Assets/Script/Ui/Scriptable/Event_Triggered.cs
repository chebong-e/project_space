using UnityEngine;

[CreateAssetMenu(fileName = "EventTriggered", menuName = "Scriptble Object/Event Triggered")]
public class Event_Triggered : ScriptableObject
{
    public enum EventType { Attack, UnderAttack, Building }

    [Header("# Event")]
    public EventType eventType;
    public int timer;
    public string coordinate;
}
