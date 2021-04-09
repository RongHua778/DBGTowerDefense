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

        public float AttackIntensify;
        private int _rangeIntensify;
        public int RangeIntensify
        {
            get => _rangeIntensify;
            set
            {
                _rangeIntensify = value;
                if (SquareTurret != null)
                {
                    SquareTurret.SetAttackRangeColliders();
                }
            }
        }
        public float SpeedIntensify;

        //public List<Cell> neighbours;
        public static Turret PreviewingTurret = null;
        //protected static readonly Vector2[] _directions =
        //{
        //    new Vector2(1,0),new Vector2(-1,0),new Vector2(0,1),new Vector2(0,-1),
        //    new Vector2(1,1),new Vector2(-1,-1),new Vector2(1,-1),new Vector2(-1,1)
        //};

        public void Start()
        {
            transform.Find("Highlighter").GetComponent<SpriteRenderer>().sortingOrder = 3;
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
            //if (neighbours.Count <= 0)
            //{
            //    neighbours = new List<Cell>(8);
            //    foreach (var direction in _directions)
            //    {
            //        var neighbour = cells.Find(c => c.OffsetCoord == OffsetCoord + direction);
            //        if (neighbour == null) continue;
            //        neighbours.Add(neighbour);
            //    }
            //}
            //return neighbours;
        }

        public List<Square> GetRangeSquares(int range)
        {
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

