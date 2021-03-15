using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DBGTD.Cells
{
    public class Square : Cell
    {
        List<Cell> neighbours = null;
        protected static readonly Vector2[] _directions =
        {
            new Vector2(1,0),new Vector2(-1,0),new Vector2(0,1),new Vector2(0,-1)
        };

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

        public override List<Cell> GetNeighbours(List<Cell> cells)
        {
            if (neighbours == null)
            {
                neighbours = new List<Cell>(4);
                foreach(var direction in _directions)
                {
                    var neighbour = cells.Find(c => c.OffsetCoord == OffsetCoord + direction);
                    if (neighbour == null) continue;
                    neighbours.Add(neighbour);
                }
            }
            return neighbours;
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
    }
}

