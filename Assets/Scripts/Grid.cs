using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private int widthMultiplier;
    [SerializeField] private int heightMultiplier;
    [SerializeField] [Range(1,5)] public int gridSize;
    [SerializeField] private GameObject tilePrefab;

    [HideInInspector] public int Width { get { return widthMultiplier * gridSize; } }
    [HideInInspector] public int Heigth { get { return heightMultiplier * gridSize; } }



    public enum BoardTransition { processing, notProcessing };
    public BoardTransition currentTransition = BoardTransition.notProcessing;

    GameManager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void Start()
    {
        CreateGrid();
    }



    void CreateGrid()
    {        
        //gameManager.allTiles = new Tile[Width, Heigth];

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Heigth; y++)
            {
                Vector2 pos = new Vector2(x, y);
                GameObject tile = Instantiate(tilePrefab, pos, Quaternion.identity);
                tile.transform.parent = transform;
                tile.name = "Tile-" + x + "," + y;

                Tile newTile = tile.GetComponent<Tile>();               
                Vector2Int posInt = new Vector2Int((int)pos.x, (int)pos.y);
                newTile.coordinate = posInt;
                gameManager.allTiles.Add(posInt, newTile);

                Node node = new Node();
                node.coordinate = posInt;
                gameManager.allNodes.Add(posInt, node);

            }
        }
    }

   

}
