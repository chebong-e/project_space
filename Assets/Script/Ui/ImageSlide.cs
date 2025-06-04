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

    public void Init_Setting() // ��Ƽ���� ���� �� �Ҵ� ����
    {
        myImage = transform.GetChild(1).GetComponent<Image>();
        Material baseMat = Resources.Load<Material>("GrayscaleMaterial");
        
        colorMat = new Material(baseMat);
        colorMat.SetFloat("_GrayAmount", 0f);

        grayMat = new Material(baseMat);
        grayMat.SetFloat("_GrayAmount", 1f);

        myImage.material = grayMat;
    }

    public void ColorChange() //�÷� <-> ��� ��ȯ
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
    public void ImageChange_toUpgrade(int index) // ���׷��̵� ���϶� �ش� ��ư �� �ٸ� ��ư ��� �� ��� ó��
    {
        // Ȱ��ȭ �ε����� �����ͼ� 
        confirm = !confirm;

        // ���� ��� �������Ϳ��� ���׷��̵带 �ߴٸ� ���������� �ڽ� �����̳� 5�� ����� �÷��� ��� �� ��Ȱ��ȭ �ؾ���.
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

    //�ִϸ��̼� ���� ����
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

    public void SliderOn_Off() // ������ �׻� �� ��ư�� �����ϱ� ����
    {
        if (info.confirm) // ���׷��̵� ���̶��
        {
            info.timeSlider[0].gameObject.SetActive(!open);
        }
    }

    public void ImageSlide_Open_Close() // �̰����� ��ü
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
