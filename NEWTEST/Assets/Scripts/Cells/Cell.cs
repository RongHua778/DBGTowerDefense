using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DBGTD.DataStructs;
using System;

namespace DBGTD.Cells
{
    public abstract class Cell : MonoBehaviour, IGraphNode, IEquatable<Cell>
    {

        private Vector2 _offsetCoord;

        public Vector2 OffsetCoord { get => _offsetCoord; set => _offsetCoord = value; }

        public bool IsTaken;

        public event EventHandler CellClicked;

        public event EventHandler CellHighlighted;
        public event EventHandler CellDehighlighted;

        protected virtual void OnMouseEnter()
        {
            if (CellHighlighted != null)
                CellHighlighted.Invoke(this, new EventArgs());
        }

        protected virtual void OnMouseExit()
        {
            if (CellDehighlighted != null)
                CellDehighlighted.Invoke(this, new EventArgs());
        }

        protected virtual void OnMouseDown()
        {
            if (CellClicked != null)
                CellClicked.Invoke(this, new EventArgs());
        }

        public abstract Vector3 GetCellDimensions();

        /// <summary>
        ///  Method marks the cell to give user an indication that selected unit can reach it.
        /// </summary>
        public abstract void MarkAsReachable();
        /// <summary>
        /// Method marks the cell as a part of a path.
        /// </summary>
        public abstract void MarkAsPath();
        /// <summary>
        /// Method marks the cell as highlighted. It gets called when the mouse is over the cell.
        /// </summary>
        public abstract void MarkAsHighlighted();
        /// <summary>
        /// Method returns the cell to its base appearance.
        /// </summary>
        public abstract void UnMark();
        public virtual bool Equals(Cell other)
        {
            return (OffsetCoord.x == other.OffsetCoord.x && OffsetCoord.y == OffsetCoord.y);
        }



        public override bool Equals(object other)
        {
            if (!(other is Cell))
                return false;
            return Equals(other as Cell);
        }

        public override int GetHashCode()
        {
            int hash = 23;
            hash = (hash * 37) + (int)OffsetCoord.x;
            hash = (hash * 37) + (int)OffsetCoord.y;
            return hash;
        }
        public abstract int GetDistance(Cell other);

        public int GetDistance(IGraphNode other)
        {
            return GetDistance(other as Cell);
        }

        public abstract List<Cell> GetNeighbours(List<Cell> cells);

        public abstract void CopyFields(Cell newCell);

    }
}

