using UnityEngine;
using UnityEditor;

/*[CustomEditor(typeof(BuildResource))]*/
public class BuildResourceEditor : Editor
{
    public override void OnInspectorGUI()
    {
        BuildResource buildResource = (BuildResource)target;

        EditorGUI.BeginChangeCheck();

        buildResource.build_Category = (BuildResource.Build_Category)EditorGUILayout.EnumPopup("Build_Category", buildResource.build_Category);
        
        switch (buildResource.build_Category)
        {
            case BuildResource.Build_Category.Resource_Factory:
                buildResource.factory_Category = (BuildResource.Factory_Category)EditorGUILayout.EnumPopup("Factory_Category", buildResource.factory_Category);
                break;
            case BuildResource.Build_Category.General_Factory:

                break;
            case BuildResource.Build_Category.ContorolCenter:
                buildResource.AllowableBuild = EditorGUILayout.IntField("생산가능 함선수", buildResource.AllowableBuild);
                break;
        }

        if (EditorGUI.EndChangeCheck()) // 변경 체크 종료
        {
            EditorUtility.SetDirty(buildResource); // 변경 사항 저장
            serializedObject.ApplyModifiedProperties(); // SerializedObject 적용
            Repaint(); // 인스펙터 다시 그리기
        }

    }
}
