using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [Header("Sprite Settings")]
    [SerializeField] public Sprite defaultImage;
    [SerializeField] public Sprite startImage;
    [SerializeField] public Sprite targetImage;
       

    public Vector2Int coordinate;

    GameManager gameManager;
    TileVisualizer tileVisualizer;
   
    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        tileVisualizer = FindObjectOfType<TileVisualizer>();
    }
        

    
   


    // Set Start - Target
    public void OnMouseDown()
    {  
        if(gameManager.IsProcessingAlgorithm()) { return; }
        if(gameManager.IsWall(coordinate)) { return;  }

        if(Input.GetMouseButtonDown(0))
        {
            bool isStartSelected = gameManager.startTile != null;
            bool isTargetSelected = gameManager.targetTile != null;

            if (isStartSelected && isTargetSelected)
            {               
                gameManager.Reset();

                gameManager.startTile = null;
                gameManager.targetTile = null;
            }
            else 
            {
                if(!isStartSelected && !isTargetSelected)
                {
                    SetImage(startImage);
                    gameManager.startTile = this;
                }
                else if(isStartSelected && !isTargetSelected)
                {
                    SetImage(targetImage);
                    gameManager.targetTile = this;
                }
                
            }           
        }           
    }

    // Create Wall
    private void OnMouseEnter()
    {     
               
        if (Input.GetMouseButton(1))
        {
            if (gameManager.IsProcessingAlgorithm()) { return; }
            if (gameManager.IsMainTile(coordinate)) { return;  }
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
