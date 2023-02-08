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

    public Node()
    {
        this.isWalkable = true;
    }


    public void Reset()
    {
        isWalkable = true;
        isExplored = false;
        isPath = false;
        parent = null;
    }
   
}
