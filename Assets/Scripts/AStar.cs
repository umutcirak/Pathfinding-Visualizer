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
           

    Vector2Int[] directions = { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left 
    , Vector2Int.up + Vector2Int.right, Vector2Int.up + Vector2Int.left, Vector2Int.down + Vector2Int.right
    , Vector2Int.down + Vector2Int.left};

    Dictionary<Vector2Int, Node> openList; // searching currently
    Dictionary<Vector2Int,Node> closedList; // searched Before

    List<Node> path;

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

    public void BuildPath()
    {
        path = new List<Node>();        

        gameManager.Reset();
        Setup();
        StartCoroutine(AStarSearch());
        AStarSearch();
        //CalculatePath();
        StartCoroutine(tileVisualizer.VisualizePathCo(path));

    }


    
    IEnumerator AStarSearch()
    {
        openList = new Dictionary<Vector2Int, Node>();
        closedList = new Dictionary<Vector2Int, Node>();

        openList.Add(startCoordinate, startNode);

        // Setup Nodes
        foreach (KeyValuePair<Vector2Int,Node> item in gameManager.allNodes)
        {
            item.Value.g_cost = int.MaxValue;
            item.Value.CalculateFCost();
            item.Value.parent = null;
        }

        startNode.g_cost = 0;
        startNode.h_cost = GetDistanceCost(startNode, targetNode);
        startNode.CalculateFCost();

        // ---------------

        while(openList.Count > 0)
        {
            currentNode = GetLowestFScoreNode(openList);

            //  Visualize
            yield return new WaitForSeconds(gameManager.searchWait);
            tileVisualizer.VisualizeExploration(gameManager.allTiles[currentNode.coordinate]);
            //

            if (currentNode.coordinate == targetCoordinate)
            {
                CalculatePath();
                break;
            }

            openList.Remove(currentNode.coordinate);
            closedList.Add(currentNode.coordinate, currentNode);

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

        }

        // Out of Open List



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
        path = new List<Node>();
        path.Add(targetNode);

        Node current = targetNode;

        while(current.parent != null)
        {
            path.Add(current.parent);
            current = current.parent;
        }

        path.Reverse();        
    }


    private int GetDistanceCost(Node a, Node b)
    {
        int xDistance = Mathf.Abs(a.coordinate.x - b.coordinate.x);
        int yDistance = Mathf.Abs(a.coordinate.y - b.coordinate.y);
        int remaining = xDistance - yDistance;

        return 14 * Mathf.Min(xDistance,yDistance) + 10 * remaining;
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
