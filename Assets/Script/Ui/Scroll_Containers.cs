using UnityEngine;
using TMPro;

[System.Serializable]
public class InfomationTarget : SerializableDictionary<string, GameObject> { };
public class Scroll_Containers : MonoBehaviour
{
    public enum Scroll_Type { Build, Research }
    public enum Research_Type { General, High, Combat }
    public enum Build_Type { Resource, General }

    public Scroll_Type scroll_Type;
    public Research_Type research_Type;
    public Build_Type build_Type;


    public InfomationTarget target;

    void Start()
    {
        if (scroll_Type == Scroll_Type.Research)
            target["name"] = transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(1).gameObject;

        if (target.ContainsKey("name"))
        {
            string exText = target["name"].GetComponent<TextMeshProUGUI>().text;
            target["name"].GetComponent<TextMeshProUGUI>().text = string.Format(exText, 99);
        }
        
    }


}
