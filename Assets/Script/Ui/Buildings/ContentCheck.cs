using UnityEngine;
using UnityEngine.UI;

public class ContentCheck : MonoBehaviour
{
    public ScrollRect[] contents;

    void Awake()
    {
        contents = GetComponentsInChildren<ScrollRect>(true);
    }
}
