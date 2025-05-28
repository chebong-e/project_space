using UnityEngine;
using UnityEngine.UI;

public class ControlCenter : MonoBehaviour
{
    public GameObject upgraing_Image;
    public GameObject iconContainer;
    public GameObject sliderContainer;
    public BuildResource buildResource; // private으로 나중 교체

    void Awake()
    {
        upgraing_Image = transform.GetChild(0).GetChild(0).GetChild(1).gameObject;
        iconContainer = transform.GetChild(1).GetChild(0).gameObject;
        sliderContainer = transform.GetChild(1).GetChild(1).gameObject;
    }

    public GameObject Upgrading(Sprite img, bool upgrading, BuildResource br)
    {
        if (upgrading)
        {
            //확인용 코드(5.27추가)
            buildResource = br;

            //이미지 받아오는걸로 메인 탭의 upgrading_Image 변경하고 오브젝트 활성화
            upgraing_Image.SetActive(true);
            upgraing_Image.GetComponent<Image>().sprite = img;

            //슬라이드 컨테이너 표시하고 슬라이드 값 반환을 통해 밸류값 조절
            sliderContainer.SetActive(true);

            //아이콘 컨테이너 비활성화 처리하고
            iconContainer.SetActive(false);
            return sliderContainer;
        }

        else
        {
            //이미지 받아오는걸로 upgraing_Image 변경하고 오브젝트 활성화
            upgraing_Image.SetActive(false);

            //슬라이드 컨테이너 표시하고 슬라이드 값 반환을 통해 밸류값 조절
            sliderContainer.SetActive(false);

            //아이콘 컨테이너 활성화 처리하고
            iconContainer.SetActive(true);
            return null;
        }
        

        
    }
}
