using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum CardType
{
    Tower,
    Magic,
    NoTargetMagic
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
    public CardEffectConfig RebuildEffectList;
    public CardEffectConfig ComboEffectList;
    public CardEffectConfig FinalEffectList;

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

    public void ChangeAttribute(AttributeConfig config)//这个只影响卡牌的基础属性,不包括之后在地形加成的属性
    {
        switch (config.ChangeAttribute)
        {
            case Attribute.Cost:
                CardCost += (int)config.Value;
                break;
            case Attribute.TurretAttack:
                TurretAttack += config.Value;
                break;
            case Attribute.TurretSpeed:
                TurretSpeed += config.Value;
                break;
            case Attribute.TurretRange:
                TurretRange += (int)config.Value;
                break;
            case Attribute.SputterRange:
                SputteringRange += config.Value;
                break;
            case Attribute.CritcalRate:
                CriticalRate += config.Value;
                break;
            case Attribute.PersistTime:
                PersistTime += config.Value;
                break;
            case Attribute.MagicRange:
                MagicRange += config.Value;
                break;
            case Attribute.MagicAttack:
                MagicAttack += config.Value;
                break;
        }
    }

    public void RebuildTrigger()
    {
        if (RebuildEffectList.AttributeEffects.Count > 0)
        {
            foreach (AttributeConfig config in RebuildEffectList.AttributeEffects)
            {
                ChangeAttribute(config);
            }
        }
        FinalEffectList.AttackEffects.AddRange(RebuildEffectList.AttackEffects);
        FinalEffectList.EnemyBuffs.AddRange(RebuildEffectList.EnemyBuffs);
        FinalEffectList.NoTargetEffects.AddRange(RebuildEffectList.NoTargetEffects);
        RebuildEffectList.AttributeEffects.Clear();
        RebuildEffectList.AttackEffects.Clear();
        RebuildEffectList.EnemyBuffs.Clear();
        RebuildEffectList.NoTargetEffects.Clear();
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
