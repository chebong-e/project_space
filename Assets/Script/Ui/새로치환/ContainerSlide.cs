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

    //�̹��� Ŭ���� �����̵� ����&Ŭ���� �Ǵ� ȿ��
    public void ImageSliding()
    {
        if (imgOpen)
            Slide_Open();
        else
            Slide_Close();

        // ���� Ŭ���� �̹����� ���� �������� �� �ݱ�ȿ��
        
    }

    //�̹��� �����̵� �ݱ�� upslider�� Ȱ��ȭ
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
        SliderOn_Off();
    }

    public void SliderOn_Off() // ������ �׻� �� ��ư�� �����ϱ� ����
    {
        /*if (info.confirm) // ���׷��̵� ���̶��
        {
            info.timeSlider[0].gameObject.SetActive(!open);
        }*/
    }
}
