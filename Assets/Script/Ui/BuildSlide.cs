using UnityEngine;
using UnityEngine.UI;

public class BuildSlide : MonoBehaviour
{
    public Animator[] anims;
    Image myImage;
    int grayscaleCount = 0;

    Material grayMat;
    Material colorMat;

    void Start()
    {
        anims = new Animator[2];
        anims = GetComponentsInChildren<Animator>();
        myImage = transform.GetChild(1).GetComponent<Image>();

        Set_Material();
    }

    void Set_Material()
    {
        Material baseMat = Resources.Load<Material>("GrayscaleMaterial");

        colorMat = new Material(baseMat);
        colorMat.SetFloat("_GrayAmount", 0f);

        grayMat = new Material(baseMat);
        grayMat.SetFloat("_GrayAmount", 1f);

        myImage.material = colorMat;
    }

    void SetGray(Image img, float grayAmount)
    {
        Material templateMat = Resources.Load<Material>("GrayscaleMaterial");
        Material newMat = new Material(templateMat);
        newMat.SetFloat("_GrayAmount", grayAmount);
        img.material = newMat;
    }

    public void ImageClick()
    {
        grayscaleCount = 1 - grayscaleCount; // 0 <-> 1 ÀüÈ¯

        if (grayscaleCount == 1)
        {
            myImage.material = grayMat;
        }
        else
        {
            myImage.material = colorMat;
        }
    }
}
