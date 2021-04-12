using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum CardType
{
    Tower,
    Magic,
    NoTargetMagic
}

public enum EffectType
{
    AttributeEffect,
    AttackEffect,
    EnemyBuff,
    NoTargetBuff
}

[System.Serializable]
public struct EffectConfig
{
    public EffectType BaseEffectType;
    public AttributeType AttributeType;
    public AttackEffectType AttackEffectType;
    public EnemyBuffType EnemyBuffType;
    public NoTargetBuffType NoTargetBuffType;
    public float KeyValue;

    public EffectConfig(EffectType baseEffectType, AttributeType attributeType = default, AttackEffectType attackEffectType = default, EnemyBuffType enemyBuffType = default, NoTargetBuffType noTargetBuffType = default, float keyValue = 0)
    {
        this.BaseEffectType = baseEffectType;
        this.AttributeType = attributeType;
        this.AttackEffectType = attackEffectType;
        this.EnemyBuffType = enemyBuffType;
        this.NoTargetBuffType = noTargetBuffType;
        this.KeyValue = keyValue;
    }
}


[CreateAssetMenu(fileName = "New Card", menuName = "DBGTD/CardSO")]
public class CardSO : ScriptableObject
{
    [HideInInspector]
    public CardSO original;
    [Header("General Info")]
    public CardType CardType;
    public string CardName;
    [TextArea(2, 3)]
    public string Description;
    public Sprite CardImage;
    public int CardCost;
    public List<EffectConfig> RemakeEffectList;
    public List<EffectConfig> PoloEffectList;
    public List<EffectConfig> FinalEffectList;


    [Header("TowerCard Info")]
    public GameObject TurretPrefab;
    public float TurretAttack;
    public int TurretRange;
    public float TurretSpeed;
    public float PersistTime;
    public float CriticalRate;
    public ProjectileType ProjectileType;
    public Sprite ProjectileSprite;
    public float SputteringRange;
    public float ProjectileSpeed;
    public GameObject HitEffectPrefab;

    [Header("MagicCard Info")]
    public float MagicAttack;
    public float MagicRange;




    public void BackUpAsset()
    {
        original = Instantiate(this);
    }

    public void ChangeAttribute(EffectConfig config)//这个只影响卡牌的基础属性,不包括之后在地形加成的属性
    {
        switch (config.AttributeType)
        {
            case AttributeType.Cost:
                CardCost += (int)config.KeyValue;
                break;
            case AttributeType.TurretAttack:
                TurretAttack += config.KeyValue;
                break;
            case AttributeType.TurretSpeed:
                TurretSpeed += config.KeyValue;
                break;
            case AttributeType.TurretRange:
                TurretRange += (int)config.KeyValue;
                break;
            case AttributeType.SputterRange:
                SputteringRange += config.KeyValue;
                break;
            case AttributeType.CritcalRate:
                CriticalRate += config.KeyValue;
                break;
            case AttributeType.PersistTime:
                PersistTime += config.KeyValue;
                break;
            case AttributeType.MagicRange:
                MagicRange += config.KeyValue;
                break;
            case AttributeType.MagicAttack:
                MagicAttack += config.KeyValue;
                break;
        }
    }

    public void RemakeTrigger()
    {
        if (RemakeEffectList.Count > 0)
        {
            foreach (EffectConfig config in RemakeEffectList)
            {
                switch (config.BaseEffectType)
                {
                    case EffectType.AttributeEffect:
                        ChangeAttribute(config);
                        break;
                    case EffectType.AttackEffect:
                    case EffectType.EnemyBuff:
                    case EffectType.NoTargetBuff:
                        FinalEffectList.Add(config);
                        break;
                }
            }
            RemakeEffectList.Clear();
        }
    }







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
