using UnityEngine;
using UnityEngine.UI;

public class ControlCenter : MonoBehaviour
{
    public GameObject[] upgraing_Image;
    public GameObject iconContainer;
    public GameObject sliderContainer;

    void Awake()
    {
        upgraing_Image = new GameObject[3];
        upgraing_Image[0] = transform.GetChild(0).GetChild(0).GetChild(1).gameObject;
        iconContainer = transform.GetChild(1).GetChild(0).gameObject;
        sliderContainer = transform.GetChild(1).GetChild(1).gameObject;
    }

    public GameObject Upgrading(Sprite img, bool upgrading)
    {
        if (upgrading)
        {
            //�̹��� �޾ƿ��°ɷ� upgraing_Image �����ϰ� ������Ʈ Ȱ��ȭ
            upgraing_Image[0].SetActive(true);
            upgraing_Image[0].GetComponent<Image>().sprite = img;

            //�����̵� �����̳� ǥ���ϰ� �����̵� �� ��ȯ�� ���� ����� ����
            sliderContainer.SetActive(true);

            //������ �����̳� ��Ȱ��ȭ ó���ϰ�
            iconContainer.SetActive(false);
            return sliderContainer;
        }

        else
        {
            //�̹��� �޾ƿ��°ɷ� upgraing_Image �����ϰ� ������Ʈ Ȱ��ȭ
            upgraing_Image[0].SetActive(false);

            //�����̵� �����̳� ǥ���ϰ� �����̵� �� ��ȯ�� ���� ����� ����
            sliderContainer.SetActive(false);

            //������ �����̳� ��Ȱ��ȭ ó���ϰ�
            iconContainer.SetActive(true);
            return null;
        }
        

        
    }
}
