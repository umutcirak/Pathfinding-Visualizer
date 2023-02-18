using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GraphNode : MonoBehaviour
{    
    Vector2Int boxCoordinate;

    public int id;
    public bool isExplored;
    public bool isPath;

    //public HashSet<GraphNode> neighbors = new HashSet<GraphNode>();
    public List<int> distances = new List<int>();

    //Djikstra
    public int shortestDistance = int.MaxValue;
    public GraphNode graphNode;

    GraphVisualizer graphVisualizer;

    private void Awake()
    {
        graphVisualizer = FindObjectOfType<GraphVisualizer>();
    }

    private void Start()
    {
        shortestDistance = int.MaxValue;
    }




    void OnMouseEnter()
    {
        graphVisualizer.Hover(this);
    }

    void OnMouseExit()
    {
        graphVisualizer.HoverExit();
    }

}
