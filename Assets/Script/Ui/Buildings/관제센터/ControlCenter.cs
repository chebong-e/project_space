using UnityEngine;
using UnityEngine.UI;

public class ControlCenter : MonoBehaviour
{
    public GameObject[] upgraing_Image;
    public GameObject iconContainer;
    public GameObject sliderContainer;

    void Awake()
    {
        upgraing_Image = new GameObject[3];
        upgraing_Image[0] = transform.GetChild(0).GetChild(0).GetChild(1).gameObject;
        iconContainer = transform.GetChild(1).GetChild(0).gameObject;
        sliderContainer = transform.GetChild(1).GetChild(1).gameObject;
    }

    public GameObject Upgrading(Sprite img, bool upgrading)
    {
        if (upgrading)
        {
            //이미지 받아오는걸로 upgraing_Image 변경하고 오브젝트 활성화
            upgraing_Image[0].SetActive(true);
            upgraing_Image[0].GetComponent<Image>().sprite = img;

            //슬라이드 컨테이너 표시하고 슬라이드 값 반환을 통해 밸류값 조절
            sliderContainer.SetActive(true);

            //아이콘 컨테이너 비활성화 처리하고
            iconContainer.SetActive(false);
            return sliderContainer;
        }

        else
        {
            //이미지 받아오는걸로 upgraing_Image 변경하고 오브젝트 활성화
            upgraing_Image[0].SetActive(false);

            //슬라이드 컨테이너 표시하고 슬라이드 값 반환을 통해 밸류값 조절
            sliderContainer.SetActive(false);

            //아이콘 컨테이너 비활성화 처리하고
            iconContainer.SetActive(true);
            return null;
        }
        

        
    }
}
