using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] public Tile startTile;
    [SerializeField] public Tile targetTile;

    public Dictionary<Vector2Int, Tile> allTiles = new Dictionary<Vector2Int, Tile>();
    public Dictionary<Vector2Int, Node> allNodes = new Dictionary<Vector2Int, Node>();
    public Dictionary<Vector2Int, Node> walls = new Dictionary<Vector2Int, Node>();

    [SerializeField] BFS bfs;


    [HideInInspector] public TileVisualizer tileVisualizer;

    [Range(0f, 0.5f)] public float searchWait;

    private void Awake()
    {
        tileVisualizer = FindObjectOfType<TileVisualizer>();
    }



    public void Reset()
    {
        foreach (KeyValuePair<Vector2Int, Node> item in allNodes)
        {
            Vector2Int coor = item.Key;
            if (!walls.ContainsKey(coor))
            {
                item.Value.Reset();
                allTiles[coor].ResetTile();
            }                       
        }       

    }

    void ClearWalls()
    {

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
        bfs.BuildPath();        
        startTile.SetImage(startTile.startImage);
        targetTile.SetImage(targetTile.targetImage);               
       
    }
    

    public bool IsProcessing()
    {
        return tileVisualizer.tilesInProcess.Count != 0;
    }





}
