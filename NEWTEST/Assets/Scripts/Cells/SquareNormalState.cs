using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DBGTD.Cells
{
    public class SquareNormalState : CellGridState
    {
        public List<Square> highLigtedCells = new List<Square>();
        public SquareNormalState(CellGrid cellGrid) : base(cellGrid)
        {
        }

        public override void OnCellSelected(Cell cell)
        {
            base.OnCellSelected(cell);
            Square square = cell as Square;
            if (square.SquareTurret != null || Square.PreviewingTurret != null)
            {
                int range = Square.PreviewingTurret != null ? Square.PreviewingTurret.AttackRange + square.RangeIntensify : square.SquareTurret.AttackRange;
                highLigtedCells = square.GetRangeSquares(range);
                foreach (var tile in highLigtedCells)
                {
                    tile.MarkAsHighlighted();
                }
            }
            Cell.HighLightedCell = cell;

        }


        public override void OnCellDeselected(Cell cell)
        {
            base.OnCellDeselected(cell);
            foreach (var square in highLigtedCells)
            {
                square.UnMark();
            }
            Cell.HighLightedCell = null;
        }
        public override void OnCellClicked(Cell cell)
        {
            base.OnCellClicked(cell);
        }
    }
}

