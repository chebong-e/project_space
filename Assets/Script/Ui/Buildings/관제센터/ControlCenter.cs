using UnityEngine;
using UnityEngine.UI;

public class ControlCenter : MonoBehaviour
{
    public GameObject upgraing_Image;
    public GameObject iconContainer;
    public GameObject sliderContainer;
    public BuildResource buildResource; // private���� ���� ��ü

    void Awake()
    {
        upgraing_Image = transform.GetChild(0).GetChild(0).GetChild(1).gameObject;
        iconContainer = transform.GetChild(1).GetChild(0).gameObject;
        sliderContainer = transform.GetChild(1).GetChild(1).gameObject;
    }

    public GameObject Upgrading(Sprite img, bool upgrading, BuildResource br)
    {
        if (upgrading)
        {
            //Ȯ�ο� �ڵ�(5.27�߰�)
            buildResource = br;

            //�̹��� �޾ƿ��°ɷ� ���� ���� upgrading_Image �����ϰ� ������Ʈ Ȱ��ȭ
            upgraing_Image.SetActive(true);
            upgraing_Image.GetComponent<Image>().sprite = img;

            //�����̵� �����̳� ǥ���ϰ� �����̵� �� ��ȯ�� ���� ����� ����
            sliderContainer.SetActive(true);

            //������ �����̳� ��Ȱ��ȭ ó���ϰ�
            iconContainer.SetActive(false);
            return sliderContainer;
        }

        else
        {
            //�̹��� �޾ƿ��°ɷ� upgraing_Image �����ϰ� ������Ʈ Ȱ��ȭ
            upgraing_Image.SetActive(false);

            //�����̵� �����̳� ǥ���ϰ� �����̵� �� ��ȯ�� ���� ����� ����
            sliderContainer.SetActive(false);

            //������ �����̳� Ȱ��ȭ ó���ϰ�
            iconContainer.SetActive(true);
            return null;
        }
        

        
    }
}
