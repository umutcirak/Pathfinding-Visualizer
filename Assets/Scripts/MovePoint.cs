using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePoint : MonoBehaviour
{
    
     public bool isStartMoving = false;
     public bool isTargetMoving = false;
     public bool IsPointMoving { get { return isStartMoving || isTargetMoving; } }


    GameManager gameManager;

    public Vector2Int previousCoordinate;
    public Vector2Int currentCoordinate;



    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
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
                    // Set Pointer Coordinates
                    if(previousCoordinate == null && currentCoordinate == null)
                    {
                        currentCoordinate = point.coordinate;
                    }
                    else if(currentCoordinate != null && point.coordinate != currentCoordinate)
                    {
                        previousCoordinate = currentCoordinate;
                        currentCoordinate = point.coordinate;
                    }
                   
                    // Check if there is movement 
                    if (gameManager.startTile != null &&
                        currentCoordinate == gameManager.startTile.coordinate && !isStartMoving)
                    {
                        isStartMoving = true;
                    }

                    if (gameManager.targetTile != null &&
                        currentCoordinate == gameManager.targetTile.coordinate && !isTargetMoving)
                    {
                        isTargetMoving = true;
                    }



                }
            }         

        }       

    }


    void StopMovement()
    {
        isStartMoving = false;
        isTargetMoving = false;
    }
    


    

}
