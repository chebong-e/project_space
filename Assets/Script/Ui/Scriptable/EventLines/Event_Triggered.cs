using UnityEngine;

[CreateAssetMenu(fileName = "EventTriggered", menuName = "Scriptble Object/Event Triggered")]
public class Event_Triggered : ScriptableObject
{
    public enum Event_Type { Attack, UnderAttack, Missions, UnionSupport }

    [Header("# Event")]
    public Event_Type eventType;
    public int timer;
    public string coordinate;
    public bool isUsed;
}
