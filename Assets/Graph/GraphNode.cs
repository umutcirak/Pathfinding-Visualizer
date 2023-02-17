using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GraphNode : MonoBehaviour
{
    Vector2Int boxCoordinate;

    public int id;
    public bool isExplored;
    public bool isPath;

    public HashSet<GraphNode> neighbors = new HashSet<GraphNode>();
    public List<int> distances = new List<int>();

    //Djikstra
    public int shortestDistance = int.MaxValue;
    public GraphNode graphNode;





    private void Start()
    {
        shortestDistance = int.MaxValue;
    }

    /*
     

    public GraphNode GetMinimumDistanceNode()
    {
        GraphNode minNode = null;
        int minDist = int.MaxValue;

        for (int i = 0; i < distances.Count; i++)
        {
            if (distances[i] < int.MaxValue)
            {
                minDist = distances[i];
                minNode = neighbors[i];
            }
        }
        return minNode;
    }

    */


}
