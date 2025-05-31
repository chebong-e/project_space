using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ImageSlide : MonoBehaviour
{
    public Animator[] anims;
    EventManager eventManager;
    BuildDetailMatter buildDetailMatter;
    int grayscaleCount = 0;

    Image myImage;
    Material grayMat;
    Material colorMat;
    public bool open;
    bool confirm;
    Button imgBtn;


    ImageSlide curImgSlide;

    void Awake()
    {
        anims = new Animator[2];
        anims = GetComponentsInChildren<Animator>();
        imgBtn = transform.GetChild(1).GetComponent<Button>();

        imgBtn.onClick.AddListener(ImageSlide_Open_Close);
    }

    public void ColorSetting(bool unlock)
    {
        if (unlock)
            myImage.material = colorMat;
    }

    public void Init_Setting() // 머티리얼 복사 및 할당 관련
    {
        buildDetailMatter = GetComponent<BuildDetailMatter>();
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

    /*public int AllWindow_Active_IndexCheck()
    {
        int index = 0;
        for (int i = 0; i < EventManager.instance.imageSliderGroup.Count; i++)
        {
            List<ImageSlide> img_s = EventManager.instance.imageSliderGroup[i].imageSlide;
            if (img_s[0].gameObject.activeInHierarchy)
            {
                index = i;
                break;
            }
        }
        return index;
    }*/

     int ControlCenter_Active_IndexCheck()
    {
        int index = 0;
        for (int i = 0; i < BuildManager.instance.controlCenter_ImageSliderGroup.Count; i++)
        {
            List<ImageSlide> img_s = BuildManager.instance.controlCenter_ImageSliderGroup[i].imageSlide;
            foreach (ImageSlide img in img_s)
            {
                if (img.gameObject == gameObject)//GetComponentInParent<ImageSlide>()
                {
                    index = i;
                    break;
                }
            }
        }
        return index;
    }


    // 버튼 잠금 및 비활성화 처리
    public void ImageChange_toUpgrade() // 업그레이드 중일때 해당 버튼 외 다른 버튼 흑백 및 잠금 처리
    {
        // 활성화 인덱스를 가져와서 
        confirm = !confirm;
        int active_index = ControlCenter_Active_IndexCheck();

        for (int i = 0; i < 5; i++)
        {
            List<ImageSlide> active_imgGroup = BuildManager.instance.controlCenter_ImageSliderGroup[i].imageSlide;
            if (i == active_index)
            {

                foreach (ImageSlide img in active_imgGroup)
                {
                    if (img.gameObject == gameObject)
                    {
                        continue;
                    }

                    if (img.buildDetailMatter.infos.unLock)
                    {
                        img.ColorChange();
                    }

                    img.transform.GetChild(1).GetComponent<Button>().enabled = confirm ? false : true;
                }
            }
            else
            {
                foreach (ImageSlide img in active_imgGroup)
                {

                    if (img.buildDetailMatter.infos.unLock)
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
        if (buildDetailMatter.confirm) // 업그레이드 중이라면
        {
            buildDetailMatter.infos.timeSlider[0].gameObject.SetActive(!open);
        }
    }

    public void ImageSlide_Open_Close() // 이것으로 대체
    {
        ImageSlide imgslide = GetComponent<ImageSlide>();

        if (imgslide.open)
            imgslide.Sliding_Close();
        else
            imgslide.Sliding_Open();

        foreach (ImageSlide img in EventManager.instance.imageSliderGroup[BuildManager.instance.AllWindow_Active_IndexCheck()].imageSlide)
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
