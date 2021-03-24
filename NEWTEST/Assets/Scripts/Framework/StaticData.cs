using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StaticData : Singleton<StaticData>
{
    public int BasicIncome = 1;
    public float BasicIncomeInterval = 2f;

    public static int RandomNumber(float[] pros)
    {
        float total = 0f;
        foreach (float elem in pros)
        {
            total += elem;
        }
        float randomPoint = Random.value * total;
        for (int i = 0; i < pros.Length; i++)
        {
            if (randomPoint < pros[i])
            {
                return i;
            }
            else
            {
                randomPoint -= pros[i];
            }
        }
        return pros.Length - 1;

    }

}
