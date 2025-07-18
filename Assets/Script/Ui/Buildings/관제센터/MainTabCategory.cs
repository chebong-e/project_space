using UnityEngine;
using UnityEngine.UI;

public class MainTabCategory : MonoBehaviour
{
    public enum Category { Planet, Resource_Building, General_Building, Reserch, Ship_Building, ControlCenter }
    public Category category;
    public GameObject upgrading_Image;
    public GameObject iconContainer;
    public GameObject sliderContainer;
    public BuildResource buildResource; // private으로 나중 교체

    Button btn;

    void Awake()
    {
        switch (category)
        {
            case Category.Resource_Building:
            case Category.General_Building:
                upgrading_Image = transform.GetChild(1).gameObject;
                iconContainer = transform.parent.parent.GetChild(1).gameObject;
                sliderContainer = transform.GetChild(2).gameObject;
                break;
            case Category.Reserch:
            case Category.Ship_Building:
            case Category.ControlCenter:
                upgrading_Image = transform.GetChild(0).GetChild(0).GetChild(1).gameObject;
                iconContainer = transform.GetChild(1).GetChild(0).gameObject;
                sliderContainer = transform.GetChild(1).GetChild(1).gameObject;
                break;
        }

        btn = GetComponent<Button>();
        /*btn.onClick.AddListener(Build_Manager.instance.InfomationUpdateForUpgrade);*/
    }

    // 메인탭의 정보 연동 및 이미지 등을 연동
    public GameObject Upgrading(Sprite img, bool upgrading, bool anotherTab)
    {
        if (upgrading)
        {
            //이미지 받아오는걸로 메인 탭의 upgrading_Image 변경하고 오브젝트 활성화
            upgrading_Image.SetActive(true);
            upgrading_Image.GetComponent<Image>().sprite = img;

            //슬라이드 컨테이너 표시하고 슬라이드 값 반환을 통해 밸류값 조절
            sliderContainer.SetActive(true);

            //아이콘 컨테이너 비활성화 처리하고
            iconContainer.SetActive(false);
            return gameObject;
        }

        else
        {
            //이미지 받아오는걸로 upgrading_Image 변경하고 오브젝트 활성화
            upgrading_Image.SetActive(false);

            //슬라이드 컨테이너 표시하고 슬라이드 값 반환을 통해 밸류값 조절
            sliderContainer.SetActive(false);

            //아이콘 컨테이너 활성화 처리하고
            if (!anotherTab)
                iconContainer.SetActive(true);
            return null;
        }
    }
}
