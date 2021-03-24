using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public List<CardSO> _startDeck = new List<CardSO>();

    public List<CardSO> _drawPile = new List<CardSO>();
    public List<CardSO> _discardPile = new List<CardSO>();
    public List<CardSO> _tablePile = new List<CardSO>();
    private void Start()
    {
        GameEvents.Instance.onDiscardCard += AddToDiscardPile;
        InitDeck();
    }

    private void OnDisable()
    {
        GameEvents.Instance.onDiscardCard -= AddToDiscardPile;
    }

    private void InitDeck()
    {
        foreach(var cardSO in _startDeck)
        {
            CardSO newCard = Instantiate(cardSO);
            newCard.name = newCard.CardName;
            _drawPile.Add(newCard);
        }
        DrawPileShuffle();
    }
    public CardSO DrawCard()
    {
        CardSO cardToReturn;
        if (_drawPile.Count > 0)
        {
            cardToReturn = _drawPile[0];
            _drawPile.RemoveAt(0);
            _tablePile.Add(cardToReturn);
            if (_drawPile.Count <= 0)
            {
                DiscardPileBackToDrawPile();
            }
            return cardToReturn;
        }
        else
        {
            Debug.LogWarning("No Card To Return");
            return null;
        }
    }


    public void AddToDiscardPile(CardSO cardSO)
    {
        _tablePile.Remove(cardSO);
        _discardPile.Add(cardSO);
        if (_drawPile.Count <= 0)
            DiscardPileBackToDrawPile();
    }

    public void DrawPileShuffle()
    {
        _drawPile.Shuffle();
    }

    public void DiscardPileShuffle()
    {
        _discardPile.Shuffle();
    }
  
    public void DiscardPileBackToDrawPile()
    {
        foreach(var cardSO in _discardPile)
        {
            _drawPile.Add(cardSO);
        }
        _discardPile.Clear();
        DrawPileShuffle();
    }

}
