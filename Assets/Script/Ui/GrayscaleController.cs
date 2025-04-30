using UnityEngine;

public class GrayscaleController : MonoBehaviour
{
    public Material grayscaleMaterial;
    int grayscaleCount = 0;

    public void SetGrayscale(float amount)
    {
        grayscaleMaterial.SetFloat("_GrayAmount", amount);
    }

    public void BtnClick()
    {
        if (grayscaleCount == 0)
        {
            grayscaleCount = 1;
            SetGrayscale(grayscaleCount);
        }
        else
        {
            grayscaleCount = 0;
            SetGrayscale(grayscaleCount);
        }
    }
}
