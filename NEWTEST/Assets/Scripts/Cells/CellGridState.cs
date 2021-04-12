using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DBGTD.Cells
{
    public abstract class CellGridState
    {
        protected CellGrid _cellGrid;
        protected CellGridState(CellGrid cellGrid)
        {
            _cellGrid = cellGrid;
        }

        public virtual void OnCellClicked(Cell cell)
        {
        }

        public virtual void OnCellSelected(Cell cell)
        {
            cell.MarkAsHighlighted();
        }

        public virtual void OnCellDeselected(Cell cell)
        {
            cell.UnMark();
        }

        public virtual void OnStateEnter()
        {
            foreach (var cell in CellGrid.Cells)
            {
                cell.UnMark();
            }
        }

        public virtual void OnStateExit()
        {
        }


    }
}

