using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] Sprite defaultImage;
    [SerializeField] Sprite startImage;
    [SerializeField] Sprite targetImage;

    bool isVisited;
    bool isSelected;
    public enum TileType { Path, Start, Target, Wall}
    TileType tileType = TileType.Path;

    GameManager gameManager;
    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    
   


    public void OnMouseDown()
    {  
        if(Input.GetMouseButtonDown(0))
        {
            bool isStartSelected = gameManager.startTile != null;
            bool isTargetSelected = gameManager.targetTile != null;

            if (isStartSelected && isTargetSelected) { gameManager.Reset(); }


            if (tileType == TileType.Path)
            {
                if(!isStartSelected && !isTargetSelected)
                {
                    SetType(TileType.Start, startImage);
                    gameManager.startTile = this;
                }
                else if(isStartSelected && !isTargetSelected)
                {
                    SetType(TileType.Target, targetImage);
                    gameManager.targetTile = this;
                }

                
            }
           
        }
               

    }


    public void SetType(TileType type, Sprite image)
    {
        this.tileType = type;
        GetComponent<SpriteRenderer>().sprite = image;        
    }

    public void ResetTile()
    {
        GetComponent<SpriteRenderer>().sprite = defaultImage;
        isVisited = false;
        isSelected = false;
        tileType = TileType.Path;
    }










}
