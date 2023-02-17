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
    MovePoint pointMover;
    TileVisualizer tileVisualizer;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        pointMover = FindObjectOfType<MovePoint>();
        tileVisualizer = FindObjectOfType<TileVisualizer>();
    }  

    void Setup()
    {
        startNode = gameManager.allNodes[gameManager.startTile.coordinate];
        targetNode = gameManager.allNodes[gameManager.targetTile.coordinate];              

        startCoordinate = startNode.coordinate;
        targetCoordinate = targetNode.coordinate;         
    }   

    void CheckDoubleSearch()
    {
        if (gameManager.stopTile != null)
        {
            gameManager.doubleSearch = true;
        }
        else
        {
            gameManager.doubleSearch = false;
        }
    }


    public void BuildPath()
    {
        path = new List<Node>();
        CheckDoubleSearch();
        
        if(!gameManager.doubleSearch)
        {
            gameManager.Reset();
            Setup();
            BreadthFirstSearch();
        }
        else
        {
            gameManager.Reset();
            gameManager.secondSearchStarted = false;

            startNode = gameManager.allNodes[gameManager.startTile.coordinate];
            targetNode = gameManager.allNodes[gameManager.stopTile.coordinate];

            startCoordinate = startNode.coordinate;
            targetCoordinate = targetNode.coordinate;

            BreadthFirstSearch();         

        }
              
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

            // Visualize Exploration
            if (pointMover.IsPointMoving)
            {
                yield return null;
                Tile tile = gameManager.allTiles[currentNode.coordinate];
                Color exploreColor;
                if (gameManager.doubleSearch && gameManager.secondSearchStarted)
                {
                    exploreColor = tileVisualizer.secondVisitedColors
                        [tileVisualizer.secondVisitedColors.Length - 1];
                }
                else
                {
                    exploreColor = tileVisualizer.
                    visitedColors[tileVisualizer.visitedColors.Length - 1];
                }               

                tileVisualizer.ChangeColorRuntime(tile, exploreColor);

            }
            else
            {
                yield return new WaitForSeconds(gameManager.searchWait);
                tileVisualizer.VisualizeExploration(gameManager.allTiles[currentNode.coordinate]);
            }
            // -----------------


            ExploreNeighbors();

            if (currentNode.coordinate == targetNode.coordinate)
            {
                isRunning = false;
            }
        }
        isRunning = false;

        GetPath();
        
        if (pointMover.IsPointMoving)
        {
            tileVisualizer.VisualizePathRuntime(path);
        }
        else if ((gameManager.doubleSearch && gameManager.secondSearchStarted) || !gameManager.doubleSearch)
        {
            StartCoroutine(tileVisualizer.VisualizePathCo(path));
        }

        // Set Second Search
        if (gameManager.doubleSearch && !gameManager.secondSearchStarted)
        {
            startNode = gameManager.allNodes[gameManager.stopTile.coordinate];
            targetNode = gameManager.allNodes[gameManager.targetTile.coordinate];

            startCoordinate = startNode.coordinate;
            targetCoordinate = targetNode.coordinate;

            gameManager.secondSearchStarted = true;
            gameManager.ResetNodes();
            BreadthFirstSearch();            
        }

        


    }


    List<Node> GetPath()
    {
        //path = new List<Node>();

        Node current = gameManager.allNodes[targetCoordinate];

        //DoubleSearch
        if(gameManager.doubleSearch && gameManager.secondSearchStarted &&
            current.coordinate == gameManager.stopTile.coordinate)
        {
            return null;
        }

        path.Add(current);
        current.isPath = true;

        while (current.parent != null)
        {
            current = current.parent;
            path.Add(current);            
            current.isPath = true;
        }
        if( (gameManager.doubleSearch && gameManager.secondSearchStarted) || !gameManager.doubleSearch)
        {
            path.Reverse();
        }        
        return path;        
    }








}
