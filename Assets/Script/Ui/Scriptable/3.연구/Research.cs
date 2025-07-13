using UnityEngine;

[CreateAssetMenu(fileName = "Research", menuName = "Scriptble Object/Research")]
public class Research : ScriptableObject
{
    public enum Research_Category { General, High, Combat }
    public enum GeneralNumber { G0, G1, G2, G3, G4, G5, G6, G7, G8, G9, G10, G11 }
    public enum High_Number { H0, H1, H2, H3, H4, H5 }
    public enum Combat_Number { C0, C1, C2, C3, C4, C5, C6, C7, C8, C9, C10 }
    public enum GeneralResearch { General, Resource, Mining, Engine } // ���ʿ���, ���ʿ���(�ڿ�����), ���ʿ���(ä��), ���ʿ���(��������)
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
    public int[] research_Time; // ���׷��̵� �ð�
    public int[] research_Cost; // ���׷��̵� ��� (��Ż/ũ����Ż/����)
    public float[] upgrade_Cost_Require; // ������ ���׷��̵� ��� ���� ���
    public float research_Ability; // ���� ������ ������
    public string ability_Text; // ���� ����
    public int[] miningSlot; // ä�� ������ �Դ� ����

    public Sprite img;
}
