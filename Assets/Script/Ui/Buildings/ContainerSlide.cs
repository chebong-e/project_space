using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContainerSlide : MonoBehaviour
{
    public Animator[] anims;
    public Base_Infomation Infomation;
    int grayscaleCount = 0;
    Image myImage;
    Material grayMat, colorMat;
    public Button imgBtn;
    

    public bool imgOpen;
    public bool confirm { get; private set; }
    

    void Awake()
    {
        anims = GetComponentsInChildren<Animator>(true);
        imgBtn = transform.GetChild(1).GetComponent<Button>();
        imgBtn.onClick.AddListener(Container_Sliding);
    }

    public void Init_Setting() // 머티리얼 복사 및 할당 관련
    {
        Infomation = GetComponent<Base_Infomation>();
        myImage = transform.GetChild(1).GetComponent<Image>();
        Material baseMat = Resources.Load<Material>("GrayscaleMaterial");

        colorMat = new Material(baseMat);
        colorMat.SetFloat("_GrayAmount", 0f);

        grayMat = new Material(baseMat);
        grayMat.SetFloat("_GrayAmount", 1f);

        myImage.material = grayMat;

        myImage.sprite = Infomation.shipBuildSlider ?
            Infomation.ship.img : Infomation.buildResource.img;
        /*switch (con_Infomation.info_types)
        {
            case Con_Infomation.Types.Tab4:
                myImage.sprite = con_Infomation.ship.img;
                break;
            case Con_Infomation.Types.ControlCenter:
                myImage.sprite = con_Infomation.buildResource.img;
                break;
        }*/


    }

    // 해금에 따른 컬러 효과전환
    public void Init_ColorSetting(bool unLock)
    {
        if (unLock)
        {
            myImage.material = colorMat;
        }
    }

    public void ColorChange() //컬러 <-> 흑백 전환
    {
        grayscaleCount = 1 - grayscaleCount; // 0 <-> 1 전환

        if (grayscaleCount == 1)
        {
            myImage.material = grayMat;
        }
        else
        {
            myImage.material = colorMat;
        }
    }

    public void ColorChange_To_Upgrade(int index)
    {
        confirm = !confirm;
        /* 예를 들어 관제센터에서 업그레이드를 했다면 
            관제센터의 자식 컨테이너 5종 모두의 컬러를 흑백 및 비활성화 해야함.*/

        List<ContainerSlide> active_containerGroup =
                Build_Manager.instance.GetTargetListByIndex(index);

        foreach (ContainerSlide con in active_containerGroup)
        {
            if (con == this)
            {
                continue;
            }

            if (con.Infomation.unLock == true)
            {
                con.ColorChange();
            }

            con.transform.GetChild(1).GetComponent<Button>().enabled = confirm ? false : true;
        }
    }


    //이미지 클릭시 슬라이드 오픈&클로즈 되는 효과
    public void Container_Sliding()
    {
        if (imgOpen)
            Slide_Close();
        else
        {
            Slide_Open();

            if (Infomation.tabs == Base_Infomation.Tabs.Tab4)
            {
                Infomation.shipBuildSlider.slider.value =
                Infomation.ship.maxHaveShip_Amount - Infomation.ship.currentHave_Ship >= 1 ? 1 : 0;
            }
        }

        // 현재 클릭한 이미지만 열고 나머지는 다 닫기효과
        foreach (ContainerSlide cont in 
            Build_Manager.instance.GetTargetListByIndex(
                Build_Manager.instance.Active_TabContainerIndex()))
        {
            if (cont == this)
            {
                continue;
            }
            else
            {
                if (cont.imgOpen)
                {
                    cont.Slide_Close();
                }
            }
        }
        
    }

    //이미지 슬라이드 닫기기 upslider는 활성화
    //슬라이드클로즈 애니메이션 안에 함수 실행 트리거가 배치되어 있음
    public void Slide_Close()
    {
        imgOpen = false;
        foreach (Animator anim in anims)
        {
            anim.SetTrigger("Close");
        }
    }

    // 이미지 슬라이드 오픈시 upslider는 비활성화
    public void Slide_Open()
    {
        imgOpen = true;
        foreach (Animator anim in anims)
        {
            anim.SetTrigger("Open");
        }
        //업그레이드 중이라면 윗쪽 슬라이더 활성화
        Slider_On_Off();
    }

    // 이미지 슬라이드시 슬라이더 표시 관련
    public void Slider_On_Off()
    {
        /*if (Infomation.controlCenter_confirm)
        {
            Infomation.timeSlider[0].gameObject.SetActive(!imgOpen);
        }*/
        if (Infomation.confirm)
        {
            Infomation.timeSlider[0].gameObject.SetActive(!imgOpen);
        }

    }

}
