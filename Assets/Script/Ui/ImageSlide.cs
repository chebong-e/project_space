using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ImageSlide : MonoBehaviour
{
    public Animator[] anims;
    Image myImage;
    BuildDetailMatter buildDetailMatter;
    int grayscaleCount = 0;

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

    public void Init_Setting() // ��Ƽ���� ���� �� �Ҵ� ����
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
    /*void Set_Material() // ��Ƽ���� ���� �� �Ҵ� ����
    {
        Material baseMat = Resources.Load<Material>("GrayscaleMaterial");

        colorMat = new Material(baseMat);
        colorMat.SetFloat("_GrayAmount", 0f);

        grayMat = new Material(baseMat);
        grayMat.SetFloat("_GrayAmount", 1f);

        myImage.material = grayMat;
    }*/

    public void ImageClick()
    {
        grayscaleCount = 1 - grayscaleCount; // 0 <-> 1 ��ȯ

        if (grayscaleCount == 1)
        {
            myImage.material = grayMat;
        }
        else
        {
            myImage.material = colorMat;
        }
    }

    // ��ư ��� �� ��Ȱ��ȭ ó��
    public void ImageChange_toUpgrade()
    {
        // �ӽ÷� �׷� 7����
        List<ImageSlide> img_s = EventManager.instance.imageSliderGroup[7].buildSlide;

        confirm = !confirm;
        foreach (ImageSlide img in img_s)
        {
            if (img == GetComponentInParent<ImageSlide>())
            {
                continue;
            }

            if (img.buildDetailMatter.infos.unLock)
            {
                img.ImageClick();
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
    }

    public void Sliding_Close()
    {
        foreach (Animator anim in anims)
        {
            anim.SetTrigger("Close");
        }
        open = false;
    }
}
