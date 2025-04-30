using UnityEngine;

public class BuildSlide : MonoBehaviour
{
    public Animator[] anims;

    void Start()
    {
        anims = new Animator[2];
        anims = GetComponentsInChildren<Animator>();
    }
}
