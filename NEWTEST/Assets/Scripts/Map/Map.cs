using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map
{
    private int _height;
    private int _width;
    private int _nodesNumber;
    private int positionX;
    private int positionY;

    private int[,] location;
    private Nodes[] nodes;

    public Map(int width, int height, int nodesNumber)
    {
        _height = height;
        _width = width;
        location = new int[width, height];
        _nodesNumber = nodesNumber;
        nodes = new Nodes[nodesNumber * 4];
    }

    public Nodes[] Nodes
    {
        get
        {
            return nodes;
        }
    }
    public int[,] Location
    {
        get
        {
            return location;
        }
    }

    public void generateMap()
    {
        generateNodes();
        drawNodes();
        drawRoad();
        Debug.Log("check:"+checkMap());

        int k = 0;
        while (!checkMap())
        {
            k++;
            generateNodes();
            drawNodes();
            drawRoad();
            Debug.Log(k);
            if (k > 2000) break;
        }
    }

    //在四条边上随机生成nodes的方法
    private void generateNodes()
    {
        int gapL = _height / _nodesNumber;
        int gapW = _width / _nodesNumber;
        int tempX=2;
        int tempY=2;

            for (int i = 0; i < _nodesNumber * 4; i++)
        {
            //计算nodes是上下左右哪条边上的（上下左右分别对应0，1，2，3）
            nodes[i] = new Nodes(1,1,i,(int)i/ _nodesNumber);
            //四条边分别生成随机nodes
            //Debug.Log("side:"+nodes[i].side);
            switch (nodes[i].side)
            {
                case 0:
                    tempX = Mathf.Min((int)Random.Range(gapW / 2, gapW)+tempX, _width-3);
                    nodes[i].X = tempX;
                    nodes[i].Y = Random.Range(1, 3);
                    break;
                case 1:
                    tempY = Mathf.Min((int)Random.Range(gapL / 2, gapL)+tempY, _height - 3);
                    tempX = 2;
                    nodes[i].X = Random.Range(_width - 3, _width - 1);
                    nodes[i].Y = tempY;
                    break;
                case 2:
                    tempY = 2;
                    tempX = Mathf.Min((int)Random.Range(gapW / 2, gapW) + tempX, _width - 4);
                    nodes[i].X = _width-tempX;
                    nodes[i].Y = Random.Range(_height-3, _height-1);
                    break;
                case 3:
                    tempY = Mathf.Min((int)Random.Range(gapL / 2, gapL) + tempY, _height - 4);
                    nodes[i].X = Random.Range(1, 3);
                    nodes[i].Y = _height - tempY;
                    break;
            }
        }
    }

    //在地图中画出nodes
    private void drawNodes()
    {
        for(int i = 0; i < _width; i++)
        {
            for(int j = 0; j < _height; j++)
            {
                location[i, j] = 0;
            }
        }
        
        for(int i=0;i< _nodesNumber*4; i++)
        {
            //Debug.Log("X:"+nodes[i].X+"Y:"+nodes[i].Y);
            location[nodes[i].X, nodes[i].Y] = 1;
        }
    }

    //绘制地图的指针走向node的方法
    private void toNode(int nodeIndex)
    {
        while (positionX != nodes[nodeIndex].X || positionY != nodes[nodeIndex].Y)
        {
            location[positionX, positionY] = 1;
            if (positionX > nodes[nodeIndex].X)
            {
                positionX -= 1;
            }else if (positionX < nodes[nodeIndex].X)
            {
                positionX += 1;
            }
            else
            {
                if (positionY > nodes[nodeIndex].Y)
                {
                    positionY -= 1;
                }
                else if (positionY < nodes[nodeIndex].Y)
                {
                    positionY += 1;
                }
            }
            location[positionX, positionY] = 1;
        }
    }

    //把地图中的nodes连在一起
    private void drawRoad()
    {
        positionX = nodes[0].X;
        positionY = nodes[0].Y;
        for(int i = 0; i < _nodesNumber * 4; i++)
        {
            toNode(i);
        }
        toNode(0);
    }

    //检查地图是否合格的方法
    private bool checkMap()
    {
        for (int i = 1; i < _width-1; i++)
        {
            for (int j = 1; j < _height-1; j++)
            {
                int temp = location[i, j - 1] +
                    location[i, j +1] +
                    location[i - 1, j] +
                    location[i + 1, j]+
                    location[i,j];
                //Debug.Log("temp:" + temp);
                if (temp >= 4)
                {
                    return false;
                }
            }
        }
        return true;
    }
}
