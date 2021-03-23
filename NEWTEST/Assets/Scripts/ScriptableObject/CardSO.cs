using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardType
{
    Tower,
    Magic,
    Function
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
    public float AttackDamage;
    public float AttackRange;
    public float AttackSpeed;

    [Header("MagicCard Info")]
    public float MagicDamage;
    public float MagicRange;

    [Header("FunctionCard Info")]
    public float Temp;

    //如果想要读取卡牌ASSET的列表，尝试以下方法
    //    Resources.LoadAll();
    //    Directory.GetFiles();
    //    int currentIndex = EditorGUILayout.Popup//这个用于CUSTOM EDITOR中创建一个列表
}
