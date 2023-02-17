using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Line : MonoBehaviour
{
    int attachedWeight;
    Vector2Int connection = new Vector2Int(-1,-1);   // nodeStart id, nodeEnd id

    [Header("Weight Text Settings")]
    [SerializeField] TextMeshPro weightText;
    [SerializeField] int textSize;
    [SerializeField] float padding;


    LineRenderer lineRenderer;

    private void Awake()
    {
        //weightText = GetComponentInChildren<TextMeshPro>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        lineRenderer.positionCount = 2;
        weightText.fontSize = textSize;
        
    }

    public void ConnectLine(Vector2 posFirst, Vector2 posSecond)
    {
        lineRenderer.SetPosition(0, posFirst);
        lineRenderer.SetPosition(1, posSecond);
    }

    public void SetWeightText(int weight)
    {
        attachedWeight = weight;
        weightText.gameObject.SetActive(true);
        weightText.text = attachedWeight.ToString();
    }

    public void PlaceText(Vector2 posStart , Vector2 posEnd)
    {       
        //weightText.gameObject.transform.position = posStart;

        Vector2 direction = posStart - posEnd;
        Vector3 newPosition = posStart - direction.normalized * padding;

        weightText.gameObject.transform.position = newPosition;
    }


}
