using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum CardType
{
    Tower,
    Magic,
    NoTargetMagic
}

public enum ProjectileType
{
    Target,
    Ground,
    Fly
}

public enum BuffName
{
    Strength,
    SpeedUp,
    LongSighted,
    Persist,
    SlowDown,
    None
 
}

public enum NoTargetBuffName
{
    Overload,
    Investment,
    MagicMaster,
    FastConveyor
}

[System.Serializable]
public struct BuffConfig
{
    public BuffName BuffName;
    public int Stacks;
    public bool IsInfinity;
    public float Duration;
}


[System.Serializable]
public struct AttackEffectConfig
{
    public AttackEffectType AttackEffectType;
    public float Value;
}

[System.Serializable]
public struct NoTargetEffectConfig
{
    public NoTargetBuffName NoTargetBuffName;
    public float Duration;
    public float KeyValue;

    public NoTargetEffectConfig(NoTargetBuffName name,float duration,float keyValue)
    {
        this.NoTargetBuffName = name;
        this.Duration = duration;
        this.KeyValue = keyValue;
    }
}


[CreateAssetMenu(fileName = "New Card", menuName = "DBGTD/CardSO")]
public class CardSO : ScriptableObject
{
    [Header("General Info")]
    public CardType CardType;
    public string CardName;
    [TextArea(2, 3)]
    public string Description;
    public Sprite CardImage;
    public int CardCost;

    [Header("TowerCard Info")]
    public GameObject TurretPrefab;
    public float Damage;
    public int Range;
    public float Speed;
    public float PersistTime;
    public float CriticalRate;
    public ProjectileType ProjectileType;
    public Sprite ProjectileSprite;
    public float SputteringRange;
    public float ProjectileSpeed;
    public List<AttackEffectConfig> AttackEffectBuffList;

    [Header("MagicCard Info")]
    public float MagicDamage;
    public float MagicRange;
    public List<BuffConfig> TurretBuffList;
    public List<BuffConfig> EnemyBuffList;

    [Header("NoTargetMagicCard Info")]
    public List<NoTargetEffectConfig> NoTargetEffectList;

    //public CardSO CreateNewInstance()
    //{
    //    var cardSO = CreateInstance<CardSO>();
    //    cardSO.CardType = this.CardType;
    //    cardSO.CardName = this.CardName;

    //    return cardSO;
    //}

    //如果想要读取卡牌ASSET的列表，尝试以下方法
    //    Resources.LoadAll();
    //    Directory.GetFiles();
    //    int currentIndex = EditorGUILayout.Popup//这个用于CUSTOM EDITOR中创建一个列表
}
