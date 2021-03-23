using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : ReusableObject
{
    public CardSO CardAsset;
    public Card PreviewCard;
    public GameObject HideWhenDrag;

    [Header("Text Component References")]
    public Text NameText;
    public Text CostText;
    public Text DescriptionText;

    [Header("Image References")]
    public Image CardBodyImage;
    public Image CardGraphicImage;

    public GameObject HandleNode;
    // Start is called before the first frame update

    //private void Awake()
    //{
    //    if (CardAsset != null)
    //        ReadCardFromAsset();
    //}

    public void ReadCardFromAsset()
    {
        NameText.text = CardAsset.CardName;
        CostText.text = CardAsset.CardCost.ToString();
        CardGraphicImage.sprite = CardAsset.CardImage;

        //PreviewSetting
        if (DescriptionText != null)
            DescriptionText.text = CardAsset.Description;

        if (PreviewCard != null)
        {
            PreviewCard.CardAsset = CardAsset;
            PreviewCard.ReadCardFromAsset();
        }
    }

    public void ShowCard()
    {
        HideWhenDrag.SetActive(true);
        this.GetComponent<Collider2D>().enabled = true;

    }

    public void HideCard()
    {
        HideWhenDrag.SetActive(false);
        this.GetComponent<Collider2D>().enabled = false;
    }


    public override void OnSpawn()
    {
        
    }

    public override void OnUnSpawn()
    {
        GameEvents.Instance.DiscardCard(CardAsset);
        ShowCard();
        HandleNode = null;
    }
}
