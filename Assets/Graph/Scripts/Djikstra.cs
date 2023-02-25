using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Djikstra : MonoBehaviour
{
   

    Dictionary<int,GraphNode> visitedNodes = new Dictionary<int, GraphNode>();     // id, node
    Dictionary<int, GraphNode> unvisitedNodes = new Dictionary<int, GraphNode>();    // id

    GraphNode startNode;

    int nullNodeID = -1;

    public List<GraphNode> path = new List<GraphNode>();

    Graph graph;
    GraphManager graphManager;
    GraphVisualizer graphVisualizer;

    public Coroutine coroutine;

    private void Awake()
    {
        graph = FindObjectOfType<Graph>();
        graphManager = FindObjectOfType<GraphManager>();
        graphVisualizer = FindObjectOfType<GraphVisualizer>();
    }
   

    public void BuildPath()
    {
        coroutine = null;
        visitedNodes.Clear();
        unvisitedNodes.Clear();

        coroutine = StartCoroutine(Search());      
    }

    IEnumerator Search()
    {
        graphManager.isRunning = true;

        startNode = graphManager.startNode;        

        foreach (var item in graph.graphNodes)
        {
            GraphNode node = item.Value;
            unvisitedNodes.Add(node.id,node);
        }

        // SET START NODE
        startNode.shortestDistance = 0;
        StartCoroutine(startNode.SetDistanceText(0f));
        startNode.previous = nullNodeID;
        visitedNodes.Add(startNode.id,startNode);
        unvisitedNodes.Remove(startNode.id);

        List<GraphNode> startNeighbours = graph.GetNeighborNodes(startNode.id);

        foreach (GraphNode neighbor in startNeighbours)
        {
            int weight = graph.GetWeight(startNode.id, neighbor.id);
            graph.graphNodes[neighbor.id].shortestDistance = weight;

            // Visualize Neighbor
            StartCoroutine(neighbor.SetDistanceText(weight));
            StartCoroutine(graphVisualizer.ChangeNeighborColorCO(neighbor));

            neighbor.previous = startNode.id;
        }


        GraphNode currentNode = GetMinDistantNode();
        
        if (currentNode != null)
        {
            while ((graphManager.isRunning && currentNode != null && currentNode.id != graphManager.targetNode.id)
           || unvisitedNodes.Count > 0)
            {
                //if(!graphManager.isRunning) { StopCoroutine(coroutine); }
                if (currentNode == null) { graphManager.isRunning = false; StopCoroutine(coroutine);}
                // Visualize Explored Node
                graphVisualizer.VisualizeExploredNode(currentNode);                
                yield return new WaitForSeconds(graphVisualizer.waitForExplore);
                //


                List<GraphNode> neighbors = graph.GetNeighborNodes(currentNode.id);
                //Debug.Log("Neighbors Count: " + neighbors.Count);
                List<GraphNode> changedNeighbors = new List<GraphNode>();
                List<int> previousDistances = new List<int>();
                for (int i = 0; i < neighbors.Count; i++)                
                {
                    GraphNode neighbor = neighbors[i];
                    
                    int distanceFromCurrent = currentNode.shortestDistance + graph.GetWeight(currentNode.id, neighbor.id);

                    if (distanceFromCurrent < neighbor.shortestDistance)
                    {
                        changedNeighbors.Add(neighbor);
                        previousDistances.Add(neighbor.shortestDistance);
                        float initalDistance = neighbor.shortestDistance;
                        graph.graphNodes[neighbor.id].shortestDistance = distanceFromCurrent;

                        StartCoroutine(graphVisualizer.ChangeNeighborColorCO(neighbor));
                        StartCoroutine(neighbor.SetDistanceText(initalDistance)); // Change Distance Text
                                               
                        neighbor.previous = currentNode.id;
                    }
                }
                //Visualize Neighbors of Explored Node
                //graphVisualizer.VisualizeNeighborOfExploredNode(changedNeighbors, previousDistances);
                //
                visitedNodes.Add(currentNode.id, currentNode);
                unvisitedNodes.Remove(currentNode.id);

                currentNode = GetMinDistantNode();

                //Check Is Paused
                if (graphManager.isPaused)
                {
                    while (graphManager.isPaused)
                    {
                        yield return new WaitForEndOfFrame();
                    }
                }


            }
        }
        


        // Visualize Path
        GetPath();        
        StartCoroutine(graphVisualizer.VisualizePathCO(path));
        //graphManager.PrintPath(path);
    }



    List<GraphNode> GetPath()
    {
        path.Clear();

        GraphNode current = graph.graphNodes[graphManager.targetNode.id];
       
        while(current.previous != nullNodeID)
        {
            current.isPath = true;
            path.Add(current);
            current = graph.graphNodes[current.previous];
        }
        path.Add(graph.graphNodes[graphManager.startNode.id]);
        graph.graphNodes[graphManager.startNode.id].isPath = true;

        path.Reverse();
        return path;
    }


    GraphNode GetMinDistantNode()
    {
        int minDistance = 999;
        int minNodeID = nullNodeID;

        foreach (KeyValuePair<int,GraphNode> item in unvisitedNodes)
        {
            GraphNode node = item.Value;

            if(node.shortestDistance < minDistance)
            {
                minDistance = node.shortestDistance;
                minNodeID = node.id;
            }
        }
        
        if(minNodeID == nullNodeID)
        {
            /*
               GraphNode node = null;
            foreach (KeyValuePair<int, GraphNode> item in unvisitedNodes)
            {
                node = item.Value;
                break;
            }
                return node;
             */
            return null;
        }

        return graph.graphNodes[minNodeID];


    }



}