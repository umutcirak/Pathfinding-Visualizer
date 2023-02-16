using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePoint : MonoBehaviour
{
        
    public bool isStartMoving = false;
    public bool isTargetMoving = false;
    public bool isStopMoving = false;
    public bool IsPointMoving { get { return isStartMoving || isTargetMoving || isStopMoving; } }
       
    private int nullCoordinate = -99;
    public Vector2Int previousCoordinate;
    public Vector2Int currentCoordinate;

    public float selectDuration;
    public float timeElapsed = 0f;

    public bool isChanged;


    GameManager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    private void Start()
    {
        previousCoordinate = new Vector2Int(nullCoordinate, nullCoordinate);
        currentCoordinate = new Vector2Int(nullCoordinate, nullCoordinate);
    }

    void Update()
    {        
        MoveThePoint();   
    }

    void MoveThePoint()
    {
        if (Input.GetMouseButtonUp(0))
        {
            StopMovement();
        }

        if (Input.GetMouseButton(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);                      

            if (hit.collider != null)
            {
                Tile point = hit.collider.gameObject.GetComponent<Tile>();

                if (point != null)
                {
                    // || gameManager.IsStopTile(point.coordinate)
                    if (gameManager.IsWall(point.coordinate)) { return; }                    
                    if (IsChanged(point.coordinate)) { return; }

                    // Set Pointer Coordinates
                    if(previousCoordinate.x == nullCoordinate && currentCoordinate.x == nullCoordinate)
                    {
                        currentCoordinate = point.coordinate;
                    }
                    else if(currentCoordinate.x != nullCoordinate && point.coordinate != currentCoordinate)
                    {
                        previousCoordinate = currentCoordinate;
                        currentCoordinate = point.coordinate;
                    }
                    
                    // THIS ONE
                    if (!IsPointMoving)
                    {
                        // Check if there is movement - Start Tile
                        if (gameManager.IsStartTile(currentCoordinate) && !isStartMoving)
                        {
                            timeElapsed += Time.deltaTime;
                            if (timeElapsed > selectDuration)
                            {
                                isStartMoving = true;
                            }

                        }
                        // Check if there is movement - Target Tile
                        else if (gameManager.IsTargetTile(currentCoordinate) && !isTargetMoving)
                        {
                            timeElapsed += Time.deltaTime;
                            if (timeElapsed > selectDuration)
                            {
                                isTargetMoving = true;
                            }
                        }

                        // Check if there is movement - Stop Tile
                        else if (gameManager.IsStopTile(currentCoordinate) && !isStopMoving)
                        {
                            timeElapsed += Time.deltaTime;
                            if (timeElapsed > selectDuration)
                            {
                                isStopMoving = true;
                            }
                        }
                    }                  



                    // Move the Tile
                    if (previousCoordinate.x != nullCoordinate)
                    {
                        Tile previousTile = gameManager.allTiles[previousCoordinate];
                        Tile nextTile = gameManager.allTiles[currentCoordinate];
                        Vector2Int coor = nextTile.coordinate;
                        if (isStartMoving) // start
                        {
                            if(!gameManager.IsTargetTile(coor) && !gameManager.IsStopTile(coor))
                            {
                                nextTile.SetImage(nextTile.startImage);
                                gameManager.startTile = nextTile;

                                previousTile.SetImage(previousTile.defaultImage);
                            }                                                     
                                                        
                        }
                        else if(isTargetMoving) // target
                        {
                            if (!gameManager.IsStartTile(coor) && !gameManager.IsStopTile(coor))
                            {
                                nextTile.SetImage(nextTile.targetImage);
                                gameManager.targetTile = nextTile;

                                previousTile.SetImage(previousTile.defaultImage);
                            }                                                    
                        }
                        else if (isStopMoving) // stop
                        {
                            if (!gameManager.IsStartTile(coor) && !gameManager.IsTargetTile(coor))
                            {
                                nextTile.SetImage(nextTile.stopImage);
                                gameManager.stopTile = nextTile;

                                previousTile.SetImage(previousTile.defaultImage);
                            }
                           
                        }

                        if (IsPointMoving)
                        {
                            gameManager.Visualize();
                        }
                                                
                    }
                }
                
            }         

        }       

    }

    bool IsChanged(Vector2Int coor)
    {
        if(currentCoordinate.x == coor.x && currentCoordinate.y == coor.y
            && previousCoordinate.x != nullCoordinate)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    void StopMovement()
    {
        isStartMoving = false;
        isTargetMoving = false;
        isStopMoving = false;
        isChanged = false;
        timeElapsed = 0f;
        currentCoordinate = new Vector2Int(nullCoordinate, nullCoordinate);
        previousCoordinate = new Vector2Int(nullCoordinate, nullCoordinate);
    }
    


    

}
