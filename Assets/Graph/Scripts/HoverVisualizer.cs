using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverVisualizer : MonoBehaviour
{
    [Header("Hover Settings")]
    [SerializeField] Color nodeHoverColor;
    [SerializeField] Color nodeNeighborColor;  
    [SerializeField] Color lineHoverColor;
    [SerializeField] Color weightHoverColor;
    [SerializeField] Color defaultColor;

    [SerializeField] float weightGrowMultipler;
    [SerializeField] float nodeGrowthMultipler;
    [SerializeField] float thicknessIncrease;
    [SerializeField] float transparencyAmount;


    [Header("Run Settings")]
    public bool isRunning = false;

    // Hover
    HashSet<GraphNode> highlitedNodes = new HashSet<GraphNode>();
    HashSet<Line> highlitedLines = new HashSet<Line>();


    // Transparency
    HashSet<GraphNode> transparantNodes = new HashSet<GraphNode>();    


    Graph graph;
    GraphManager graphManager;
    NeighborGroup neighborGroup;

    private void Awake()
    {
        graph = FindObjectOfType<Graph>();
        neighborGroup = FindObjectOfType<NeighborGroup>();
        graphManager = FindObjectOfType<GraphManager>();
    }




    public void Hover(GraphNode node)
    {        
        // NODE
        highlitedNodes.Add(node);
        List<GraphNode> neighborNodes = graph.GetNeighborNodes(node.id);
        foreach (GraphNode neighbor in neighborNodes)
        {
            highlitedNodes.Add(neighbor);
        }

        foreach (GraphNode nodeHighlight in highlitedNodes)
        {
            nodeHighlight.gameObject.layer = LayerMask.NameToLayer("Top");
            if (!nodeHighlight.isPath)
            {
                ChangeColor(nodeHighlight, nodeNeighborColor);
            }         
            nodeHighlight.transform.localScale *= nodeGrowthMultipler;
        }

        if (!node.isPath)
        {
            ChangeColor(node, nodeHoverColor);
        }
        
        MakeTransparentAround(node);

        // Lines
        List<Line> neighborLines = graph.GetNeighborLines(node.id);

        foreach (Line neighborLine in neighborLines)
        {
            highlitedLines.Add(neighborLine);
        }

        foreach (Line lineHighlight in highlitedLines)
        {
            lineHighlight.gameObject.layer = LayerMask.NameToLayer("Top");
            lineHighlight.weightText.gameObject.layer = LayerMask.NameToLayer("Top");
            lineHighlight.HoverWeightText(weightGrowMultipler, thicknessIncrease, weightHoverColor);
        }

        

    }

    public void HoverExit(GraphNode node)
    {
        RevokeTransparency();

        foreach (GraphNode nodeHighlight in highlitedNodes)
        {
            nodeHighlight.gameObject.layer = LayerMask.NameToLayer("Bottom");
            nodeHighlight.transform.localScale *= (1/nodeGrowthMultipler);
            ChangeColor(nodeHighlight, nodeHighlight.lastColor);
        }
        //ChangeColor(node, node.lastColor);

        foreach (Line lineHighlight in highlitedLines)
        {
            lineHighlight.gameObject.layer = LayerMask.NameToLayer("Bottom");
            lineHighlight.weightText.gameObject.layer = LayerMask.NameToLayer("Bottom");
            lineHighlight.RevokeWeightText();
        }

        highlitedNodes.Clear();
        highlitedLines.Clear();
        
        transparantNodes.Clear();
                
    }

    void MakeTransparentAround(GraphNode node)
    {
        // 3 level of nodes tranparant       

        Vector2Int boxCoor = node.GetComponentInParent<Box>().coordinate;

        HashSet<Vector2Int> boxesPossible = new HashSet<Vector2Int>();

        List<Vector2Int> boxesAround = new List<Vector2Int>();

        foreach (Vector2Int jump in neighborGroup.firstLevel)
        {
            boxesPossible.Add(boxCoor + jump);
        }
        foreach (Vector2Int jump in neighborGroup.secondLevel)
        {
            boxesPossible.Add(boxCoor + jump);
        }
        foreach (Vector2Int jump in neighborGroup.thirdLevel)
        {
            boxesPossible.Add(boxCoor + jump);
        }

        foreach (Vector2Int jumpedBox in boxesPossible)
        {
            if (graph.boxes.ContainsKey(jumpedBox))
            {
                boxesAround.Add(jumpedBox);
            }
        }

        foreach (Vector2Int boxIndex in boxesAround)
        {
            transparantNodes.Add(graph.boxes[boxIndex].GetComponentInChildren<GraphNode>());
        }


        foreach (GraphNode nodeTransparant in transparantNodes)
        {
            if (highlitedNodes.Contains(nodeTransparant)) { continue; }
            Material material = nodeTransparant.GetComponent<SpriteRenderer>().material;
            Color color = material.color;
            color.a = transparencyAmount;
            material.color = color;
        }
        



    }

    void RevokeTransparency()
    {
        foreach (GraphNode nodeTransparant in transparantNodes)
        {
            Material material = nodeTransparant.GetComponent<SpriteRenderer>().material;
            Color color = material.color;
            color.a = 1f;
            material.color = color;
        }

        transparantNodes.Clear();
    }



    private void ChangeColor(GraphNode node, Color targetColor)
    {
        node.GetComponent<SpriteRenderer>().color = targetColor;
    }

    

}
