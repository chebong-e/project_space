using UnityEngine;
using UnityEngine.UI;

public class GalaxyInitSet : MonoBehaviour
{
    public GalaxyManager galaxyManager;

    ScrollRect scrollRect;

    private void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();
    }

    private void Start()
    {
        Galaxy_InitSetting(13);
    }

    void Galaxy_InitSetting(int num)
    {
        

        for (int i = 0; i < num; i++)
        {
            if (i == 0)
            {
                Instantiate(galaxyManager.planet_Prefabs[0], scrollRect.content);
                Instantiate(galaxyManager.planet_Prefabs[2], scrollRect.content);
                Instantiate(galaxyManager.planet_Prefabs[4], scrollRect.content);
                galaxyManager.res_Planets.Add(Instantiate(galaxyManager.planet_Prefabs[5], scrollRect.content));
            }
            else
            {
                Instantiate(galaxyManager.planet_Prefabs[2], scrollRect.content);
            }
        }

    }
}
