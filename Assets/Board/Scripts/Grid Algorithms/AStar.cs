using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour
{
    Vector2Int startCoordinate;
    Vector2Int targetCoordinate;

    Node startNode;
    Node targetNode;
    Node currentNode;


    /*
    Vector2Int[] directions = { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left 
    , Vector2Int.up + Vector2Int.right, Vector2Int.up + Vector2Int.left, Vector2Int.down + Vector2Int.right
    , Vector2Int.down + Vector2Int.left};
    */
    Vector2Int[] directions = { Vector2Int.right, Vector2Int.up, Vector2Int.down, Vector2Int.left };

    Dictionary<Vector2Int, Node> openList = new Dictionary<Vector2Int, Node>(); // searching currently
    Dictionary<Vector2Int,Node> closedList = new Dictionary<Vector2Int, Node>(); // searched Before

    List<Node> path;

    GameManager gameManager;
    MovePoint pointMover;
    TileVisualizer tileVisualizer;

    bool isRunning;

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
        CheckDoubleSearch();
       
        path = new List<Node>();

        if (!gameManager.doubleSearch)
        {
            gameManager.Reset();
            Setup();
            StartCoroutine(AStarSearch());
        }
        else
        {
            gameManager.Reset();
            gameManager.secondSearchStarted = false;

            startNode = gameManager.allNodes[gameManager.startTile.coordinate];
            targetNode = gameManager.allNodes[gameManager.stopTile.coordinate];

            startCoordinate = startNode.coordinate;
            targetCoordinate = targetNode.coordinate;

            StartCoroutine(AStarSearch());
        }

    }


    
    IEnumerator AStarSearch()
    {
        openList.Clear();
        closedList.Clear();

        isRunning = true;

        openList.Add(startCoordinate, startNode);

        // Setup Nodes
        /*
        foreach (KeyValuePair<Vector2Int,Node> item in gameManager.allNodes)
        {
            item.Value.g_cost = int.MaxValue;
            item.Value.CalculateFCost();
            item.Value.parent = null;
        }
        */

        startNode.g_cost = 0;
        startNode.h_cost = GetDistanceCost(startNode, targetNode);
        startNode.CalculateFCost();

        // ---------------

        while(isRunning && openList.Count > 0)
        {
            currentNode = GetLowestFScoreNode(openList);

            if (currentNode.coordinate == targetCoordinate)
            {
                break;
            }

            // Visualize Exploration
            if (pointMover.IsPointMoving)
            {
                //yield return null;
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


            

            openList.Remove(currentNode.coordinate);
            if (!closedList.ContainsKey(currentNode.coordinate))
            {
                closedList.Add(currentNode.coordinate, currentNode);

            }

            List<Node> neighbors = GetNeighbors(currentNode.coordinate);

            foreach(Node neighbor in neighbors)
            {
                if (closedList.ContainsKey(neighbor.coordinate)) { continue; }
                              

                int tempGCost = currentNode.g_cost + GetDistanceCost(currentNode, neighbor);

                if(tempGCost < neighbor.g_cost)
                {
                    neighbor.parent = currentNode;
                    neighbor.g_cost = tempGCost;
                    neighbor.h_cost = GetDistanceCost(neighbor, targetNode);
                    neighbor.CalculateFCost();

                    if (!openList.ContainsKey(neighbor.coordinate))
                    {
                        openList.Add(neighbor.coordinate, neighbor);
                    }
                }
            }

            if (currentNode.coordinate == targetNode.coordinate)
            {
                isRunning = false;
            }

        }

        // Out of Open List
        isRunning = false;

        CalculatePath();

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
            StartCoroutine(AStarSearch());
        }

    }


    private List<Node> GetNeighbors(Vector2Int coor)
    {
        List<Node> neighbors = new List<Node>();

        foreach (Vector2Int direction in directions)
        {
            Vector2Int neighborPos = coor + direction;
            if (gameManager.IsWall(neighborPos)) { continue; }

            if (gameManager.allNodes.ContainsKey(neighborPos))
            {
                neighbors.Add(gameManager.allNodes[neighborPos]);
            }
        }

        return neighbors;
    }

    private void CalculatePath()
    {
        //path = new List<Node>();
        /*
        path.Add(targetNode);

        Node current = targetNode;

        while(current.parent != null)
        {
            path.Add(current.parent);
            current = current.parent;
        }
        */

        Node current = gameManager.allNodes[targetCoordinate];

        //DoubleSearch
        if (gameManager.doubleSearch && gameManager.secondSearchStarted &&
            current.coordinate == gameManager.stopTile.coordinate)
        {
            return;
        }

        path.Add(current);
        current.isPath = true;

        while (current.parent != null)
        {
            current = current.parent;
            path.Add(current);
            current.isPath = true;
        }
        if ((gameManager.doubleSearch && gameManager.secondSearchStarted) || !gameManager.doubleSearch)
        {
            path.Reverse();
        }             
    }


    private int GetDistanceCost(Node a, Node b)
    {
        int xDistance = Mathf.Abs(a.coordinate.x - b.coordinate.x);
        int yDistance = Mathf.Abs(a.coordinate.y - b.coordinate.y);

        //int remaining = xDistance - yDistance;
        //return 14 * Mathf.Min(xDistance,yDistance) + 10 * remaining;
        return xDistance + yDistance;
    }



    private static Node GetLowestFScoreNode(Dictionary<Vector2Int, Node> list)
    {
        Node lowestFScoreNode = null;
        int lowestFScore = int.MaxValue;

        foreach (KeyValuePair<Vector2Int,Node> item in list)
        {
            if (item.Value.f_cost < lowestFScore)
            {
                lowestFScore = item.Value.f_cost;
                lowestFScoreNode = item.Value;
            }
        }
        return lowestFScoreNode;
    }


}
