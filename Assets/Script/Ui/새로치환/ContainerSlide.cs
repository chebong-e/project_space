using UnityEngine;
using UnityEngine.InputSystem;

public class ContainerSlide : MonoBehaviour
{
    public Animator[] anims;
    int grayscaleCount = 0;

    public bool imgOpen;

    void Awake()
    {
        anims = GetComponentsInChildren<Animator>(true);
    }

    //이미지 클릭시 슬라이드 오픈&클로즈 되는 효과
    public void ImageSliding()
    {
        if (imgOpen)
            Slide_Open();
        else
            Slide_Close();

        // 현재 클릭한 이미지만 열고 나머지는 다 닫기효과
        
    }

    //이미지 슬라이드 닫기기 upslider는 활성화
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
        SliderOn_Off();
    }

    public void SliderOn_Off() // 오픈은 항상 한 버튼만 유지하기 위함
    {
        /*if (info.confirm) // 업그레이드 중이라면
        {
            info.timeSlider[0].gameObject.SetActive(!open);
        }*/
    }
}
