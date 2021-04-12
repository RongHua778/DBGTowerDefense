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

    public override void LandTurret(Square landedSquare)
    {
        base.LandTurret(landedSquare);
        //foreach (Square square in LandedSquare.GetRangeSquares(1))
        //{
        //    square.RangeIntensify += 1;
        //}
    }

    public override void OnSpawn()
    {
        base.OnSpawn();
    }

    public override void OnUnSpawn()
    {
        //if (LandedSquare != null)
        //{
        //    foreach (Square square in LandedSquare.GetRangeSquares(1))
        //    {
        //        square.RangeIntensify -= 1;
        //    }
        //}
        base.OnUnSpawn();
        
    }

}
