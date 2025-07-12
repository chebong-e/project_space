using UnityEngine;

[System.Serializable]
public class BuildingLevels : SerializableDictionary<int, int[,]> { };
[CreateAssetMenu(fileName = "Player_Infomation", menuName = "Scriptble Object/Player_Infomation")]
public class PlayerInfomation : ScriptableObject
{
    public string userCode; // ������ ���� �ڵ� (���� �ߺ����� �ʴ� ������ ���� ���� �ѹ��ο�)
    public string userID;
    public string userPassword;
    public string userName;
    public int[][] userLocation; // ù��° �ε��� : Ȩ�÷���(0), �ݷδ�(1~) / (���� - ���� - ��ġ)

    // key�� : Ȩ�÷��ְ� �ݷδ� ���� Ȯ���ϴ� �ε�����
    // value�� : ù��° �ε���: tab1~5, �ι�° �ε���: ù��° �ε����� �ش��ϴ� �ǹ����� ����(������)
    public BuildingLevels build_Levels;
    public BuildLevels[] planets;

    //�ѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤ�
    public static float buildingSpeed 
    {
        get { return Build_Manager.instance.scriptable_Group.tab2Groups[0].level * 0.01f; }
    }
    public static float spacialBuildingSpeed
    {
        get { return Build_Manager.instance.scriptable_Group.tab2Groups[6].spacilAbility[0]; }
    }
    public static float researchSpeed
    {
        get { return Build_Manager.instance.scriptable_Group.tab2Groups[1].level * 0.01f; }
    }
    public static float spacialResearchSpeed
    {
        get { return Build_Manager.instance.scriptable_Group.tab2Groups[6].spacilAbility[1]; }
    }
    public static float makingSpeed
    {
        get { return Build_Manager.instance.scriptable_Group.tab2Groups[4].level * 0.01f; }
    }

}

[System.Serializable]
public class BuildLevels
{
    public TabWindows[] tabs;
}

[System.Serializable]
public class TabWindows
{
    public int[] lv;
    public Int_Grade[] gradeLv; // tabwindow3,4,5�� Ȱ���ϵ���
}

[System.Serializable]
public class Int_Grade
{
    public int[] lv;
}
