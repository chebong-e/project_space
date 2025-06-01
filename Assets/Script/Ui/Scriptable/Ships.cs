using UnityEngine;

[CreateAssetMenu(fileName = "Ships", menuName = "Scriptble Object/Build_Ships")]
public class Ships : ScriptableObject
{
    public enum Ship_Category { G1, G2, G3, G4, Defence }
    public enum Ship_Engine { 연소엔진, 램제트엔진, 핵추진엔진, 힉스입자_가속엔진 }
    public Ship_Category category;
    

    [Header("# Ship Name")]
    public string shipName;

    [Header("Ship Build Info")]
    public int maxHaveShip_Amount; // 최고 보유 가능 수량
    public int currentHave_Ship; // 현재 보유 수량
    public float shipMaking_Time; // 함선 1대당 생산 시간
    public int shipMake_Cost; // 함선 1대당 생산 비용

    [Header("Internal Ship Info")]
    public float ship_Damage; // 공격
    public float ship_Armor; // 장갑
    public float ship_Speed; // 이동속도
    public Ship_Engine ship_Engine;
}
