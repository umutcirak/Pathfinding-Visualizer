using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] public Tile startTile;
    [SerializeField] public Tile targetTile;

    public Dictionary<Vector2Int, Tile> allTiles = new Dictionary<Vector2Int, Tile>();
    public Dictionary<Vector2Int, Node> allNodes = new Dictionary<Vector2Int, Node>();


    [SerializeField] BFS bfs;


    [HideInInspector] public TileVisualizer tileVisualizer;

    [Range(0f, 0.5f)] public float searchWait;

    private void Awake()
    {
        tileVisualizer = FindObjectOfType<TileVisualizer>();
    }



    public void Reset()
    {
        foreach (KeyValuePair<Vector2Int, Tile> item in allTiles)
        {
            item.Value.ResetTile();
        }

        foreach (KeyValuePair<Vector2Int,Node> item in allNodes)
        {
            item.Value.Reset();
        }        
    }


    //DEBUG
    public void PrintStartNode()
    {
        Node startNode = allNodes[startTile.coordinate];
        Node targetNode = allNodes[targetTile.coordinate];

        Debug.Log(startNode.coordinate);
        Debug.Log(targetNode.coordinate);

    }

    public void Visualize()
    {       
        List<Node> path = bfs.GetPath();

        startTile.SetImage(startTile.startImage);
        targetTile.SetImage(targetTile.targetImage);

        foreach (Node node in path)
        {
            Debug.Log(node.coordinate);
            Tile pathTile = allTiles[node.coordinate];
            //pathTile.GetComponent<SpriteRenderer>().color = pathTile.pathColor;

            tileVisualizer.VisualizeExploration(pathTile);
        }       


    }
    

    public bool IsProcessing()
    {
        return tileVisualizer.tilesInProcess.Count != 0;
    }





}
