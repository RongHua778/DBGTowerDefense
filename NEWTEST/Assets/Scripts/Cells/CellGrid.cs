using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DBGTD.Cells
{
    public class CellGrid : MonoBehaviour
    {
        /// <summary>
        /// LevelLoading event is invoked before Initialize method is run.
        /// </summary>
        public event EventHandler LevelLoading;
        /// <summary>
        /// LevelLoadingDone event is invoked after Initialize method has finished running.
        /// </summary>
        public event EventHandler LevelLoadingDone;
        /// <summary>
        /// GameStarted event is invoked at the beggining of StartGame method.
        /// </summary>
        public event EventHandler GameStarted;
        /// <summary>
        /// GameEnded event is invoked when there is a single player left in the game.
        /// </summary>
        public event EventHandler GameEnded;

        public bool GameFinished { get; private set; }

        private const float cellOffset = 1.5f;
        public List<Cell> Cells;

        private CellGridState _cellGridState; //The grid delegates some of its behaviours to cellGridState object.
        public CellGridState CellGridState
        {
            get
            {
                return _cellGridState;
            }
            set
            {
                if (_cellGridState != null)
                    _cellGridState.OnStateExit();
                _cellGridState = value;
                _cellGridState.OnStateEnter();
            }
        }

        private void Start()
        {
            if (LevelLoading != null)
                LevelLoading.Invoke(this, new EventArgs());

            Initialize();

            if (LevelLoadingDone != null)
                LevelLoadingDone.Invoke(this, new EventArgs());

            StartGame();
        }

        private void StartGame()
        {
            if (GameStarted != null)
                GameStarted.Invoke(this, new EventArgs());

            CellGridState = new CellGridStateNormalBase(this);
            Debug.Log("Game started");
        }

        private void Initialize()
        {
            GameFinished = false;
            Cells = new List<Cell>();
            for (int i = 0; i < transform.childCount; i++)
            {
                var cell = transform.GetChild(i).gameObject.GetComponent<Cell>();
                if (cell != null)
                {
                    CorrectCellCoord(cell);
                    Cells.Add(cell);
                }
                else
                    Debug.LogError("在Cellparent中存在不合规的Object");
            }

            foreach (var cell in Cells)
            {
                cell.CellClicked += OnCellClicked;
                cell.CellHighlighted += OnCellHighlighted;
                cell.CellDehighlighted += OnCellDehighlighted;
                cell.GetComponent<Cell>().GetNeighbours(Cells);
            }
        }

        private void CorrectCellCoord(Cell cell)
        {
            Vector2 coord = cell.transform.position;
            coord = new Vector2(coord.x + cellOffset / 2, coord.y + cellOffset / 2);
            float newX = coord.x / cellOffset;
            float newY = coord.y / cellOffset;
            cell.OffsetCoord = new Vector2(newX, newY);
        }

        private void OnCellDehighlighted(object sender, EventArgs e)
        {
            CellGridState.OnCellDeselected(sender as Cell);
        }
        private void OnCellHighlighted(object sender, EventArgs e)
        {
            CellGridState.OnCellSelected(sender as Cell);
        }
        private void OnCellClicked(object sender, EventArgs e)
        {
            CellGridState.OnCellClicked(sender as Cell);
        }
    }
}


