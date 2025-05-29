using UnityEngine;

[CreateAssetMenu(fileName = "Init_Settings", menuName = "Scriptble Object/Init_Settings")]
public class Init_SettingScriptable : ScriptableObject
{
    [Header("Script")]
    public BuildDetailMatter buildDetailMatter;
    public Infomations Infomations;
    public ImageSlide imageSlide;
    public ShipBuildSlider shipBuildSlider;

    [Header("ScriptableObject")]
    public Ships ships;
    public BuildResource buildResource;
}
