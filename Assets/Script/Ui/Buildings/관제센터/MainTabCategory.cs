using UnityEngine;
using UnityEngine.UI;

public class MainTabCategory : MonoBehaviour
{
    public enum Category { Planet, Resource_Building, General_Building, Reserch, Ship_Building, ControlCenter }
    public Category category;
    public GameObject upgrading_Image;
    public GameObject iconContainer;
    public GameObject sliderContainer;
    public BuildResource buildResource; // private���� ���� ��ü

    Button btn;

    void Awake()
    {
        switch (category)
        {
            case Category.Resource_Building:
            case Category.General_Building:
                upgrading_Image = transform.GetChild(1).gameObject;
                iconContainer = transform.parent.parent.GetChild(1).gameObject;
                sliderContainer = transform.GetChild(2).gameObject;
                break;
            case Category.Reserch:
            case Category.Ship_Building:
            case Category.ControlCenter:
                upgrading_Image = transform.GetChild(0).GetChild(0).GetChild(1).gameObject;
                iconContainer = transform.GetChild(1).GetChild(0).gameObject;
                sliderContainer = transform.GetChild(1).GetChild(1).gameObject;
                break;
        }

        btn = GetComponent<Button>();
        /*btn.onClick.AddListener(Build_Manager.instance.InfomationUpdateForUpgrade);*/
    }

    // �������� ���� ���� �� �̹��� ���� ����
    public GameObject Upgrading(Sprite img, bool upgrading, bool anotherTab)
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
            return gameObject;
        }

        else
        {
            //�̹��� �޾ƿ��°ɷ� upgrading_Image �����ϰ� ������Ʈ Ȱ��ȭ
            upgrading_Image.SetActive(false);

            //�����̵� �����̳� ǥ���ϰ� �����̵� �� ��ȯ�� ���� ����� ����
            sliderContainer.SetActive(false);

            //������ �����̳� Ȱ��ȭ ó���ϰ�
            if (!anotherTab)
                iconContainer.SetActive(true);
            return null;
        }
    }
}
