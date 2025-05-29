using UnityEngine;

[CreateAssetMenu(fileName = "Ships", menuName = "Scriptble Object/Build_Ships")]
public class Ships : ScriptableObject
{
    public enum Ship_Category { G1, G2, G3, G4, Defence }
    public Ship_Category category;
    [Header("# Ship Name")]
    public string shipName;

    [Header("Ship Build Info")]
    public int maxShip_Amount; // 최고 보유 가능 수량
    public int have_CurrentShip; // 현재 보유 수량
    public float shipBuild_Time; // 함선 1대당 생산 시간

    [Header("Internal Ship Info")]
    public float ship_Damage; // 공격
    public float ship_Armor; // 장갑
    public float ship_Speed; // 이동속도
}
