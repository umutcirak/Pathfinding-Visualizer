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

    private void Awake()
    {
        graph = FindObjectOfType<Graph>();
        graphManager = FindObjectOfType<GraphManager>();
        graphVisualizer = FindObjectOfType<GraphVisualizer>();
    }


    public void BuildPath()
    {
        visitedNodes.Clear();
        unvisitedNodes.Clear();

        StartCoroutine(Search());      
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
        startNode.previous = nullNodeID;
        visitedNodes.Add(startNode.id,startNode);
        unvisitedNodes.Remove(startNode.id);

        List<GraphNode> startNeighbours = graph.GetNeighborNodes(startNode.id);

        foreach (GraphNode neighbor in startNeighbours)
        {
            int weight = graph.GetWeight(startNode.id, neighbor.id);
            graph.graphNodes[neighbor.id].shortestDistance = weight;
            neighbor.previous = startNode.id;
        }


        GraphNode currentNode = GetMinDistantNode();

        if (currentNode != null)
        {
            while ((currentNode != null && currentNode.id != graphManager.targetNode.id)
           || unvisitedNodes.Count > 0)
            {
                // Visualize Explored Node
                graphVisualizer.VisualizeExploredNode(currentNode);
                yield return new WaitForSeconds(graphVisualizer.waitForExplore);
                //


                List<GraphNode> neighbors = graph.GetNeighborNodes(currentNode.id);

                foreach (GraphNode neighbor in neighbors)
                {
                    int distanceFromCurrent = currentNode.shortestDistance + graph.GetWeight(currentNode.id, neighbor.id);

                    if (distanceFromCurrent < neighbor.shortestDistance)
                    {
                        graph.graphNodes[neighbor.id].shortestDistance = distanceFromCurrent;
                        neighbor.previous = currentNode.id;
                    }
                }

                visitedNodes.Add(currentNode.id, currentNode);
                unvisitedNodes.Remove(currentNode.id);

                currentNode = GetMinDistantNode();
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
            return null;
        }

        return graph.graphNodes[minNodeID];


    }



}