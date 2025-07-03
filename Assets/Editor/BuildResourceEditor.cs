using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BuildResource))]
public class BuildResourceEditor : Editor
{
    
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUI.BeginChangeCheck();

        BuildResource buildResource = (BuildResource)target;

        buildResource.build_Category = 
            (BuildResource.Build_Category)EditorGUILayout.EnumPopup("Build_Category", buildResource.build_Category);
        
        switch (buildResource.build_Category)
        {
            case BuildResource.Build_Category.Resource_Factory: // tab1
                buildResource.resource_Factory =
                    (BuildResource.Resource_Factory)EditorGUILayout.EnumPopup("Resource_Factory", buildResource.resource_Factory);
                break;
            case BuildResource.Build_Category.General_Factory: // tab2
                buildResource.general_Factory = (BuildResource.General_Factory)EditorGUILayout.EnumPopup("General_Factory", buildResource.general_Factory);
                break;
            case BuildResource.Build_Category.ContorolCenter: // tab5
                

                break;
        }

        
        // 공통 표시 및 적용 변수들
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Main Infomation", EditorStyles.boldLabel);
        //EditorGUILayout.HelpBox("설명내용 예시", MessageType.Info);

        buildResource.level = EditorGUILayout.IntField("Level", buildResource.level);

        EditorGUILayout.PropertyField(serializedObject.FindProperty("init_Needs"), new GUIContent("init_Needs", "초기 필요 자원"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("building_Time"), new GUIContent("building_Time ", "레벨당 건설 시간"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("build_require"), new GUIContent("build_require", "레벨당 업그레이드 필요 자원 배수"), true);

        switch (buildResource.build_Category)
        {
            case BuildResource.Build_Category.Resource_Factory: // tab1
            case BuildResource.Build_Category.General_Factory: // tab2
                if (buildResource.build_Category == BuildResource.Build_Category.Resource_Factory)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("manufacture"), new GUIContent("manufacture - 자원 생산량"), true);
                }
                else
                {
                    EditorGUILayout.LabelField("건물 능력 / 건물능력 Text", EditorStyles.boldLabel);
                    // 일반건물간 특정 능력을 표기하기
                    if (buildResource.general_Factory == BuildResource.General_Factory.General)
                    {
                        EditorGUILayout.BeginHorizontal();

                        buildResource.buildAbility = EditorGUILayout.IntField(GUIContent.none, buildResource.buildAbility, GUILayout.Width(50));
                        buildResource.ability_Text = EditorGUILayout.TextField(GUIContent.none, buildResource.ability_Text, GUILayout.Width(300));

                        EditorGUILayout.EndHorizontal();
                    }
                    else
                    {
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("ability_Text"), new GUIContent("ability", "건물 능력"), true);
                        if (buildResource.general_Factory == BuildResource.General_Factory.Cion)
                        {
                            EditorGUILayout.PropertyField(serializedObject.FindProperty("spacilAbility"), new GUIContent("spacilAbility", "티어당 합성 확률"), true);
                        }
                        else
                        {
                            EditorGUILayout.PropertyField(serializedObject.FindProperty("spacilAbility"), new GUIContent("spacilAbility", "[0]: 건설 시간 감소율\n[1]: 유닛 제작 시간 감소율"), true);
                        }   
                    }

                }
                buildResource.electricity_Consumption = EditorGUILayout.IntField("electricity_Consumption - 전력 소모량", buildResource.electricity_Consumption);
                break;
            case BuildResource.Build_Category.ContorolCenter: // tab5
                EditorGUILayout.PropertyField(serializedObject.FindProperty("AllowableBuild"), new GUIContent("AllowableBuild", "해당 함선의 생산 가능 수량"), true);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("build_result"), new GUIContent("build_result", "레벨당 함선 보유 수량과 관련\nAllowableBuild와 +연산 작용"), true);

                break;
        }


        EditorGUILayout.PropertyField(serializedObject.FindProperty("require_condition"), new GUIContent("require_condition", "해당 오브젝트를 해금하기 위한 특정 요구조건 작성"), true);

        EditorGUILayout.PropertyField(serializedObject.FindProperty("img"), new GUIContent("img", "이미지"), true);


        serializedObject.ApplyModifiedProperties();

        if (EditorGUI.EndChangeCheck()) // 변경 체크 종료
        {
            EditorUtility.SetDirty(buildResource); // 변경 사항 저장
            serializedObject.ApplyModifiedProperties(); // SerializedObject 적용
            Repaint(); // 인스펙터 다시 그리기
        }
    }
}
