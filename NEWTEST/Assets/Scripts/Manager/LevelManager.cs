using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//�������ֵ���㣬ʤ�������ж�
public class LevelManager : MonoBehaviour
{
    [SerializeField] private int lives = 10;

    private int _maxLive = 10;
    public int TotalLives { 
        get { return lives; } 
        set 
        {
            if (TotalLives <= 0)
            {
                //game over
            }
            lives = Mathf.Clamp(value, 0, _maxLive); 
        } 
    }

    private void ReduceLives(int live)
    {
        TotalLives -= live;
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }
}
