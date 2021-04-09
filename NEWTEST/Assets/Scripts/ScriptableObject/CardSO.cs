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
    Fly,
    Wave
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
    public CardEffectConfig RebuildEffectList;
    public CardEffectConfig PlayEffectList;

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
    public GameObject HitEffectPrefab;
    //public List<AttackEffectConfig> AttackEffectBuffList;

    [Header("MagicCard Info")]
    public float MagicDamage;
    public float MagicRange;
    //public List<AttributeEffectConfig> TurretBuffList;
    //public List<EnemyBuffConfig> EnemyBuffList;

    //[Header("NoTargetMagicCard Info")]
    //public List<NoTargetEffectConfig> NoTargetEffectList;

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
