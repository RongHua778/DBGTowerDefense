using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DBGTD.Cells;

public class PoloTurret : Turret
{
    public float AtackIntensify;
    public float RangeIntensify;
    public float SpeedIntensify;

    protected override void RotateTowardsEnemy()
    {

    }

    protected override void FireProjectile()
    {

    }

    public override void LandTurret()
    {
        base.LandTurret();
        foreach (Square square in LandedSquare.neighbours)
        {
            square.AttackIntensify += 1;
        }
    }

    public override void OnSpawn()
    {
        base.OnSpawn();
    }

    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        if (LandedSquare != null)
        {
            foreach (Square square in LandedSquare.neighbours)
            {
                square.AttackIntensify -= 1;
            }
        }
    }

}
