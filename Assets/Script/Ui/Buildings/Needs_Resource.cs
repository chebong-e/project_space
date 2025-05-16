using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class Needs_Resource : MonoBehaviour
{
    public enum ResourceType { Metal, Cristal, Gas }
    public ResourceType resourceType;
    GameObject res;

    void Start()
    {
        BuildDetailMatter parent = GetComponentInParent<BuildDetailMatter>();

        if (parent != null)
        {
            parent.resources[(int)resourceType] = gameObject;
        }


        res = GetComponentInChildren<TextMeshProUGUI>().gameObject;
    }
}
