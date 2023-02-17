using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightGenerator : MonoBehaviour
{
    [Header("Edge Possibility for Levels")]
    [SerializeField] [Range(0,100)] int firstLevelPossibility;
    [SerializeField] [Range(0, 100)] int secondLevelPossibility;
    [SerializeField] [Range(0, 100)] int thirdLevelPossibility;


    [Header("Weight Interval for Levels")]
    [SerializeField] Vector2Int firstWeightInterval;
    [SerializeField] Vector2Int secondWeightInterval;
    [SerializeField] Vector2Int thirdWeightInterval;

    Graph graph;
    NeighborGroup neighborGroup;

    private void Awake()
    {
        neighborGroup = FindObjectOfType<NeighborGroup>();
        graph = FindObjectOfType<Graph>();
    }



    public void CreateWeights()
    {
        neighborGroup.CreateLevels();

        foreach (var item in graph.boxes)
        {
            Box currentBox = item.Value;

            CreateWeightForNode(item.Key);
        }

    }

    void CreateWeightForNode(Vector2Int boxCoordinate)
    {
        int randPossibility;

        List<Vector2Int> validFirstLevel = GetValidNeighbors(boxCoordinate, neighborGroup.firstLevel);
        List<Vector2Int> validSecondLevel = GetValidNeighbors(boxCoordinate, neighborGroup.secondLevel);
        List<Vector2Int> validThirdLevel = GetValidNeighbors(boxCoordinate, neighborGroup.thirdLevel);

        GraphNode currentNode = graph.boxes[boxCoordinate].GetComponentInChildren<GraphNode>();

        
        
        
        // First Level
        foreach (Vector2Int neighborBoxCoor in validFirstLevel)
        {
            //DEBUG
            /*
            if(boxCoordinate.x == 1 && boxCoordinate.y == 1)
            {
               Debug.Log("Current Coordinate: " + boxCoordinate + ", Valid Jump: " + neighborBoxCoor);
            }
            */
           

            randPossibility = Random.Range(0, 100);
            if (randPossibility < firstLevelPossibility)
            {
                int weightRand = Random.Range(firstWeightInterval.x, firstWeightInterval.y);
                GraphNode neighborNode = graph.boxes[neighborBoxCoor].GetComponentInChildren<GraphNode>();

                graph.edges[currentNode.id, neighborNode.id] = weightRand;
                graph.edges[neighborNode.id, currentNode.id] = weightRand;

                //Debug.Log("Connection:" + currentNode.id + ", " + neighborNode.id);
            }
        }

        // Second Level
        foreach (Vector2Int neighborBoxCoor in validSecondLevel)
        {
            randPossibility = Random.Range(0, 100);
            if (randPossibility < secondLevelPossibility)
            {
                int weightRand = Random.Range(secondWeightInterval.x, secondWeightInterval.y);
                GraphNode neighborNode = graph.boxes[neighborBoxCoor].GetComponentInChildren<GraphNode>();

                graph.edges[currentNode.id, neighborNode.id] = weightRand;
                graph.edges[neighborNode.id, currentNode.id] = weightRand;
            }
        }

        // Third Level
        foreach (Vector2Int neighborBoxCoor in validThirdLevel)
        {
            randPossibility = Random.Range(0, 100);
            if (randPossibility < thirdLevelPossibility)
            {
                int weightRand = Random.Range(thirdWeightInterval.x, thirdWeightInterval.y);
                GraphNode neighborNode = graph.boxes[neighborBoxCoor].GetComponentInChildren<GraphNode>();

                graph.edges[currentNode.id, neighborNode.id] = weightRand;
                graph.edges[neighborNode.id, currentNode.id] = weightRand;
            }
        }




    }




    void CreateWeightFirst()
    {


    }


    List<Vector2Int> GetValidNeighbors(Vector2Int coordinateCurrent,HashSet<Vector2Int> level)
    {
        List<Vector2Int> validBoxes = new List<Vector2Int>();

        foreach (Vector2Int jump in level)
        {
            Vector2Int newJump = coordinateCurrent + jump;

            if (graph.boxes.ContainsKey(newJump))
            {
                validBoxes.Add(newJump);
            }


        }

        return validBoxes;

    }











}
