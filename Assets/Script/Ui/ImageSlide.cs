using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using static UnityEditor.PlayerSettings;

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

    int Active_Check()
    {
        int index = 0;
        for (int i = 0; i < EventManager.instance.contents.Length; i++)
        {
            if (EventManager.instance.contents[i].gameObject.activeInHierarchy)
            {
                index = i;
                break;
            }
        }
        return index;
    }

    // ��ư ��� �� ��Ȱ��ȭ ó��
    public void ImageChange_toUpgrade() // ���׷��̵� ���϶� �ش� ��ư �� �ٸ� ��ư ��� �� ��� ó��
    {
        int index = 0;
        index = Active_Check();
        Debug.Log(Active_Check());
        // �ӽ÷� �׷� 7����
        List<ImageSlide> img_s = EventManager.instance.imageSliderGroup[index].imageSlide;

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
        SliderOn_Off();
    }

    public void SliderOn_Off()
    {
        if (buildDetailMatter.confirm) // ���׷��̵� ���̶��
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
