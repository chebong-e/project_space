using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Building", menuName = "Scriptble Object/Building Substance")]
public class BuildResource : ScriptableObject
{
    public enum BuildingType { Mineral, Cristal, Gas, Electric_Energy, population }

    [Header("# Main Info")]
    public BuildingType buildType;

    [Header("# Level Data")]
    public int[] building_Time; // �Ǽ� �ð�
    public Dictionary<int, int>[] testValue;
    public float[] build_require; // �Ǽ� �䱸 ��ȭ
    public float[] build_result; // �Ǽ� �� �ɷ�ġ
    [TextArea]
    public string require_condition; // �Ǽ� Ư�� �䱸 ����

}
