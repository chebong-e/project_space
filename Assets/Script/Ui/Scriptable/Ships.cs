using UnityEngine;

[CreateAssetMenu(fileName = "Ships", menuName = "Scriptble Object/Build_Ships")]
public class Ships : ScriptableObject
{
    public enum Ship_Category { G1, G2, G3, G4, Defence }
    public Ship_Category category;
    [Header("# Ship Name")]
    public string shipName;

    [Header("Ship Build Info")]
    public int maxShip_Amount; // �ְ� ���� ���� ����
    public int have_CurrentShip; // ���� ���� ����
    public float shipBuild_Time; // �Լ� 1��� ���� �ð�

    [Header("Internal Ship Info")]
    public float ship_Damage; // ����
    public float ship_Armor; // �尩
    public float ship_Speed; // �̵��ӵ�
}
