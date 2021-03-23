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

    //�����Ҫ��ȡ����ASSET���б��������·���
    //    Resources.LoadAll();
    //    Directory.GetFiles();
    //    int currentIndex = EditorGUILayout.Popup//�������CUSTOM EDITOR�д���һ���б�
}
