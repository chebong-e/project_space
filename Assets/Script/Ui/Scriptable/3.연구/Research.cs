using UnityEngine;

[CreateAssetMenu(fileName = "Research", menuName = "Scriptble Object/Research")]
public class Research : ScriptableObject
{
    public enum Research_Category { General, High, Combat }
    public enum GeneralResearch { General, Resource, Mining, Engine } // ���ʿ���, ���ʿ���(�ڿ�����), ���ʿ���(ä��), ���ʿ���(��������)
    public enum Engine_Type { G1, G2, G3, G4 } // ����, ����Ʈ, �� ����, �������� ����

    public Research_Category research_Category;
    public GeneralResearch generalResearch;
    public Engine_Type engine_Type;

    public enum High_Research { S1, S2, S3, S4, S5, S6 } // ��Ž, ��ǻ�Ͱ���, õü������, ���ϰ�������, ���ź����, ���������
    public High_Research high_Research;

    public enum Combat_Research { Armor, Attack }
    public Combat_Research combat_Research;

    [Header("Research Info")]
    public int research_Level;
    public int[] research_Time; // ���׷��̵� �ð�
    public int[] research_Cost; // ���׷��̵� ��� (��Ż/ũ����Ż/����)
    public float[] upgrade_Cost_Require; // ������ ���׷��̵� ��� ���� ���


}
