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
        

    
   


    public void OnMouseDown()
    {  
        if(gameManager.IsProcessing()) { return; }

        if(Input.GetMouseButtonDown(0))
        {
            bool isStartSelected = gameManager.startTile != null;
            bool isTargetSelected = gameManager.targetTile != null;

            if (isStartSelected && isTargetSelected)
            {
                //gameManager.startTile.ResetTile();
                //gameManager.targetTile.ResetTile();

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
        if (gameManager.IsProcessing()) { return; }

        //Debug.Log("On Wall");
        
        if (Input.GetMouseButton(1))
        {
            if(gameManager.walls.ContainsKey(coordinate)) { return; }

            Node wall = gameManager.allNodes[coordinate];
            wall.Block();
            gameManager.walls.Add(coordinate,wall);          
            GetComponent<SpriteRenderer>().color = tileVisualizer.wallColor;
        }
    }

    


    public void SetImage(Sprite image)
    {      
        GetComponent<SpriteRenderer>().sprite = image;        
    }

    public void ResetTile()
    {
        Debug.Log("Resetted");
        GetComponent<SpriteRenderer>().sprite = defaultImage;
        GetComponent<SpriteRenderer>().color = tileVisualizer.defaultColor;           
          
    }










}
