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

    void Awake()
    {
        anims = new Animator[2];
        anims = GetComponentsInChildren<Animator>();
        
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

    public void ColorChange()
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

    int Active_Check()
    {
        int index = 0;
        for (int i = 0; i < EventManager.instance.imageSliderGroup.Count; i++)
        {
            List<ImageSlide> img_s = EventManager.instance.imageSliderGroup[i].imageSlide;
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
        int index = 0;
        index = Active_Check();
        // 임시로 그룹 7지정
        List<ImageSlide> img_s = EventManager.instance.imageSliderGroup[index].imageSlide;

        confirm = !confirm;
        foreach (ImageSlide img in img_s)
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

    public void Sliding_Open()
    {
        foreach (Animator anim in anims)
        {
            anim.SetTrigger("Open");
        }
        open = true;
        SliderOn_Off();
    }

    public void SliderOn_Off()
    {
        if (buildDetailMatter.confirm) // 업그레이드 중이라면
        {
            buildDetailMatter.infos.slider[0].gameObject.SetActive(!open);
        }
    }

    public void Sliding_Close()
    {
        open = false;
        foreach (Animator anim in anims)
        {
            anim.SetTrigger("Close");
        }
    }
}
