using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;

    void Start()
    {
        RandomTriger();
    }

    void Update()
    {
        
    }

    private void RandomTriger()
    {
        var random = Random.Range(1, 3);
        switch(random)
        {
            case 1:
                animator.SetTrigger("T1");
                break;
            case 2:
                animator.SetTrigger("T2");
                break;
            default:
                break;
        }

        var rand = Random.Range(2f, 5f) + 10;
        DOVirtual.DelayedCall(rand, () =>
        {
            RandomTriger();
        });
    }
}
