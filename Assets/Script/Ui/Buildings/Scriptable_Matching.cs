using NUnit.Framework;
using UnityEngine;

public class Scriptable_Matching : MonoBehaviour
{
    public enum Tab_Category { Tab1, Tab2, Tab3, Tab4, Tab5 }
    public enum Container_Grade { G1, G2, G3, G4, G5 }

    public Tab_Category tab_Category;
    public Container_Grade container_Grade;

    public void Init()
    {
        Scriptable_Group scriptable_Group = Build_Manager.instance.GetComponent<Scriptable_Group>();
        Base_Infomation[] Infomations = GetComponentsInChildren<Base_Infomation>(true);
        switch (tab_Category)
        {
            case Tab_Category.Tab1:
                for (int i = 0; i < Infomations.Length; i++)
                {
                    Infomations[i].buildResource = scriptable_Group.GetTargetListByBuildResource((int)tab_Category, (int)container_Grade)[i];
                    
                    Infomations[i].Init_Setting();

                }
                break;
            case Tab_Category.Tab2:
                for (int i = 0; i < Infomations.Length; i++)
                {
                    Infomations[i].buildResource = scriptable_Group.GetTargetListByBuildResource((int)tab_Category, (int)container_Grade)[i];

                    Infomations[i].Init_Setting();

                }
                break;
            case Tab_Category.Tab3:
                for (int i = 0; i < Infomations.Length; i++)
                {
                    Infomations[i].research = scriptable_Group.GetTargetListByResearch((int)container_Grade)[i];

                    Infomations[i].Init_Setting();

                }
                break;

            case Tab_Category.Tab4:
            case Tab_Category.Tab5:

                //Scriptable_Group scriptable_Group = Build_Manager.instance.GetComponent<Scriptable_Group>();
                //Base_Infomation[] Infomations = GetComponentsInChildren<Base_Infomation>(true);
                //List<BuildResource> B_list = scriptable_Group.GetTargetListByBuildResource((int)tab_Category, (int)container_Grade);
                for (int i = 0; i < Infomations.Length; i++)
                {
                    Infomations[i].buildResource = scriptable_Group.GetTargetListByBuildResource((int)tab_Category, (int)container_Grade)[i];
                    Infomations[i].ship = scriptable_Group.GetTargetListByShips((int)container_Grade)[i];
                    Infomations[i].Init_Setting();
                    
                }
                break;
        }
    }
}
