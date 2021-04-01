using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneySystem : MonoBehaviour
{
    private float _incomeTimer;

    private static int _currentMoney;
    public static int CurrentMoney
    {
        get => _currentMoney;
        set
        {
            _currentMoney = value;
            LevelUIManager.Instance.UpdateMoneyTxt(value);
        }
    }

    private void Start()
    {
        CurrentMoney = StaticData.Instance.StartMoney;
    }

    public static void ReduceMoney(int amount)
    {
        CurrentMoney -= amount;
        GameEvents.Instance.MoneySpend(-amount);//���Ǹ�ֵ
    }

    public static void AddMoney(int amount)
    {
        CurrentMoney += amount;
        GameEvents.Instance.MoneyIncrease(amount);
    }

    public static bool CanOfferCost(int amount)
    {
        return CurrentMoney >= amount;
    }

    private void Update()
    {
        GetBaiscIncome();
    }

    private void GetBaiscIncome()
    {
        if (Time.time - _incomeTimer >= StaticData.Instance.BasicIncomeInterval)
        {
            CurrentMoney += StaticData.Instance.BasicIncome;
            _incomeTimer = Time.time;
        }
    }
}
