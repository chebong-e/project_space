using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Building", menuName = "Scriptble Object/Building Substance")]
public class BuildResource : ScriptableObject
{
    public enum Build_Category { Resource_Factory, General_Factory, Research, ContorolCenter, BuildShip }
    public enum Resource_Factory { Metal, Cristal, Gas, Energy, 
        Metal_Repasitory, Cristal_Repasitory, Gas_Repasitory, Recycling_Factory }
    public enum General_Factory { General, Cion, HighTech }


    [Header("# Main Info")]
    public Build_Category build_Category;
    public Resource_Factory resource_Factory;
    public General_Factory general_Factory;
    //public int grade;

    [Header("Upgrade Data")]
    public int[] init_Needs;
    public int AllowableBuild;
    public int basic_manufacture;
    public int[] manufacture;
    public float magnification;
    public int electricity_Consumption;
    [Header("# Level Data")]
    public int level;
    public int[] building_Time; // 건설 시간
    public float[] build_require; // 업그레이드시 필요 자원 배수
    public float[] build_result; // 건설 후 능력치 배수 또는 함선보유수량
    [TextArea]
    public string require_condition; // 건설 특정 요구 조건
    public int buildAbility;
    public string ability_Text;
    public float[] spacilAbility;

    [Header("Datas")]
    public Sprite img;
}
