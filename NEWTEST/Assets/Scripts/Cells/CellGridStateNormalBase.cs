using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DBGTD.Cells
{
    public class CellGridStateNormalBase : CellGridState
    {
        public CellGridStateNormalBase(CellGrid cellGrid) : base(cellGrid)
        {
        }

        public override void OnCellClicked(Cell cell)
        {
            Debug.Log(cell.OffsetCoord);
        }
    }
}

