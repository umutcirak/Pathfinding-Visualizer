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
    GraphAlgorithmPicker algoPicker;

    public bool isRunning = false;
    public bool isPaused;

    private void Awake()
    {
        graph = FindObjectOfType<Graph>();
        djikstra = FindObjectOfType<Djikstra>();
        graphVisualizer = FindObjectOfType<GraphVisualizer>();
        algoPicker = FindObjectOfType<GraphAlgorithmPicker>();
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

            switch (algoPicker.selectedAlgorithm)
            {
                case GraphAlgorithmPicker.AlgorithmType.None:
                    return;

                case GraphAlgorithmPicker.AlgorithmType.Djikstra:
                    djikstra.BuildPath();
                    break;
            }
                                   
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

           if(isRunning) { return; }

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
