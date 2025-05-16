using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Building", menuName = "Scriptble Object/Building Substance")]
public class BuildResource : ScriptableObject
{
    public enum Build_Category { Resource_Factory, General_Factory, Research, ContorolCenter }
    public enum Factory_Category { Metal, Cristal, Gas, Energy, 
        Metal_Repasitory, Cristal_Repasitory, Gas_Repasitory, Recycling_Factory }


    /*public enum BuildingType { Mineral, Cristal, Gas, Electric_Energy, population }*/

    [Header("# Main Info")]
    public Build_Category build_Category;
    public Factory_Category factory_Category;

    [Header("# Level Data")]
    public int[] building_Time; // �Ǽ� �ð�
    public Dictionary<int, int>[] testValue;
    public float[] build_require; // �Ǽ� �䱸 ��ȭ
    public float[] build_result; // �Ǽ� �� �ɷ�ġ
    [TextArea]
    public string require_condition; // �Ǽ� Ư�� �䱸 ����

}
