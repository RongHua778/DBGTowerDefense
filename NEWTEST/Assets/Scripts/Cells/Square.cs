using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DBGTD.Cells
{
    public class Square : Cell
    {
        public bool HasTurret;
        public bool IsRoad;
        public Turret SquareTurret;



        [SerializeField]
        private TextMesh rangeInidicator = default;//测试用 范围强化效果指示器

        public float AttackIntensify;
        private int _rangeIntensify;
        public int RangeIntensify
        {
            get => _rangeIntensify;
            set
            {
                int temp = _rangeIntensify;
                _rangeIntensify = value;//先把强化值设定好，防御塔的攻击范围需要取这个值来决定
                if (SquareTurret != null)
                {
                    if (temp < value)
                    {
                        GridIntensifyHelper.TurretApplyPoloEffect(SquareTurret);
                        // SquareTurret.RecaculatePoloEffect();//如果改变后的值大于原先值，则进行一次递归
                        rangeInidicator.text = _rangeIntensify.ToString();
                    }
                    SquareTurret.SetAttackRangeColliders();
                }
                rangeInidicator.text = _rangeIntensify.ToString();
            }
        }
        public float SpeedIntensify;

        public static Turret PreviewingTurret = null;


        public void Start()
        {
            transform.Find("Highlighter").GetComponent<SpriteRenderer>().sortingOrder = 3;
            rangeInidicator.GetComponent<MeshRenderer>().sortingOrder = 5;
        }

        public void ResetAllIntensify()
        {
            AttackIntensify = 0;
            RangeIntensify = 0;
            SpeedIntensify = 0;
        }
        public void ApplyPoloEffect(List<EffectConfig> poloEffectList, bool isRemove)
        {
            foreach (EffectConfig config in poloEffectList)
            {
                if (config.BaseEffectType == EffectType.AttributeEffect)
                {
                    ModifyIntensify(config, isRemove);
                }
            }
        }


        private void ModifyIntensify(EffectConfig config, bool isRemove)
        {
            int flag = isRemove ? -1 : 1;
            switch (config.AttributeType)
            {
                case AttributeType.TurretAttack:
                    AttackIntensify += config.KeyValue * flag;
                    break;
                case AttributeType.TurretSpeed:
                    SpeedIntensify += config.KeyValue * flag;
                    break;
                case AttributeType.TurretRange:
                    RangeIntensify += (int)config.KeyValue * flag;
                    break;
                case AttributeType.SputterRange:
                    break;
                case AttributeType.CritcalRate:
                    break;
                case AttributeType.PersistTime:
                    break;
            }
        }
        public override Vector3 GetCellDimensions()
        {
            var ret = GetComponent<SpriteRenderer>().bounds.size;
            return ret * 0.98f;
        }
        public override int GetDistance(Cell other)
        {
            return (int)(Mathf.Abs(OffsetCoord.x - other.OffsetCoord.x) + Mathf.Abs(OffsetCoord.y - other.OffsetCoord.y));
        }

        public override void GetMap(List<Cell> cells)
        {
            base.GetMap(cells);
        }

        public List<Square> GetRangeSquares(int range)
        {
            range = Mathf.Clamp(range, 0, 20);
            List<Square> squaresToReturn = new List<Square>();
            for (int x = -range; x <= range; x++)
            {
                for (int y = -(range - Mathf.Abs(x)); y <= range - Mathf.Abs(x); y++)
                {
                    if (x == 0 && y == 0)
                        continue;
                    var cell = cellMap.Find(c => c.OffsetCoord == OffsetCoord + new Vector2(x, y));
                    if (cell != null)
                        squaresToReturn.Add(cell as Square);
                }
            }
            return squaresToReturn;
        }

        public override void CopyFields(Cell newCell)
        {
            newCell.OffsetCoord = OffsetCoord;
        }

        public override void MarkAsReachable()
        {
            SetColor(new Color(1, 0.92f, 0.16f, 0.5f));
        }

        public override void MarkAsPath()
        {
            SetColor(new Color(0, 1, 0, 0.5f));
        }

        public override void MarkAsHighlighted()
        {
            SetColor(new Color(0.8f, 0.8f, 0.8f, 0.5f));
        }

        public override void UnMark()
        {
            SetColor(new Color(1, 1, 1, 0));
        }

        private void SetColor(Color color)
        {
            var highlighter = transform.Find("Highlighter");
            var spriteRenderer = highlighter.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.color = color;
            }
        }

        public void SetTurret(Turret turret)
        {
            HasTurret = true;
            SquareTurret = turret;
        }

        public void TurretDemolish()
        {
            HasTurret = false;
            SquareTurret = null;
        }
    }
}

