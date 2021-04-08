using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RHTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //GetRangeTiles(3);
        UnionTest();
    }
    public void GetRangeTiles(int range)
    {
        int x, y;
        for (x = -range; x <= range; x++)
        {
            for (y = -(range - Mathf.Abs(x)); y <= range - Mathf.Abs(x); y++)
            {
                if (x == 0 && y == 0)
                    continue;
                Debug.Log("x=" + x + " y=" + y);
            }
        }
    }

    public void UnionTest()
    {
        List<int> A = new List<int> { 1, 2 };
        List<int> B = new List<int> { 2, 3 };
        List<int> C = A.Union(B).ToList();
        foreach(var i in C)
        {
            Debug.Log(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
