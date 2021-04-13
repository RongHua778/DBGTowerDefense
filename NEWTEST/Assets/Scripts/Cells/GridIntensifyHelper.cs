using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DBGTD.Cells;
using System.Linq;

public class GridIntensifyHelper : MonoBehaviour
{



    public void CaculateIntensifyMap(Turret turret, List<Cell> map)
    {
        if(turret._cardAsset.PoloEffectList.Count>0)
            PrepareIntensify(map);
    }

    private void PrepareIntensify(List<Cell> map)//从这个塔开始递归，强化能强化到的塔，每次强化对应塔的攻击范围时，就让被强化的塔继续递归
    {

        foreach (Square square in map)//先把所有地形的强化效果重置
        {
            square.ResetAllIntensify();
            if (square.SquareTurret != null)
                square.SquareTurret.AlreadyIntensifyList.Clear();//重置该塔已强化过的地形list

        }
        foreach (Square square in map)
        {
            if (square.SquareTurret != null && square.SquareTurret.AlreadyIntensifyList.Count == 0)//如果该塔的已强化list不为空，说明已递归过，不需要再开始递归
                TurretApplyPoloEffect(square.SquareTurret);
        }
    }
    public static void TurretApplyPoloEffect(Turret turret)
    {
        if (turret._cardAsset.PoloEffectList.Count <= 0)
            return;
        List<Square> changeList = turret.LandedSquare.GetRangeSquares(turret.TurretRange);
        changeList = changeList.Except(turret.AlreadyIntensifyList).ToList();
        turret.AlreadyIntensifyList.AddRange(changeList);
        if (changeList.Count > 0)
        {
            foreach (Square square in changeList)
            {
                square.ApplyPoloEffect(turret._cardAsset.PoloEffectList, false);
            }
        }

    }


}
