using UnityEngine;

[CreateAssetMenu(fileName = "EventTriggered", menuName = "Scriptble Object/Event Triggered")]
public class Event_Triggered : ScriptableObject
{
    public enum Event_Type { Nuetral_Missions, UnionSupport, Attack, UnderAttack }
    public enum NuetralCategory { Mining, Move, Transport }

    [Header("# Event")]
    /*public Event_Type eventType;*/
    public float timer;
    public string baseLocate;
    /*public string coordinate;*/
    public bool isUsed;

    // 실험적 확인
    [SerializeField]
    public Mission_Infomation mission;
}
