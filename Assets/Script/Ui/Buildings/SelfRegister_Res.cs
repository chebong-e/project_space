using TMPro;
using UnityEngine;

public class SelfRegister_Res : MonoBehaviour
{
    public enum Resource_Type { Metal, Cristal, Gas, Energy, Cash }

    public Resource_Type type;

    public void Init()
    {
        ResourceValue resourceValue = new ();   
        resourceValue.container = GetComponent<TextMeshProUGUI>();

        ResourceManager.instance.resourceWindow[((Resource_Type)(int)type).ToString()] = resourceValue;
    }
}
