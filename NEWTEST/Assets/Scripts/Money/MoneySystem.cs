using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneySystem : MonoBehaviour
{
    private float _incomeTimer;

    private static int _currentMoney;

    private static int _maxMoney = 10;

    public static int MaxMoney
    {
        get => _maxMoney;
        set => _maxMoney = value;
    }
    public static int CurrentMoney
    {
        get => _currentMoney;
        set
        {
            _currentMoney = Mathf.Min(MaxMoney, value);
            LevelUIManager.Instance.UpdateMoneyTxt(_currentMoney);
        }
    }

    private void Start()
    {
        CurrentMoney = StaticData.Instance.StartMoney;
    }

    public static void ReduceMoney(int amount)
    {
        CurrentMoney -= amount;
        GameEvents.Instance.MoneySpend(-amount);//这是负值
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
