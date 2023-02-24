using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node
{    
    public bool isWalkable;
    public bool isExplored;
    public bool isPath;
    public Node parent;

    public Vector2Int coordinate;


    // A*
    public int g_cost;
    public int h_cost;
    public int f_cost;        

    public Node()
    {
        this.isWalkable = true;
    }


    public void Reset()
    {
        isWalkable = true;
        isExplored = false;
        isPath = false;

        //Common
        parent = null;

        // A*
        g_cost = int.MaxValue;
        CalculateFCost();       
    }

    public void Block()
    {
        isWalkable = false;
    }
   

    // A* Search
    public void CalculateFCost()
    {
        f_cost = h_cost + g_cost;
    }

}
