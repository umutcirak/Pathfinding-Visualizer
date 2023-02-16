using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] public Tile startTile;
    [SerializeField] public Tile targetTile;
    [SerializeField] public Tile stopTile;

    public Dictionary<Vector2Int, Tile> allTiles = new Dictionary<Vector2Int, Tile>();
    public Dictionary<Vector2Int, Node> allNodes = new Dictionary<Vector2Int, Node>();
    public Dictionary<Vector2Int, Node> walls = new Dictionary<Vector2Int, Node>();

    [SerializeField] BFS bfs;


    [HideInInspector] public TileVisualizer tileVisualizer;
    [HideInInspector] public UiVisualizer uiVisualizer;

    [Range(0f, 0.5f)] public float searchWait;

    public bool isPathDone = false;

    public bool isStopPlacing;    
    

    private void Awake()
    {
        tileVisualizer = FindObjectOfType<TileVisualizer>();
        uiVisualizer = FindObjectOfType<UiVisualizer>();
    }
          
   
    public void Reset()
    {        
        foreach (KeyValuePair<Vector2Int, Node> item in allNodes)
        {
            Vector2Int coor = item.Key;
            if (!walls.ContainsKey(coor) && !IsStopTile(coor))
            {                               
                item.Value.Reset();
                allTiles[coor].ResetTile();
            }                       
        }
        //tileVisualizer.ChangeColorRuntime(stopTile, tileVisualizer.defaultColor);
        isPathDone = false;

    }    


    public void ResetNodes()
    {
        foreach (KeyValuePair<Vector2Int,Node> item in allNodes)
        {            
            if(!IsWall(item.Value.coordinate))
            {
                item.Value.Reset();
            }
        }
    }
     
    public void ClearWalls()
    {
        if(IsProcessingWall() || IsProcessingAlgorithm() ) { return; }
        

        foreach (KeyValuePair<Vector2Int, Node> item in walls)
        {
            Vector2Int coor = item.Key;
            item.Value.Reset();
            allTiles[coor].ResetTile();
        }      

        walls.Clear();
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
        if( !CanStart()) { return; }

        bfs.BuildPath();        
        startTile.SetImage(startTile.startImage);
        targetTile.SetImage(targetTile.targetImage);       
       
    }
    

    public bool IsProcessingAlgorithm()
    {
        return tileVisualizer.tilesInProcess.Count != 0;
    }
    public bool IsProcessingWall()
    {
        return tileVisualizer.wallsInProcess.Count != 0;
    }

    bool CanStart()
    {
        return startTile != null && targetTile != null;
    }


    public bool IsWall(Vector2Int coor)
    {
        return walls.ContainsKey(coor);
    }

    public bool IsStartTile(Vector2Int coor)
    {   
        if(startTile == null)
        {
            return false;
        }
        else
        {
            return startTile.coordinate == coor;
        }
         
    }

    public bool IsTargetTile(Vector2Int coor)
    {
        if (targetTile == null)
        {
            return false;
        }
        else
        {
            return targetTile.coordinate == coor;
        }
    }

    public bool IsStopTile(Vector2Int coor)
    {
        if (stopTile == null)
        {
            return false;
        }
        else
        {
            return stopTile.coordinate == coor;
        }
    }

    public void ReloadScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }


    public void ChangeStop()
    {
        bool isPlaced = stopTile != null;
        
        if (!isStopPlacing && !isPlaced)
        {
            isStopPlacing = true;
            uiVisualizer.ChangeStopButton(1);
        }        
        else if (!isStopPlacing && isPlaced)
        {
            RemoveStop();
            uiVisualizer.ChangeStopButton(0);
        }
        


    }     

    public void RemoveStop()
    {
        stopTile.SetImage(stopTile.defaultImage);
        stopTile = null;
    }

}
