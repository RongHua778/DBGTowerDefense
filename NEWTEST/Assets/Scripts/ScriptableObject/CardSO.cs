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

public enum EffectType
{
    Strength,
    SpeedUp,
    LongSighted,
    SlowDown
}

public enum NoTargetEffectType
{
    Overload
}

[System.Serializable]
public class TargetEffectConfig
{
    public EffectType EffectType;
    public int Stacks;
    public float Duration;
}

[System.Serializable]
public class AttackEffectConfig
{
    public AttackEffectType AttackEffectType;
    public float Value;
}

[System.Serializable]
public class NoTargetEffectConfig
{
    public NoTargetEffectType NoTargetEffectType;
    public float Duration;
    public float KeyValue;
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
    public List<AttackEffectConfig> AttackEffectBuffList;

    [Header("MagicCard Info")]
    public float MagicDamage;
    public float MagicRange;
    public List<TargetEffectConfig> TargetEffectList;

    [Header("NoTargetMagicCard Info")]
    public List<NoTargetEffectConfig> NoTargetEffectList;

    //public CardSO CreateNewInstance()
    //{
    //    var cardSO = CreateInstance<CardSO>();
    //    cardSO.CardType = this.CardType;
    //    cardSO.CardName = this.CardName;

    //    return cardSO;
    //}

    //�����Ҫ��ȡ����ASSET���б��������·���
    //    Resources.LoadAll();
    //    Directory.GetFiles();
    //    int currentIndex = EditorGUILayout.Popup//�������CUSTOM EDITOR�д���һ���б�
}
