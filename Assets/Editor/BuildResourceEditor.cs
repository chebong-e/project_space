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
                buildResource.AllowableBuild = EditorGUILayout.IntField("���갡�� �Լ���", buildResource.AllowableBuild);
                break;
        }

        if (EditorGUI.EndChangeCheck()) // ���� üũ ����
        {
            EditorUtility.SetDirty(buildResource); // ���� ���� ����
            serializedObject.ApplyModifiedProperties(); // SerializedObject ����
            Repaint(); // �ν����� �ٽ� �׸���
        }

    }
}
