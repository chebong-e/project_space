using System.Collections.Generic;
using UnityEngine;
using static Scriptable_Matching;

public class Scriptable_Matching : MonoBehaviour
{
    public enum Tab_Category { Tab1, Tab2, Tab3, Tab4, Tab5 }
    public enum Container_Grade { G1, G2, G3, G4, G5 }

    public Tab_Category tab_Category;
    public Container_Grade container_Grade;

    public void Init()
    {
        switch (tab_Category)
        {
            case Tab_Category.Tab1:

                break;
            case Tab_Category.Tab2:

                break;
            case Tab_Category.Tab3:

                break;
            case Tab_Category.Tab4:
            case Tab_Category.Tab5:

                Scriptable_Group scriptable_Group = Build_Manager.instance.GetComponent<Scriptable_Group>();
                Con_Infomation[] con_Infomations = GetComponentsInChildren<Con_Infomation>(true);

                for (int i = 0; i < con_Infomations.Length; i++)
                {
                    con_Infomations[i].buildResource = scriptable_Group.GetTargetListByBuildResource((int)container_Grade)[i];
                    con_Infomations[i].ship = scriptable_Group.GetTargetListByShips((int)container_Grade)[i];
                    con_Infomations[i].Init_Setting();
                    
                }
                break;
        }
    }

    /*public List<BuildResource> GetTargetListByBuildResource(int index)
    {
        Scriptable_Group scriptable_Group = Build_Manager.instance.GetComponent<Scriptable_Group>();
        switch (index)
        {
            case 0: return scriptable_Group.b_G1;
            case 1: return scriptable_Group.b_G2;
            case 2: return scriptable_Group.b_G3;
            case 3: return scriptable_Group.b_G4;
            case 4: return scriptable_Group.b_G5;
            default: return null;
        }
    }

    public List<Ship> GetTargetListByShips(int index)
    {
        Scriptable_Group scriptable_Group = Build_Manager.instance.GetComponent<Scriptable_Group>();
        switch (index)
        {
            case 0: return scriptable_Group.s_G1;
            case 1: return scriptable_Group.s_G2;
            case 2: return scriptable_Group.s_G3;
            case 3: return scriptable_Group.s_G4;
            case 4: return scriptable_Group.s_G5;
            default: return null;
        }
    }*/
}
