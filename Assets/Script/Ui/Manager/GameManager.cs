using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public void UserSet()
    {
        int locate;
        int number;

        locate = Random.Range(0, 30);
        number = Random.Range(0, GalaxyManager.instance.galaxyMaps[0].galaxy_Index_To_Map[locate].GetComponent<ScrollRect>()
            .content.childCount);

        while (number == 10 || number == 13)
        {
            number = Random.Range(0, GalaxyManager.instance.galaxyMaps[0].galaxy_Index_To_Map[locate].GetComponent<ScrollRect>()
            .content.childCount);
        }
        
        Debug.Log(locate);
        Debug.Log(number);

        Destroy(GalaxyManager.instance.galaxyMaps[0].galaxy_Index_To_Map[locate].GetComponent<ScrollRect>().content
            .GetChild(number).gameObject);

        GameObject userPlanet = Instantiate(GalaxyManager.instance.planet_Prefabs[1],
            GalaxyManager.instance.galaxyMaps[0].galaxy_Index_To_Map[locate].GetComponent<ScrollRect>().content);

        userPlanet.transform.SetSiblingIndex(number);



        /*GalaxyManager.instance.galaxyMaps[0].galaxy_Index_To_Map*/
        /*Instantiate(GalaxyManager.instance.planet_Prefabs[1],
            GalaxyManager.instance.galaxyMaps[0].galaxy_Index_To_Map[0].transform.parent);*/
    }
}
