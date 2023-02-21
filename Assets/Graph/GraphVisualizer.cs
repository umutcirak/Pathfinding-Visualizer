using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphVisualizer : MonoBehaviour
{

    [Header("Start-Target Node Settings")]
    [SerializeField] Color startNodeColor;
    [SerializeField] Color targetNodeColor;


    [Header("Path Node Settings")]
    [SerializeField] [Range(1f, 4f)] float growthPathMax;
    [SerializeField] [Range(1f, 3f)] float growthPathEnd;
    [SerializeField] [Range(1.01f,1.1f)] float growthPathStep;
    [SerializeField] [Range(0f, 0.1f)] float growthPathWait;
    [SerializeField] Color colorPath;
    [SerializeField] Color colorPathLine;
    [SerializeField] public float pathWait;

    [Header("Path Line Settings")]
    [SerializeField] float thicknessPathLine;
    [SerializeField] float weightGrowAmount;
    [SerializeField] Color weightColor;


    [Header("Explore Settings")]
    [SerializeField] [Range(1f, 2.5f)] float growthExploreMax;
    [SerializeField] [Range(1f, 1.4f)] float growthExploreEnd;
    [SerializeField] [Range(1.01f, 1.1f)] float growthExploreStep;
    [SerializeField] [Range(0f, 0.1f)] float growthExploreWait;
    [SerializeField] Color colorExplored;    
    [SerializeField] public float waitForExplore;
    [SerializeField] public float exploreColorLerpTime;

    [Header("Neighbor of Explored Node Settings")]
    [SerializeField] Color colorNeighborExplored;
    [SerializeField] float neighborExploredGrowthAmount;
    [SerializeField] float neighborExploredHangWait;
    [SerializeField] float  neighborExploredStepWait;
    [SerializeField] float neighborExploredGrowthStep;

    Graph graph;
    GraphManager graphManager;
      

    [SerializeField] float processTimeLeft;
    [SerializeField] float debugWait;
    [SerializeField] bool isPathBuilding;


    List<GraphNode> neighborInProcess = new List<GraphNode>();
    List<GraphNode> exploreInProcess = new List<GraphNode>();

    private void Awake()
    {
        graphManager = FindObjectOfType<GraphManager>();
        graph = FindObjectOfType<Graph>();
    }

    private void Update()
    {
        DecreaseProcesTimeLeft();
    }

    public void SetStartColors()
    {
        ChangeColor(graphManager.startNode, startNodeColor);
        ChangeColor(graphManager.targetNode, targetNodeColor);
    }


    public IEnumerator VisualizePathCO(List<GraphNode> path)
    {
        while(processTimeLeft >= 0f)
        {
            yield return new WaitForEndOfFrame();
        }
        isPathBuilding = true;

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
        isPathBuilding = false;
        graphManager.isRunning = false;
               
    }   

    void VisualizePathLine(Line line)
    {
        line.HoverWeightText(weightGrowAmount,thicknessPathLine,weightColor);
        line.thicknessLast = line.lineRenderer.startWidth;      

    }

    public void VisualizeExploredNode(GraphNode nodeExplored)
    {
        if(nodeExplored!= null && nodeExplored.id != graphManager.targetNode.id)
        {            
            StartCoroutine(ChangeColorCO(nodeExplored, colorExplored));

            StartCoroutine(EnlargeShrinkNodeCO(nodeExplored,growthExploreMax,growthExploreEnd,
                growthExploreStep,growthExploreWait,0f));
        
        }      
    }    

    public IEnumerator ChangeNeighborColorCO(GraphNode node)
    {
        if(waitForExplore >= 0.15f)
        {
            Debug.Log("Amk processi basladi");

            Color initialColor = node.GetComponent<SpriteRenderer>().material.color;
            Color targetColor = colorNeighborExplored;

            float duration = (waitForExplore / 2) - 0.1f;
            float elapsedTime = 0.0f;

            //Color currentColor = node.GetComponent<SpriteRenderer>().color;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / duration;
                node.GetComponent<SpriteRenderer>().material.color = Color.Lerp(initialColor, targetColor, t);
                yield return null;
            }
            elapsedTime = 0.0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / duration;
                node.GetComponent<SpriteRenderer>().material.color = Color.Lerp(targetColor, initialColor, t);
                yield return null;
            }
            node.GetComponent<SpriteRenderer>().material.color = initialColor;

        }    
            

    } 

    public void VisualizeNeighborOfExploredNode(List<GraphNode> changedNeighbors, List<int> previousDistances)
    {       

        for (int i = 0; i < changedNeighbors.Count; i++)
        {
            GraphNode node = changedNeighbors[i];
            int previousDistance = previousDistances[i];

            //ChangeColor(node, colorNeighborExplored);

            StartCoroutine(EnlargeShrinkNodeCO(node, neighborExploredGrowthAmount, 1f, neighborExploredGrowthStep, 
                neighborExploredStepWait, waitForExplore));
            StartCoroutine(node.SetDistanceText(previousDistance));
        }

        
    }

    


    public IEnumerator EnlargeShrinkNodeCO(GraphNode node, float max_growth,float end_growth, float step, 
        float waitStep, float waitBeforeShrink)
    {
        if (!isPathBuilding)
        {
            processTimeLeft = 100f;
        }
        

        float max_x = node.transform.localScale.x * max_growth;
        float end_x = node.transform.localScale.x * end_growth;

        // node.transform.localScale.x < max_x
        while (Mathf.Abs(node.transform.localScale.x - max_x) > 0.1f)
        {
            yield return new WaitForSeconds(waitStep);

            node.transform.localScale *= step;

        }
        yield return new WaitForSeconds(waitBeforeShrink);
        // node.transform.localScale.x > end_x
        while (Mathf.Abs(node.transform.localScale.x - end_x) > 0.1f)
        {
            yield return new WaitForSeconds(waitStep);
            node.transform.localScale /= step;
        }

        processTimeLeft = debugWait;

        
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

        node.lastColor = sr.color;       

    }


    void DecreaseProcesTimeLeft()
    {
        if(processTimeLeft > 0f)
        {
            processTimeLeft -= Time.deltaTime;
        }
        
    }

}
