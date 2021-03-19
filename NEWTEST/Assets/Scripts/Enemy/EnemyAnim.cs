using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnim : MonoBehaviour
{
    [SerializeField] private Animator _anim;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void PlayDie()
    {
        _anim.SetTrigger("Die");
    }

    public float GetCurrentAnimationLenght()
    {
        float animationLenght = _anim.GetCurrentAnimatorStateInfo(0).length;
        return animationLenght;
    }

}
