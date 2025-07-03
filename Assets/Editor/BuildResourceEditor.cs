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

        
        // ���� ǥ�� �� ���� ������
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Main Infomation", EditorStyles.boldLabel);
        //EditorGUILayout.HelpBox("������ ����", MessageType.Info);

        buildResource.level = EditorGUILayout.IntField("Level", buildResource.level);

        EditorGUILayout.PropertyField(serializedObject.FindProperty("init_Needs"), new GUIContent("init_Needs", "�ʱ� �ʿ� �ڿ�"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("building_Time"), new GUIContent("building_Time ", "������ �Ǽ� �ð�"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("build_require"), new GUIContent("build_require", "������ ���׷��̵� �ʿ� �ڿ� ���"), true);

        switch (buildResource.build_Category)
        {
            case BuildResource.Build_Category.Resource_Factory: // tab1
            case BuildResource.Build_Category.General_Factory: // tab2
                if (buildResource.build_Category == BuildResource.Build_Category.Resource_Factory)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("manufacture"), new GUIContent("manufacture - �ڿ� ���귮"), true);
                }
                else
                {
                    EditorGUILayout.LabelField("�ǹ� �ɷ� / �ǹ��ɷ� Text", EditorStyles.boldLabel);
                    // �Ϲݰǹ��� Ư�� �ɷ��� ǥ���ϱ�
                    if (buildResource.general_Factory == BuildResource.General_Factory.General)
                    {
                        EditorGUILayout.BeginHorizontal();

                        buildResource.buildAbility = EditorGUILayout.IntField(GUIContent.none, buildResource.buildAbility, GUILayout.Width(50));
                        buildResource.ability_Text = EditorGUILayout.TextField(GUIContent.none, buildResource.ability_Text, GUILayout.Width(300));

                        EditorGUILayout.EndHorizontal();
                    }
                    else
                    {
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("ability_Text"), new GUIContent("ability", "�ǹ� �ɷ�"), true);
                        if (buildResource.general_Factory == BuildResource.General_Factory.Cion)
                        {
                            EditorGUILayout.PropertyField(serializedObject.FindProperty("spacilAbility"), new GUIContent("spacilAbility", "Ƽ��� �ռ� Ȯ��"), true);
                        }
                        else
                        {
                            EditorGUILayout.PropertyField(serializedObject.FindProperty("spacilAbility"), new GUIContent("spacilAbility", "[0]: �Ǽ� �ð� ������\n[1]: ���� ���� �ð� ������"), true);
                        }   
                    }

                }
                buildResource.electricity_Consumption = EditorGUILayout.IntField("electricity_Consumption - ���� �Ҹ�", buildResource.electricity_Consumption);
                break;
            case BuildResource.Build_Category.ContorolCenter: // tab5
                EditorGUILayout.PropertyField(serializedObject.FindProperty("AllowableBuild"), new GUIContent("AllowableBuild", "�ش� �Լ��� ���� ���� ����"), true);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("build_result"), new GUIContent("build_result", "������ �Լ� ���� ������ ����\nAllowableBuild�� +���� �ۿ�"), true);

                break;
        }


        EditorGUILayout.PropertyField(serializedObject.FindProperty("require_condition"), new GUIContent("require_condition", "�ش� ������Ʈ�� �ر��ϱ� ���� Ư�� �䱸���� �ۼ�"), true);

        EditorGUILayout.PropertyField(serializedObject.FindProperty("img"), new GUIContent("img", "�̹���"), true);


        serializedObject.ApplyModifiedProperties();

        if (EditorGUI.EndChangeCheck()) // ���� üũ ����
        {
            EditorUtility.SetDirty(buildResource); // ���� ���� ����
            serializedObject.ApplyModifiedProperties(); // SerializedObject ����
            Repaint(); // �ν����� �ٽ� �׸���
        }
    }
}
