using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DFS : MonoBehaviour
{
    Vector2Int startCoordinate;
    Vector2Int targetCoordinate;

    Node startNode;
    Node targetNode;
    Node currentNode;

    Stack<Node> stack = new Stack<Node>();
    Dictionary<Vector2Int, Node> searchedNodes = new Dictionary<Vector2Int, Node>();

    Vector2Int[] directions = { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };

    List<Node> path;

    GameManager gameManager;
    MovePoint pointMover;


    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        pointMover = FindObjectOfType<MovePoint>();
    }

    // DFS algorithm to find the minimum path from startNode to targetNode
    public List<Node> Search(Node start, Node target)
    {
        startNode = start;
        targetNode = target;

        // Initialize the search by adding the start node to the stack
        stack.Clear();
        searchedNodes.Clear();
        stack.Push(startNode);

        while (stack.Count > 0)
        {
            // Get the next node from the stack and mark it as explored
            currentNode = stack.Pop();
            currentNode.isExplored = true;

            // If we've found the target node, construct and return the path
            if (currentNode == targetNode)
            {
                return ConstructPath(currentNode);
            }

            // Otherwise, add unexplored neighboring nodes to the stack
            foreach (Vector2Int direction in directions)
            {
                Vector2Int neighborCoords = currentNode.coordinate + direction;

                // Make sure the neighbor is within the grid
                if (!searchedNodes.ContainsKey(neighborCoords) && gameManager.allNodes.ContainsKey(neighborCoords))
                {                    
                    // Get the neighbor node and mark it as explored
                    Node neighborNode = gameManager.allNodes[neighborCoords];
                    searchedNodes.Add(neighborCoords, neighborNode);

                    // Only add the neighbor to the stack if it's walkable
                    if (neighborNode.isWalkable)
                    {
                        stack.Push(neighborNode);
                        neighborNode.parent = currentNode;
                    }
                }
            }
        }

        // If we haven't found the target node and there are no more nodes to search, return null
        return null;
    }

    // Construct the path by tracing the parent nodes from the end node to the start node
    List<Node> ConstructPath(Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode.isPath = true;
            currentNode = currentNode.parent;
        }

        path.Add(startNode);
        path.Reverse();

        return path;
    }
}
