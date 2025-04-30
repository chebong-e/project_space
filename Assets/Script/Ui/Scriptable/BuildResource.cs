using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Building", menuName = "Scriptble Object/Building Substance")]
public class BuildResource : ScriptableObject
{
    public enum BuildingType { Mineral, Cristal, Gas, Electric_Energy, population }

    [Header("# Main Info")]
    public BuildingType buildType;

    [Header("# Level Data")]
    public int[] building_Time; // 건설 시간
    public Dictionary<int, int>[] testValue;
    public float[] build_require; // 건설 요구 재화
    public float[] build_result; // 건설 후 능력치
    [TextArea]
    public string require_condition; // 건설 특정 요구 조건

}
