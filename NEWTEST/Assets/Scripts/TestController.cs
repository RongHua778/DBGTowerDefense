using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DBGTD.Cells;

public class TestController : Singleton<TestController>
{
    public CellGrid _cellGrid;
    public CardSO _turretSO;
    public MoneySystem moneySystem;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //List<Cell> cells = _cellGrid.Cells[13].GetNeighbours(_cellGrid.Cells);
            //foreach (Cell cell in cells)
            //{
            //    Debug.Log(cell.OffsetCoord);
            //}
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GameObject turret = ObjectPool.Instance.Spawn(_turretSO.TurretPrefab);
            //turret.GetComponent<Turret>().ReadCardAsset(_turretSO);

        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            MoneySystem.AddMoney(5);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            MoneySystem.ReduceMoney(5);
        }
    }
}
