using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileVisualizer : MonoBehaviour
{    
    [SerializeField] [Range(0f, 2.5f)] float colorLerpTime;
    [SerializeField] [Range(0f, 0.25f)] float pathStep;
    [SerializeField] ParticleSystem exploreVFX;
    [SerializeField] GameObject circlePrefab;


    [Header("Color Settings")]
    [SerializeField] public Color defaultColor;
    [SerializeField] public Color pathColor;
    [SerializeField] public Color[] visitedColors;
    [SerializeField] public Color[] secondVisitedColors;
    [SerializeField] public Color wallColor;


    public List<Tile> tilesInProcess = new List<Tile>();
    public List<Tile> wallsInProcess = new List<Tile>();

    private float debugWait = 0.1f;

    GameManager gameManager;    

    public void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();        
    }

    public void VisualizeExploration(Tile tile)
    {
        ParticleSystem vfx = Instantiate(exploreVFX, tile.transform.position, Quaternion.identity);
        vfx.transform.parent = tile.transform;
        Destroy(vfx.gameObject, vfx.duration);

        if(gameManager.doubleSearch && gameManager.secondSearchStarted)
        {
            ChangeColor(tile, secondVisitedColors);
        }
        else
        {
            ChangeColor(tile, visitedColors);
        }               
    }

    public IEnumerator VisualizeWallCo(Tile tile)
    {
        wallsInProcess.Add(tile);

        GameObject circleFX = Instantiate(circlePrefab, tile.transform.position, Quaternion.identity);
        circleFX.transform.parent = tile.transform;
                
        Vector3 initialScale = circleFX.transform.localScale;       
        Vector3 finalScale = initialScale * 5f;
        float elapsedTime = 0f;
        float duration = 1f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            circleFX.transform.localScale = Vector3.Lerp(initialScale, finalScale, t);
            yield return null;
        }

        Destroy(circleFX);
        tile.GetComponent<SpriteRenderer>().color = wallColor;
        wallsInProcess.Remove(tile);
    }


    public IEnumerator VisualizePathCo(List<Node> path)
    {
        yield return new WaitForSeconds(colorLerpTime + debugWait);
        List<Tile> pathTiles = new List<Tile>();

        for (int i = 0; i < path.Count; i++)
        {
            pathTiles.Add(gameManager.allTiles[path[i].coordinate]);
        }
        
        for (int i = 0; i < pathTiles.Count; i++)
        {
            Tile tile = pathTiles[i];
            ChangeColor(tile, pathColor);
            yield return new WaitForSeconds(pathStep);
        }
        gameManager.isPathDone = true;
    }

    private void ChangeColor(Tile tile, Color targetColor)
    {
        StartCoroutine(ColorChangeCO(tile, targetColor));        
    }

    private void ChangeColor(Tile tile, Color[] targetColors)
    {
        StartCoroutine(ColorChangeCO(tile, targetColors));
    }

    private IEnumerator ColorChangeCO(Tile tile, Color targetColor)
    {
        tilesInProcess.Add(tile);

        SpriteRenderer sr = tile.GetComponent<SpriteRenderer>();
        Color startColor = sr.color;
        float currentTime = 0.0f;
        while (currentTime <= colorLerpTime)
        {
            currentTime += Time.deltaTime;
            float t = currentTime / colorLerpTime;
            sr.color = Color.Lerp(startColor, targetColor, t);
            yield return null;
        }

        tilesInProcess.Remove(tile);
    }

    private IEnumerator ColorChangeCO(Tile tile, Color[] targetColors)
    {
        for (int i = 0; i < targetColors.Length; i++)
        {
            StartCoroutine(ColorChangeCO(tile, targetColors[i]));
            yield return new WaitForSeconds(colorLerpTime);
        }       
    }


    public void ChangeColorRuntime(Tile tile, Color color)
    {
        SpriteRenderer sr = tile.GetComponent<SpriteRenderer>();
        sr.color = color;
    }


    public void VisualizePathRuntime(List<Node> path)
    {
        List<Tile> pathTiles = new List<Tile>();

        for (int i = 0; i < path.Count; i++)
        {
            pathTiles.Add(gameManager.allTiles[path[i].coordinate]);
        }

        for (int i = 0; i < pathTiles.Count; i++)
        {
            Tile tile = pathTiles[i];
            ChangeColorRuntime(tile, pathColor);
        }
        gameManager.isPathDone = true;
    }



}
