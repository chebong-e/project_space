using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ImageSlide : MonoBehaviour
{
    public Animator[] anims;
    EventManager eventManager;
    int grayscaleCount = 0;

    Image myImage;
    Material grayMat;
    Material colorMat;
    public bool open;
    bool confirm;
    Button imgBtn;
    Infomations info;

    ImageSlide curImgSlide;

    void Awake()
    {
        anims = new Animator[2];
        anims = GetComponentsInChildren<Animator>();
        imgBtn = transform.GetChild(1).GetComponent<Button>();
        info = GetComponent<Infomations>();

        imgBtn.onClick.AddListener(ImageSlide_Open_Close);
    }

    public void ColorSetting(bool unlock)
    {
        if (unlock)
            myImage.material = colorMat;
    }

    public void Init_Setting() // 머티리얼 복사 및 할당 관련
    {
        myImage = transform.GetChild(1).GetComponent<Image>();
        Material baseMat = Resources.Load<Material>("GrayscaleMaterial");
        
        colorMat = new Material(baseMat);
        colorMat.SetFloat("_GrayAmount", 0f);

        grayMat = new Material(baseMat);
        grayMat.SetFloat("_GrayAmount", 1f);

        myImage.material = grayMat;
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




    // 버튼 잠금 및 비활성화 처리
    public void ImageChange_toUpgrade(int index) // 업그레이드 중일때 해당 버튼 외 다른 버튼 흑백 및 잠금 처리
    {
        // 활성화 인덱스를 가져와서 
        confirm = !confirm;

        // 예를 들어 관제센터에서 업그레이드를 했다면 관제센터의 자식 컨테이너 5종 모두의 컬러를 흑백 및 비활성화 해야함.
        for (int i = 0; i < BuildManager.instance.TabContainer.Length; i++)
        {
            List<ImageSlide> active_imgGroup = BuildManager.instance.GetTargetListByIndex(index);
            if (i == index)
            {
                foreach(ImageSlide img in active_imgGroup)
                {
                    Debug.Log(img.gameObject.name);
                    if (img.gameObject == gameObject)
                    {
                        continue;
                    }

                    if (img.info.unLock)
                    {
                        img.ColorChange();
                    }

                    img.transform.GetChild(1).GetComponent<Button>().enabled = confirm ? false : true;
                }
            }
        }
    }

    //애니메이션 동작 여부
    public void Sliding_Open()
    {
        foreach (Animator anim in anims)
        {
            anim.SetTrigger("Open");
        }
        open = true;
        SliderOn_Off();
    }

    public void Sliding_Close()
    {
        open = false;
        foreach (Animator anim in anims)
        {
            anim.SetTrigger("Close");
        }
    }

    public void SliderOn_Off() // 오픈은 항상 한 버튼만 유지하기 위함
    {
        if (info.confirm) // 업그레이드 중이라면
        {
            info.timeSlider[0].gameObject.SetActive(!open);
        }
    }

    public void ImageSlide_Open_Close() // 이것으로 대체
    {
        ImageSlide imgslide = GetComponent<ImageSlide>();

        if (imgslide.open)
            imgslide.Sliding_Close();
        else
            imgslide.Sliding_Open();

        foreach (ImageSlide img in BuildManager.instance.imageSliderGroup[BuildManager.instance.AllWindow_Active_IndexCheck()].imageSlide)
        {
            if (img == imgslide)
            {
                continue;
            }
            else
            {
                if (img.open)
                    img.Sliding_Close();
            }
        }
    }
    
}
