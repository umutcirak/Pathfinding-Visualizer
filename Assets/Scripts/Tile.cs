using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [Header("Sprite Settings")]
    [SerializeField] public Sprite defaultImage;
    [SerializeField] public Sprite startImage;
    [SerializeField] public Sprite targetImage;
    [SerializeField] public Sprite stopImage;
       

    public Vector2Int coordinate;

    GameManager gameManager;
    TileVisualizer tileVisualizer;
    UiVisualizer uiVisualizer;

   
    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        tileVisualizer = FindObjectOfType<TileVisualizer>();
        uiVisualizer = FindObjectOfType<UiVisualizer>();
    }    


    public void OnMouseDown()
    {  
        if(gameManager.IsProcessingAlgorithm()) { return; }
        if(gameManager.IsWall(coordinate)) { return;  }      

        // Set Start - Target - Stop
        if (Input.GetMouseButtonDown(0))
        {           
            bool isStartSelected = gameManager.startTile != null;
            bool isTargetSelected = gameManager.targetTile != null;
            bool isStopSelected = gameManager.stopTile != null;

            if (!gameManager.isStopPlacing)
            {
                if(gameManager.stopTile != null)
                {
                    if (gameManager.IsStopTile(coordinate)) { return; }
                }

                if (isStartSelected && isTargetSelected)
                {
                    if (gameManager.IsStartTile(coordinate) || gameManager.IsTargetTile(coordinate)) { return; }

                    gameManager.Reset();
                    gameManager.startTile = null;
                    gameManager.targetTile = null;
                }
                else
                {
                    if (!isStartSelected && !isTargetSelected)
                    {
                        SetImage(startImage);
                        gameManager.startTile = this;
                    }
                    else if (isStartSelected && !isTargetSelected)
                    {
                        if (gameManager.IsStartTile(coordinate)) { return; }

                        SetImage(targetImage);
                        gameManager.targetTile = this;
                    }

                }
            }
            else
            {
                SetImage(stopImage);
                gameManager.stopTile = this;
                gameManager.isStopPlacing = false;
                uiVisualizer.ChangeStopButton(2);
                
            }
           
                        
        }      

    }

    // Create Wall
    private void OnMouseEnter()
    {                    
        if (Input.GetMouseButton(1))
        {
            if (gameManager.IsProcessingAlgorithm()) { return; }

            if(gameManager.IsStartTile(coordinate) || gameManager.IsTargetTile(coordinate) || 
                gameManager.IsStopTile(coordinate))
            { return; }           

            if (gameManager.walls.ContainsKey(coordinate)) { return; }

            Node wall = gameManager.allNodes[coordinate];
            wall.Block();
            gameManager.walls.Add(coordinate,wall);

            StartCoroutine(tileVisualizer.VisualizeWallCo(this));
        }
    }


   



    public void SetImage(Sprite image)
    {      
        GetComponent<SpriteRenderer>().sprite = image;        
    }

    public void ResetTile()
    {
        GetComponent<SpriteRenderer>().sprite = defaultImage;
        GetComponent<SpriteRenderer>().color = tileVisualizer.defaultColor;           
          
    }

   









}
