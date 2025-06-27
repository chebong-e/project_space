using System;
using System.Collections.Generic;
using UnityEngine;

public class Scriptable_Group : MonoBehaviour
{
    [Header("Tab1 Scriptables")]
    public List<BuildResource> tab1Group;

    [Header("Tab2 Scriptables")]
    public List<BuildResource> tab2Groups;

    [Header("Tab3 Scriptables")]
    public List<Tab3Group> tab3Groups;

    [Header("Ships")]
    public List<ShipGroup> shipGroups;

    [Header("Controlcenters")]
    public List<ControlcenterGroup> controlcenterGroup;

    public List<BuildResource> GetTargetListByBuildResource(int tab_index, int grade)
    {
        List<BuildResource> targetList = new List<BuildResource>();
        switch (tab_index)
        {
            case 0:
                targetList = tab1Group;
                break;
            case 1:

                break;
            case 2:

                break;
            case 3:
            case 4:
                if (grade >= 0 && grade < controlcenterGroup.Count)
                {
                    targetList = controlcenterGroup[grade].buildResources;
                }
                break;
        }


        return targetList;
    }

    /*public List<BuildResource> GetTargetListByBuildResource(int index) => (index >= 0 && index < controlcenterGroup.Count) ? controlcenterGroup[index].buildResources : null;*/
    public List<Ship> GetTargetListByShips(int index) => (index >= 0 && index < shipGroups.Count) ? shipGroups[index].ships : null;

}

[System.Serializable]
public class Tab3Group
{
    public List<BuildResource> buildResources;
}

[System.Serializable]
public class ShipGroup
{
    public List<Ship> ships;
}

[System.Serializable]
public class ControlcenterGroup
{
    public List<BuildResource> buildResources;
}