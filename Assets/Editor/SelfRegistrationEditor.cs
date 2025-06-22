using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SelfRegistration))]

public class SelfRegistrationEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SelfRegistration selfRegistration = (SelfRegistration)target;

        EditorGUI.BeginChangeCheck(); // 변경 체크 시작

        selfRegistration.grateTitle = (SelfRegistration.GrateTitle)EditorGUILayout.EnumPopup("GrateTitle", selfRegistration.grateTitle);
        selfRegistration.titleType = (SelfRegistration.TitleType)EditorGUILayout.EnumPopup("TitleType", selfRegistration.titleType);

        switch (selfRegistration.grateTitle)
        {
            case SelfRegistration.GrateTitle.MainTab:

                break;
            case SelfRegistration.GrateTitle.BuildTab1:

                break;
            case SelfRegistration.GrateTitle.BuildTab2:

                break;
            case SelfRegistration.GrateTitle.BuildTab3:
                
                break;
            case SelfRegistration.GrateTitle.BuildTab4:

                break;
            case SelfRegistration.GrateTitle.BuildTab5:

                break;
            case SelfRegistration.GrateTitle.FleetMision:

                break;
        }

        switch (selfRegistration.titleType)
        {
            case SelfRegistration.TitleType.Title:
                selfRegistration.subType0 = (SelfRegistration.SubType0)EditorGUILayout.EnumPopup("SubType0", selfRegistration.subType0);
                break;

            case SelfRegistration.TitleType.Resource:
                selfRegistration.subType1 = (SelfRegistration.SubType1)EditorGUILayout.EnumPopup("SubType1", selfRegistration.subType1);
                break;

            case SelfRegistration.TitleType.TimeSlide:
                selfRegistration.subType2 = (SelfRegistration.SubType2)EditorGUILayout.EnumPopup("SubType2", selfRegistration.subType2);
                break;

            case SelfRegistration.TitleType.Button:
                selfRegistration.subType3 = (SelfRegistration.SubType3)EditorGUILayout.EnumPopup("SubType3", selfRegistration.subType3);
                break;
            case SelfRegistration.TitleType.BuildShips:
                selfRegistration.subType4 = (SelfRegistration.SubType4)EditorGUILayout.EnumPopup("SubType4", selfRegistration.subType4);
                break;
            case SelfRegistration.TitleType.Production:
                selfRegistration.subType6 = (SelfRegistration.SubType6)EditorGUILayout.EnumPopup("SubType6", selfRegistration.subType6);
                break;
        }


    }

}
