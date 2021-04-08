using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Deck", menuName = "DBGTD/DeckSO")]
public class DeckSO : ScriptableObject
{
    public CardSO[] DeckCards;
}
