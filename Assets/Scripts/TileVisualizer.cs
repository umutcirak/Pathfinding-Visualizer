using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileVisualizer : MonoBehaviour
{

    [SerializeField] [Range(0f, 2.5f)] float lerpTime;
    [SerializeField] ParticleSystem exploreVFX;


    [Header("Color Settings")]
    [SerializeField] public Color defaultColor;
    [SerializeField] public Color pathColor;
    [SerializeField] Color[] visitedColors;
    [SerializeField] Color wallColor;


    public List<Tile> tilesInProcess = new List<Tile>();


    public void VisualizeExploration(Tile tile)
    {
        ParticleSystem vfx = Instantiate(exploreVFX, tile.transform.position, Quaternion.identity);
        Destroy(vfx, vfx.main.duration);
        vfx.transform.parent = tile.transform;
        ChangeColor(tile, visitedColors);        
    }


    public void VisualizePath(Tile tile)
    {
        ChangeColor(tile, pathColor);
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
        while (currentTime <= lerpTime)
        {
            currentTime += Time.deltaTime;
            float t = currentTime / lerpTime;
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
            yield return new WaitForSeconds(lerpTime);
        }       

    }



}
