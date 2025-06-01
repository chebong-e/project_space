using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Building", menuName = "Scriptble Object/Building Substance")]
public class BuildResource : ScriptableObject
{
    public enum Build_Category { Resource_Factory, General_Factory, Research, ContorolCenter, BuildShip }
    public enum Factory_Category { Metal, Cristal, Gas, Energy, 
        Metal_Repasitory, Cristal_Repasitory, Gas_Repasitory, Recycling_Factory }


    [Header("# Main Info")]
    public Build_Category build_Category;
    public Factory_Category factory_Category;
    public int grade;

    [Header("Upgrade Data")]
    public int[] init_Needs;
    public int AllowableBuild;
    [Header("# Level Data")]
    public int level;
    public int[] building_Time; // �Ǽ� �ð�
    public float[] build_require; // ���׷��̵�� �ʿ� �ڿ�
    public float[] build_result; // �Ǽ� �� �ɷ�ġ �Ǵ� �Լ���������
    [TextArea]
    public string require_condition; // �Ǽ� Ư�� �䱸 ����

    [Header("String Data")]
    public string Lv_Title;

    [Header("Datas")]
    public Sprite img;
}
