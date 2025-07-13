using UnityEngine;

[CreateAssetMenu(fileName = "Research", menuName = "Scriptble Object/Research")]
public class Research : ScriptableObject
{
    public enum Research_Category { General, High, Combat }
    public enum GeneralNumber { G0, G1, G2, G3, G4, G5, G6, G7, G8, G9, G10, G11 }
    public enum High_Number { H0, H1, H2, H3, H4, H5 }
    public enum Combat_Number { C0, C1, C2, C3, C4, C5, C6, C7, C8, C9, C10 }
    public enum GeneralResearch { General, Resource, Mining, Engine } // 기초연구, 기초연구(자원관련), 기초연구(채굴), 기초연구(엔진연구)
    public enum Engine_Type { E1, E2, E3, E4 }

    public Research_Category research_Category;
    public GeneralResearch generalResearch;
    public Engine_Type engine_Type;

    public GeneralNumber generalNumber;
    public High_Number high_Number;
    public Combat_Number combat_Number;

    public enum Combat_Research { Armor, Attack }
    public Combat_Research combat_Research;

    [Header("Research Info")]
    public int level;
    public int[] research_Time; // 업그레이드 시간
    public int[] research_Cost; // 업그레이드 비용 (메탈/크리스탈/가스)
    public float[] upgrade_Cost_Require; // 레벨당 업그레이드 비용 증가 배수
    public float research_Ability; // 연구 레벨당 증가량
    public string ability_Text; // 연구 내용
    public int[] miningSlot; // 채광 레벨당 함대 슬롯

    public Sprite img;
}
