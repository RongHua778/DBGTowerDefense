using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DBGTD.Cells;

public class TestController : Singleton<TestController>
{
    public CellGrid _cellGrid;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            List<Cell> cells = _cellGrid.Cells[13].GetNeighbours(_cellGrid.Cells);
            foreach(Cell cell in cells)
            {
                Debug.Log(cell.OffsetCoord);
            }
        }
    }
}
