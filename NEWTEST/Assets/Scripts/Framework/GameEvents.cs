using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameEvents : Singleton<GameEvents>
{
    // public event Action<int> onEventName;
    // public void EventName(int para)
    // {
    //     if (onEventName != null)
    //         onEventName(para);
    // }

    public event Action onEnemyReach;
    public void EnemyReach()
    {
        onEnemyReach?.Invoke();
    }

    public event Action onEnemyDie;
    public void EnemyDie()
    {
        onEnemyDie?.Invoke();
    }

    public event Action<CardSO> onDiscardCard;
    public void DiscardCard(CardSO cardSO)
    {
        onDiscardCard?.Invoke(cardSO);
    }

    public event Action<int> onMoneySpend;
    public void MoneySpend(int amount)
    {
        onMoneySpend?.Invoke(amount);
    }

    public event Action<int> onMoneyIncrease;

    public void MoneyIncrease(int amount)
    {
        onMoneyIncrease?.Invoke(amount);
    }
}
