using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class values : SerializableDictionary<string, GameObject> { };
public class BuildDetailMatter : MonoBehaviour
{
    public Infomations infos;
    ImageSlide imgSlide;
    public float buildTimer;
    public float targetTimer;
    public bool confirm;

    Coroutine coroutine;

    //아래로는 함선생산 관련 변수



    void Awake()
    {
        infos = transform.GetChild(2).GetComponent<Infomations>();
        imgSlide = GetComponent<ImageSlide>();

    }

    


    public void Upgrade()
    {
        confirm = !confirm;
        BuildManager.instance.upgraing = confirm;

        BuildManager.instance.ControlCenter_Upgrade(transform.GetChild(1).GetComponent<Image>().sprite, infos, imgSlide, confirm);
    }

}
