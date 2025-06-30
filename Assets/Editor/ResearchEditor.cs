using UnityEditor;
using UnityEngine;
using static Research;


[CustomEditor(typeof(Research))]
public class ResearchEditor : Editor
{
    SerializedProperty research_Level;
    SerializedProperty research_Time;
    SerializedProperty research_Cost;
    SerializedProperty upgrade_Cost_Require;

    public override void OnInspectorGUI()
    {
        Research research = (Research)target;

        EditorGUI.BeginChangeCheck();

        research.research_Category = (Research.Research_Category)EditorGUILayout.EnumPopup("Research_Category", research.research_Category);

        switch (research.research_Category)
        {
            case Research_Category.General:
                research.generalResearch = (Research.GeneralResearch)EditorGUILayout.EnumPopup("GeneralResearch", research.generalResearch);
                research.engine_Type = (Research.Engine_Type)EditorGUILayout.EnumPopup("Engine_Type", research.engine_Type);
                break;

            case Research_Category.High:
                research.high_Research = (Research.High_Research)EditorGUILayout.EnumPopup("High_Research", research.high_Research);
                break;
            case Research_Category.Combat:
                research.combat_Research = (Research.Combat_Research)EditorGUILayout.EnumPopup("Combat_Research", research.combat_Research);
                break;
        }

        serializedObject.Update();
        research_Level = serializedObject.FindProperty("research_Level");
        research_Time = serializedObject.FindProperty("research_Time");
        research_Cost = serializedObject.FindProperty("research_Cost");
        upgrade_Cost_Require = serializedObject.FindProperty("upgrade_Cost_Require");

        EditorGUILayout.PropertyField(research_Level, new GUIContent("연구 레벨"), true);
        EditorGUILayout.PropertyField(research_Time, new GUIContent("연구 시간"), true);
        EditorGUILayout.PropertyField(research_Cost, new GUIContent("연구 비용"), true);
        EditorGUILayout.PropertyField(upgrade_Cost_Require, new GUIContent("업그레이드 비용 배수"), true);

        serializedObject.ApplyModifiedProperties();
    }
}
