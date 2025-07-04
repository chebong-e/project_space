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

    public void Init_Setting() // ��Ƽ���� ���� �� �Ҵ� ����
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

    // �رݿ� ���� �÷� ȿ����ȯ
    public void Init_ColorSetting(bool unLock)
    {
        if (unLock)
        {
            myImage.material = colorMat;
        }
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

    public void ColorChange_To_Upgrade(int index)
    {
        confirm = !confirm;
        /* ���� ��� �������Ϳ��� ���׷��̵带 �ߴٸ� 
            ���������� �ڽ� �����̳� 5�� ����� �÷��� ��� �� ��Ȱ��ȭ �ؾ���.*/

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


    //�̹��� Ŭ���� �����̵� ����&Ŭ���� �Ǵ� ȿ��
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

        // ���� Ŭ���� �̹����� ���� �������� �� �ݱ�ȿ��
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

    //�̹��� �����̵� �ݱ�� upslider�� Ȱ��ȭ
    //�����̵�Ŭ���� �ִϸ��̼� �ȿ� �Լ� ���� Ʈ���Ű� ��ġ�Ǿ� ����
    public void Slide_Close()
    {
        imgOpen = false;
        foreach (Animator anim in anims)
        {
            anim.SetTrigger("Close");
        }
    }

    // �̹��� �����̵� ���½� upslider�� ��Ȱ��ȭ
    public void Slide_Open()
    {
        imgOpen = true;
        foreach (Animator anim in anims)
        {
            anim.SetTrigger("Open");
        }
        //���׷��̵� ���̶�� ���� �����̴� Ȱ��ȭ
        Slider_On_Off();
    }

    // �̹��� �����̵�� �����̴� ǥ�� ����
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
