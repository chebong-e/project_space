using UnityEngine;
using UnityEngine.UI;

public class MainTabCategory : MonoBehaviour
{
    public enum Category { Planet, Building, Reserch, Ship_Building, ControlCenter }
    public Category category;
    public GameObject upgrading_Image;
    public GameObject iconContainer;
    public GameObject sliderContainer;
    public BuildResource buildResource; // private���� ���� ��ü

    void Awake()
    {
        upgrading_Image = transform.GetChild(0).GetChild(0).GetChild(1).gameObject;
        iconContainer = transform.GetChild(1).GetChild(0).gameObject;
        sliderContainer = transform.GetChild(1).GetChild(1).gameObject;
    }

    //���� ��Ʈ�Ѽ��� ���׷��̵� ���� �Լ���
    public GameObject Upgrading(Sprite img, bool upgrading)
    {
        if (upgrading)
        {
            //�̹��� �޾ƿ��°ɷ� ���� ���� upgrading_Image �����ϰ� ������Ʈ Ȱ��ȭ
            upgrading_Image.SetActive(true);
            upgrading_Image.GetComponent<Image>().sprite = img;

            //�����̵� �����̳� ǥ���ϰ� �����̵� �� ��ȯ�� ���� ����� ����
            sliderContainer.SetActive(true);

            //������ �����̳� ��Ȱ��ȭ ó���ϰ�
            iconContainer.SetActive(false);
            return sliderContainer;
        }

        else
        {
            //�̹��� �޾ƿ��°ɷ� upgrading_Image �����ϰ� ������Ʈ Ȱ��ȭ
            upgrading_Image.SetActive(false);

            //�����̵� �����̳� ǥ���ϰ� �����̵� �� ��ȯ�� ���� ����� ����
            sliderContainer.SetActive(false);

            //������ �����̳� Ȱ��ȭ ó���ϰ�
            iconContainer.SetActive(true);
            return null;
        }
        

        
    }


}
