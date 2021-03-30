using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum CardType
{
    Tower,
    Magic
}

public enum ProjectileType
{
    Target,
    Ground,
    Fly
}

public enum MagicType
{
    Target,
    NoTarget
}

public enum EffectType
{
    Strength,
    SpeedUp,
    LongSighted,
    SlowDown
}

[System.Serializable]
public class MagicBuff
{
    public EffectType EffectType;
    public int Stacks;
    public float Duration;

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
    public float Range;
    public float Speed;
    public float PersistTime;
    public float CriticalRate;
    public ProjectileType ProjectileType;
    public Sprite ProjectileSprite;
    public float SputteringRange;
    public float ProjectileSpeed;

    [Header("MagicCard Info")]
    public MagicType MagicType;
    public float MagicDamage;
    public float MagicRange;
    public List<MagicBuff> BuffList;

    [Header("FunctionCard Info")]
    public float Temp;

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
