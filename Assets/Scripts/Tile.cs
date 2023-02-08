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
