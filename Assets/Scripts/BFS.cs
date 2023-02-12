using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFS : MonoBehaviour
{
    Vector2Int startCoordinate;
    Vector2Int targetCoordinate;

    Node startNode;
    Node targetNode;
    Node currentNode;

    Queue<Node> queue = new Queue<Node>();
    Dictionary<Vector2Int, Node> searchedNodes = new Dictionary<Vector2Int, Node>();

    Vector2Int[] directions = { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };
    // TRAVERSE PRIORITY

    List<Node> path;

    bool isRunning;    

    GameManager gameManager;
    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }  

    void Setup()
    {
        startNode = gameManager.allNodes[gameManager.startTile.coordinate];
        targetNode = gameManager.allNodes[gameManager.targetTile.coordinate];              

        startCoordinate = startNode.coordinate;
        targetCoordinate = targetNode.coordinate;         
    }


    public void BuildPath()
    {
        gameManager.Reset();
        Setup();        
        BreadthFirstSearch();        
    }


    void ExploreNeighbors()
    {
        List<Node> neighbors = new List<Node>();

        foreach (Vector2Int direction in directions)
        {
            Vector2Int neighborPos = currentNode.coordinate + direction;

            if (gameManager.allNodes.ContainsKey(neighborPos))
            {
                neighbors.Add(gameManager.allNodes[neighborPos]);
            }

        }

        foreach (Node neighbor in neighbors)
        {
            
            if (!searchedNodes.ContainsKey(neighbor.coordinate) && neighbor.isWalkable)
            {
                neighbor.parent = currentNode;
                searchedNodes.Add(neighbor.coordinate, neighbor);
                queue.Enqueue(neighbor);
            }

        }
    }

    void BreadthFirstSearch()
    {
        StartCoroutine(BreadtFirstSearchCO());
    }

    IEnumerator BreadtFirstSearchCO()
    {
        queue.Clear();
        searchedNodes.Clear();


        isRunning = true;
        queue.Enqueue(startNode);        
        searchedNodes.Add(startNode.coordinate, startNode);
              

        while (isRunning && queue.Count > 0)
        {
            currentNode = queue.Dequeue();
            currentNode.isExplored = true;

            yield return new WaitForSeconds(gameManager.searchWait);
            gameManager.tileVisualizer.VisualizeExploration(gameManager.allTiles[currentNode.coordinate]);

            ExploreNeighbors();

            if (currentNode.coordinate == targetNode.coordinate)
            {
                isRunning = false;
            }
        }
        isRunning = false;
        GetPath();
        StartCoroutine(gameManager.tileVisualizer.VisualizePathCo(path));
        
    }


    List<Node> GetPath()
    {
        path = new List<Node>();

        Node current = gameManager.allNodes[targetCoordinate];
        path.Add(current);
        current.isPath = true;

        while (current.parent != null)
        {
            current = current.parent;
            path.Add(current);
            Debug.Log("BFS Inside Path Length:" + path.Count);
            current.isPath = true;
        }
        
        path.Reverse();
        return path;
        
    }








}
