using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    [SerializeField] GraphNode graphNodePrefab;
    [SerializeField] Line linePrefab;
    [SerializeField] Box box;
   
    public int minWeight;
    public int maxWeight;
    [Range(0.25f, 0.75f)] public float edgePossibility;

    private Dictionary<int, GraphNode> graphNodes = new Dictionary<int, GraphNode>(); // id, node
    public Dictionary<Vector2Int, Box> boxes = new Dictionary<Vector2Int, Box>();
    private Dictionary<Vector2Int, Line> lines = new Dictionary<Vector2Int, Line>();  // nodeId, nodeId, Line

    Vector2Int[] boxDirections = { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left,
    Vector2Int.up + Vector2Int.right, Vector2Int.up + Vector2Int.left, Vector2Int.down, Vector2Int.right,
    Vector2Int.down, Vector2Int.left};


    public int[,] edges;

    [SerializeField] [Range(1, 3)] int graphSize;

    [Header("Box Settings")]
    [Tooltip("Boxes are node containers, Each node in a box can have an edge with nodes" +
        " in neighbor boxes not further away")]
    [SerializeField] int boxCountHorizontal = 16;      // 16 * graphSize
    [SerializeField] int boxCountVertical = 6;         // 6 * graphSize
    [SerializeField] int boxPadding = 3;    // padding to Box Border
    [SerializeField] int boxLength = 12;     // width and height in unit   

    public int UpperHorizontalPos { get {return (boxCountHorizontal * graphSize * boxLength) + boxLength; } }
    public int UpperVerticalPos { get { return (boxCountVertical * graphSize * boxLength) + boxLength; } }
    public int GraphSize { get { return graphSize; } }


    WeightGenerator weightGenerator;
    private void Awake()
    {
        weightGenerator = FindObjectOfType<WeightGenerator>();
    }

    private void Start()
    {
        InitializeEdges();
        CreateBoxes();
        PlaceNodes();       
        weightGenerator.CreateWeights();

        //PrintEdgesofNode(1);

        DrawLines();
    }




    void PlaceNodes()
    {        
        int idNode = 1;

        foreach (KeyValuePair<Vector2Int,Box> item in boxes)
        {
            Box currentBox = item.Value;

            int xLower = currentBox.coordinate.x * boxLength; 
            int xUpper = currentBox.coordinate.x * boxLength + boxLength - boxPadding;
            int yLower = currentBox.coordinate.y * boxLength;
            int yUpper = currentBox.coordinate.y * boxLength + boxLength - boxPadding;

            Vector3 nodePos = GetRandomPos(xLower, xUpper, yLower, yUpper);

            GraphNode newNode = Instantiate(graphNodePrefab, nodePos, Quaternion.identity);
            newNode.transform.parent = currentBox.transform;
            newNode.id = idNode;
            newNode.name = "Node: " + idNode;

            graphNodes.Add(idNode, newNode);

            idNode++;

        }

    }

    void CreateBoxes()
    {
        int countHorizontal = boxCountHorizontal * graphSize;
        int countVertical = boxCountVertical * graphSize;

        for (int x = 1; x <= countHorizontal; x++)
        {
            for (int y = 1; y <= countVertical; y++)
            {
                Box boxNew = Instantiate(box,transform.position,Quaternion.identity);
                Vector2Int boxCoor = new Vector2Int(x, y);
                boxNew.coordinate = boxCoor;
                boxNew.transform.parent = transform;
                boxNew.name = "Box: " + x + "," + y;

                boxes.Add(boxCoor, boxNew);

            }
        }
    }




    void DrawLines()
    {
        for (int x = 1; x < edges.GetLength(0); x++)
        {
            Vector2 posFirst = graphNodes[x].transform.position;
            for (int y = x+1; y < edges.GetLength(1); y++)
            {
                if(edges[x,y] == -1) { continue; }
                Vector2 posSecond = graphNodes[y].transform.position;
                int edgeWeight = edges[x, y];

                Line newLine = Instantiate(linePrefab, transform.position, Quaternion.identity);
                newLine.transform.parent = transform;
                newLine.name = "Line: " + x + "," + y;
                newLine.ConnectLine(posFirst, posSecond);

                newLine.SetWeightText(edgeWeight);
                newLine.PlaceText(posFirst, posSecond);

            }
        }


    }

    List<Node> GetNodesFromBoxes(List<Box> boxes)
    {
        List<Node> nodes = new List<Node>();

        // TODO Check all nodes grom that box group

        return nodes;

    }


    void InitializeEdges()
    {
        int sizeMatrix = (boxCountHorizontal *boxCountVertical * graphSize) + 1;
        
        edges = new int[sizeMatrix, sizeMatrix];

        // Fill With -1
        for (int x = 0; x < edges.GetLength(0); x++)
        {
            for (int y = 0; y < edges.GetLength(1); y++)
            {
                edges[x, y] = -1;
            }
        }       

    }   


    Vector3 GetRandomPos(int xLower ,int xUpper, int yLower, int yUpper)
    {
        int xCoor = Random.Range(xLower, xUpper);
        int yCoor = Random.Range(yLower, yUpper);

        return new Vector3(xCoor, yCoor, 0f);

    }

    // DEBUG
    void PrintAllEdges()
    {
        string str = "\n";
        for (int x = 0; x < edges.GetLength(0); x++)
        {            
            for (int y = 0; y < edges.GetLength(1); y++)
            {
                str += (edges[x, y] + " ");
            }
            str += "\n";
        }
        Debug.Log(str);

    }

    //DEBUG
    void PrintEdgesofNode(int nodeId)
    {
        //int nodeId = boxes[boxCoor].GetComponentInChildren<GraphNode>().id;

        Debug.Log(nodeId + ". Node Edges:");
        for (int y = 1; y < edges.GetLength(1); y++)
        {
            if(edges[nodeId,y] != -1)
            {
                Debug.Log("Node: " + y + " , Weight: " + edges[nodeId, y]);
            }
            
        }


    }
    
}
