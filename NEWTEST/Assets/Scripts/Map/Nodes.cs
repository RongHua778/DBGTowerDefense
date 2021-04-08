using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nodes: isData
{
    private int _x;
    private int _y;
    private int _index;
    private int _side;


    public Nodes(int x, int y, int index,int side)
    {
        _x = x;
        _y = y;
        _index = index;
        _side = side;
    }

    public int X
    {
        get => _x;
        set => _x = value;
    }

    public int Y
    {
        get => _y;
        set => _y = value;
    }

    public int index
    {
        get => _index;
        set => _index = value;
    }

    public int side
    {
        get => _side;
        set => _side = value;
    }

    public void returnData()
    {

    }
}

