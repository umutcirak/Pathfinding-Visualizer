using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GraphNode : MonoBehaviour
{     
    public int id;    
    public Color lastColor;
    public bool isPath;

    //Djikstra
    public int shortestDistance = int.MaxValue;
    public int previous; // Previous Node ID       

    private Vector3 startSize;
    

    HoverVisualizer hoverVisualizer;
    GraphVisualizer graphVisualizer;
    GraphManager graphManager;

    private void Awake()
    {
        hoverVisualizer = FindObjectOfType<HoverVisualizer>();
        graphVisualizer = FindObjectOfType<GraphVisualizer>();
        graphManager = FindObjectOfType<GraphManager>();
    }

    private void Start()
    {
        shortestDistance = int.MaxValue;
        lastColor = GetComponent<SpriteRenderer>().color;
        startSize = transform.localScale;
    }


    void OnMouseEnter()
    {
        if (graphManager.isRunning) { return; }
        lastColor = GetComponent<SpriteRenderer>().color;
        hoverVisualizer.Hover(this);               
    }

    void OnMouseExit()
    {
        if (graphManager.isRunning) { return; }
        hoverVisualizer.HoverExit(this);
    }


    void OnMouseOver()
    {
        if(graphManager.isRunning) { return; }

        if (Input.GetMouseButtonDown(0))
        {
            if (graphManager.startNode == null)
            {
                graphManager.startNode = this;
                graphVisualizer.VisualizeSelectStartNode();
                graphManager.startNode.isPath = true;
            }
            else if (graphManager.startNode != null && graphManager.targetNode == null)
            {
                graphManager.targetNode = this;
                graphVisualizer.VisualizeSelectTargetNode();
                graphManager.targetNode.isPath = true;
            }
        }
    }


    public void Reset()
    {               
        shortestDistance = int.MaxValue;
        previous = -1;
        isPath = false;

        transform.localScale = startSize;
        GetComponent<SpriteRenderer>().color = Color.white;
        lastColor = Color.white;

        gameObject.layer = LayerMask.NameToLayer("Bottom");
    }
      



}
