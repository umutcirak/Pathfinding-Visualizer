using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphVisualizer : MonoBehaviour
{

    [Header("Start-Target Node Settings")]
    [SerializeField] Color startNodeColor;
    [SerializeField] Color targetNodeColor;


    [Header("Path Settings")]
    [SerializeField] [Range(1f, 4f)] float growthPathMax;
    [SerializeField] [Range(1f, 3f)] float growthPathEnd;
    [SerializeField] [Range(1.01f,1.1f)] float growthPathStep;
    [SerializeField] [Range(0f, 0.1f)] float growthPathWait;
    [SerializeField] Color colorPath;
    [SerializeField] Color colorPathLine;
    [SerializeField] public float pathWait;
    [SerializeField] float thicknessPathLine;

    [Header("Explore Settings")]
    [SerializeField] [Range(1f, 2.5f)] float growthExploreMax;
    [SerializeField] [Range(1f, 1.4f)] float growthExploreEnd;
    [SerializeField] [Range(1.01f, 1.1f)] float growthExploreStep;
    [SerializeField] [Range(0f, 0.1f)] float growthExploreWait;
    [SerializeField] Color colorExplored;
    [SerializeField] public float waitForExplore;
    [SerializeField] public float exploreColorLerpTime;



    Graph graph;
    GraphManager graphManager;


    private void Awake()
    {
        graphManager = FindObjectOfType<GraphManager>();
        graph = FindObjectOfType<Graph>();
    }
  
   
    public void SetStartColors()
    {
        ChangeColor(graphManager.startNode, startNodeColor);
        ChangeColor(graphManager.targetNode, targetNodeColor);
    }


    public IEnumerator VisualizePathCO(List<GraphNode> path)
    {
        for (int i = 0; i < path.Count; i++)
        {
            yield return new WaitForSeconds(pathWait);

            GraphNode current = path[i];
            current.gameObject.layer = LayerMask.NameToLayer("Top");


            // Color            

            if (path[i] != graphManager.targetNode && path[i] != graphManager.startNode)
            {
                ChangeColor(path[i], colorPath);
            }

            // Line
            if (i + 1 != path.Count)
            {
                int index_first = path[i].id;
                int index_second = path[i + 1].id;
                Line lineBetween = graph.GetLine(index_first, index_second);
                VisualizePathLine(lineBetween);
            }

            // GROW
            float max_x = current.transform.localScale.x * growthPathMax;
            float end_x = current.transform.localScale.x * growthPathEnd;

            while (current.transform.localScale.x < max_x)
            {
                yield return new WaitForSeconds(growthPathWait);

                current.transform.localScale *= growthPathStep;

            }

            while (current.transform.localScale.x > end_x)
            {
                yield return new WaitForSeconds(growthPathWait);
                current.transform.localScale /= growthPathStep;
            }

        }
        graphManager.isRunning = false;
               
    }   

    void VisualizePathLine(Line line)
    {
        //line.GetComponent<LineRenderer>().SetColors(colorPathLine, colorPathLine);

    }

    public void VisualizeExploredNode(GraphNode nodeExplored)
    {
        if(nodeExplored!= null && nodeExplored.id != graphManager.targetNode.id)
        {
            //nodeExplored.GetComponent<SpriteRenderer>().color = colorExplored;
            StartCoroutine(ChangeColorCO(nodeExplored, colorExplored));

            StartCoroutine(EnlargeExploredNodeCO(nodeExplored));
        
        }      
    }    

    public IEnumerator EnlargeExploredNodeCO(GraphNode nodeExplored)
    {
        float max_x = nodeExplored.transform.localScale.x * growthExploreMax;
        float end_x = nodeExplored.transform.localScale.x * growthExploreEnd;

        while (nodeExplored.transform.localScale.x < max_x)
        {
            yield return new WaitForSeconds(growthExploreWait);

            nodeExplored.transform.localScale *= growthExploreStep;

        }

        while (nodeExplored.transform.localScale.x > end_x)
        {
            yield return new WaitForSeconds(growthExploreWait);
            nodeExplored.transform.localScale /= growthExploreStep;
        }
    }

    public void VisualizeExploreLine()
    {

    }

    public void VisualizeSelectStartNode()
    {
        graphManager.startNode.GetComponent<SpriteRenderer>().color = startNodeColor;
        graphManager.startNode.lastColor = startNodeColor;
    }

    public void VisualizeSelectTargetNode()
    {
        graphManager.targetNode.GetComponent<SpriteRenderer>().color = targetNodeColor;
        graphManager.targetNode.lastColor = targetNodeColor;
    }


    public void ChangeColor(GraphNode node,Color targetColor)
    {
        node.GetComponent<SpriteRenderer>().color = targetColor;
        node.lastColor = targetColor;
    }

    IEnumerator ChangeColorCO(GraphNode node, Color targetColor)
    {
        SpriteRenderer sr = node.GetComponent<SpriteRenderer>();
        Color startColor = sr.color;
        float currentTime = 0.0f;
        while (currentTime <= exploreColorLerpTime)
        {
            currentTime += Time.deltaTime;
            float t = currentTime / exploreColorLerpTime;
            sr.color = Color.Lerp(startColor, targetColor, t);
            yield return null;
        }


    }



}
