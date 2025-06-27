using UnityEngine;

[CreateAssetMenu(fileName = "Research", menuName = "Scriptble Object/Research")]
public class Research : ScriptableObject
{
    public enum Research_Category { General, High, Combat }
    public enum GeneralResearch { General, Resource, Mining, Engine } // 기초연구, 기초연구(자원관련), 기초연구(채굴), 기초연구(엔진연구)
    public enum Engine_Type { G1, G2, G3, G4 } // 연소, 램제트, 핵 추진, 힉스입자 가속

    public Research_Category research_Category;
    public GeneralResearch generalResearch;
    public Engine_Type engine_Type;

    public enum High_Research { S1, S2, S3, S4, S5, S6 } // 정탐, 컴퓨터공학, 천체물리학, 은하간연구망, 고등탄도학, 고등조선학
    public High_Research high_Research;

    public enum Combat_Research { Armor, Attack }
    public Combat_Research combat_Research;

    [Header("Research Info")]
    public int research_Level;
    public int[] research_Time; // 업그레이드 시간
    public int[] research_Cost; // 업그레이드 비용 (메탈/크리스탈/가스)
    public float[] upgrade_Cost_Require; // 레벨당 업그레이드 비용 증가 배수


}
