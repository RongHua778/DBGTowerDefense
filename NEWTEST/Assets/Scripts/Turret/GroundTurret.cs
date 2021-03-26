using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTurret : Turret
{

    [SerializeField] protected float sputteringRange = 0f;

    public float SputteringRange { get => sputteringRange; set => sputteringRange = value; }

    public override void ReadCardAsset(CardSO cardSO)
    {
        base.ReadCardAsset(cardSO);
        sputteringRange = _cardAsset.SputteringRange;
    }

}
