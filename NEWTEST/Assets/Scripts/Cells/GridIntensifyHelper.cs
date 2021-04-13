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

    private void PrepareIntensify(List<Cell> map)//���������ʼ�ݹ飬ǿ����ǿ����������ÿ��ǿ����Ӧ���Ĺ�����Χʱ�����ñ�ǿ�����������ݹ�
    {

        foreach (Square square in map)//�Ȱ����е��ε�ǿ��Ч������
        {
            square.ResetAllIntensify();
            if (square.SquareTurret != null)
                square.SquareTurret.AlreadyIntensifyList.Clear();//���ø�����ǿ�����ĵ���list

        }
        foreach (Square square in map)
        {
            if (square.SquareTurret != null && square.SquareTurret.AlreadyIntensifyList.Count == 0)//�����������ǿ��list��Ϊ�գ�˵���ѵݹ��������Ҫ�ٿ�ʼ�ݹ�
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
