using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphManager : MonoBehaviour
{

    public GraphNode startNode;

    public GraphNode targetNode;

    Graph graph;
    Djikstra djikstra;
    GraphVisualizer graphVisualizer;
    GraphManager graphManager;

    public bool isRunning = false;

    private void Awake()
    {
        graph = FindObjectOfType<Graph>();
        djikstra = FindObjectOfType<Djikstra>();
        graphVisualizer = FindObjectOfType<GraphVisualizer>();
        graphManager = FindObjectOfType<GraphManager>();
    }
        
    private void Update()
    {
        ResetSelectedNodes();
    }

    public void Run()
    {
        if(isRunning) { return; }

        if(startNode != null && targetNode != null)
        {
            graph.ResetGraph();
            graphVisualizer.SetStartColors();
            djikstra.BuildPath();
            // PrintPath(djikstra.path);            
        }
        
    }

    public void PrintPath(List<GraphNode> path)
    {
        Debug.Log("Path Length:" + path.Count);

        for (int i = 0; i < path.Count; i++)
        {
            Debug.Log(path[i].id + " ");
        }
    }
   

    
    void ResetSelectedNodes()
    {
        if(Input.GetMouseButtonDown(1))
        {
            if(graphManager.isRunning) { return; }
            graph.ResetGraph();
            if(startNode != null)
            {
                startNode.GetComponent<SpriteRenderer>().color = Color.white;
                startNode.lastColor = Color.white;
                startNode = null;
            }
            if(targetNode != null)
            {
                targetNode.GetComponent<SpriteRenderer>().color = Color.white;
                targetNode.lastColor = Color.white;
                targetNode = null;
            }
                   
            
        }        
    }

}
