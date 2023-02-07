using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Node Settings")]
    
    [SerializeField] public Tile startTile;
    [SerializeField] public Tile targetTile;

    public Tile[,] allTiles;




    void Update()
    {
       
    }






    public void Reset()
    {
        foreach (Tile item in allTiles)
        {
            item.GetComponent<Tile>().ResetTile();
        }

        startTile = null;
        targetTile = null;
    }







}
