using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public GameObject square;
    public GameObject node;
    Quaternion randomRotation = Quaternion.Euler(0f, 0f, 0f);
    public int width;
    public int height;
    public int nodes;

    void Start()
    {
        if (width < 8) width = 8;
        if (height < 8) height = 8;
        Map map = new Map(width, height,nodes);
        map.generateMap();
        for (int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                if (map.Location[i, j] == 0)
                {
                    Instantiate(square, new Vector3(i, j), randomRotation);
                }
                else
                {
                    Instantiate(node, new Vector3(i, j), randomRotation);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
