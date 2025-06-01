using UnityEngine;

[CreateAssetMenu(fileName = "Ships", menuName = "Scriptble Object/Build_Ships")]
public class Ships : ScriptableObject
{
    public enum Ship_Category { G1, G2, G3, G4, Defence }
    public enum Ship_Engine { ���ҿ���, ����Ʈ����, ����������, ��������_���ӿ��� }
    public Ship_Category category;
    

    [Header("# Ship Name")]
    public string shipName;

    [Header("Ship Build Info")]
    public int maxHaveShip_Amount; // �ְ� ���� ���� ����
    public int currentHave_Ship; // ���� ���� ����
    public float shipMaking_Time; // �Լ� 1��� ���� �ð�
    public int shipMake_Cost; // �Լ� 1��� ���� ���

    [Header("Internal Ship Info")]
    public float ship_Damage; // ����
    public float ship_Armor; // �尩
    public float ship_Speed; // �̵��ӵ�
    public Ship_Engine ship_Engine;
}
