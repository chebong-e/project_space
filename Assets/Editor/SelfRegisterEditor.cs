using UnityEditor;

[CustomEditor(typeof(SelfRegister))]
public class SelfRegisterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SelfRegister selfRegistration = (SelfRegister)target;

        EditorGUI.BeginChangeCheck(); // 변경 체크 시작

        /*selfRegistration.grateTitle = (SelfRegistration.GrateTitle)EditorGUILayout.EnumPopup("GrateTitle", selfRegistration.grateTitle);*/
        selfRegistration.titleType = (SelfRegister.TitleType)EditorGUILayout.EnumPopup("TitleType", selfRegistration.titleType);

        /*switch (selfRegistration.grateTitle)
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
        }*/

        switch (selfRegistration.titleType)
        {
            case SelfRegister.TitleType.Title:
                selfRegistration.subType0 = (SelfRegister.SubType0)EditorGUILayout.EnumPopup("SubType0", selfRegistration.subType0);
                break;

            case SelfRegister.TitleType.Resource:
                selfRegistration.subType1 = (SelfRegister.SubType1)EditorGUILayout.EnumPopup("SubType1", selfRegistration.subType1);
                break;

            case SelfRegister.TitleType.TimeSlide:
                selfRegistration.subType2 = (SelfRegister.SubType2)EditorGUILayout.EnumPopup("SubType2", selfRegistration.subType2);
                break;

            case SelfRegister.TitleType.Button:
                selfRegistration.subType3 = (SelfRegister.SubType3)EditorGUILayout.EnumPopup("SubType3", selfRegistration.subType3);
                break;
            case SelfRegister.TitleType.BuildShips:
                selfRegistration.subType4 = (SelfRegister.SubType4)EditorGUILayout.EnumPopup("SubType4", selfRegistration.subType4);
                break;
            case SelfRegister.TitleType.Production:
                selfRegistration.subType6 = (SelfRegister.SubType6)EditorGUILayout.EnumPopup("SubType6", selfRegistration.subType6);
                break;
        }


    }

}
