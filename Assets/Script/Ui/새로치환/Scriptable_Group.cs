using System.Collections.Generic;
using UnityEngine;

public class Scriptable_Group : MonoBehaviour
{
    [Header("BuildeResources")]
    public List<BuildResourceGroup> buildGroups;

    [Header("Ships")]
    public List<ShipGroup> shipGroups;

    public List<BuildResource> GetTargetListByBuildResource(int index) => (index >= 0 && index < buildGroups.Count) ? buildGroups[index].buildResources : null;
    public List<Ship> GetTargetListByShips(int index) => (index >= 0 && index < shipGroups.Count) ? shipGroups[index].ships : null;

}

[System.Serializable]
public class BuildResourceGroup
{
    public List<BuildResource> buildResources;
}

[System.Serializable]
public class ShipGroup
{
    public List<Ship> ships;
}
