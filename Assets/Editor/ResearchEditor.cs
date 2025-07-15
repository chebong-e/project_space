using UnityEditor;
using UnityEngine;
using static Research;


[CustomEditor(typeof(Research))]
public class ResearchEditor : Editor
{
    SerializedProperty level;
    SerializedProperty research_Time;
    SerializedProperty research_Cost;
    SerializedProperty upgrade_Cost_Require;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUI.BeginChangeCheck();
        Research research = (Research)target;

        EditorGUI.BeginChangeCheck();

        research.research_Category = (Research.Research_Category)EditorGUILayout.EnumPopup("Research_Category", research.research_Category);

        switch (research.research_Category)
        {
            case Research_Category.General:
                research.generalResearch = (Research.GeneralResearch)EditorGUILayout.EnumPopup("GeneralResearch", research.generalResearch);
                research.generalNumber = (Research.GeneralNumber)EditorGUILayout.EnumPopup("GeneralNumber", research.generalNumber);
                if (research.generalResearch == GeneralResearch.Engine)
                {
                    research.engine_Type = (Research.Engine_Type)EditorGUILayout.EnumPopup("Engine_Type", research.engine_Type);
                }
                else if (research.generalResearch == GeneralResearch.Mining)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("miningSlot"), new GUIContent("채광 레벨당 함대 슬롯"), true);
                }
                    break;
            case Research_Category.High:
                research.high_Number = (Research.High_Number)EditorGUILayout.EnumPopup("High_Number", research.high_Number);
                break;
            case Research_Category.Combat:
                research.combat_Research = (Research.Combat_Research)EditorGUILayout.EnumPopup("Combat_Research", research.combat_Research);
                research.combat_Number = (Research.Combat_Number)EditorGUILayout.EnumPopup("Combat_Number", research.combat_Number);
                break;
        }


        level = serializedObject.FindProperty("level");
        research_Time = serializedObject.FindProperty("research_Time");
        research_Cost = serializedObject.FindProperty("research_Cost");
        upgrade_Cost_Require = serializedObject.FindProperty("upgrade_Cost_Require");

        EditorGUILayout.PropertyField(level, new GUIContent("연구 레벨"), true);
        EditorGUILayout.PropertyField(research_Time, new GUIContent("연구 시간"), true);
        EditorGUILayout.PropertyField(research_Cost, new GUIContent("연구 비용"), true);
        EditorGUILayout.PropertyField(upgrade_Cost_Require, new GUIContent("업그레이드 비용 배수"), true);
        
        EditorGUILayout.LabelField(new GUIContent("연구수치 증가량 / 연구 능력 Text", "research_Ability / ability_Text"), EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("research_Ability"), GUIContent.none, GUILayout.Width(50));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("ability_Text"), GUIContent.none, GUILayout.Width(300));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("img"), new GUIContent("img", "이미지"), true);

        serializedObject.ApplyModifiedProperties();
    }
}
