using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BascInfomation : MonoBehaviour
{
    public enum Types { ControlCenter, Tab3 }
    public Types info_types;

    public Titles titles;
    public TextMeshProUGUI[] resources;
    public TextMeshProUGUI[] timeText;
    public TextMeshProUGUI[] amount_Text;
}
