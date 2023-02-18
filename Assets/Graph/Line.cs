using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Line : MonoBehaviour
{
    public int attachedWeight;    

    [Header("Weight Line - Text Settings")]
    [SerializeField] public TextMeshPro weightText;
    [SerializeField] int fontSize;
    [SerializeField] float padding;

    
    [SerializeField] float thicknessStart;
    [SerializeField] float thicknessEnd;


    Vector2 weightTextDefaultPos;

    LineRenderer lineRenderer;

    private void Awake()
    {       
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        lineRenderer.positionCount = 2;
        weightText.fontSize = fontSize;

        lineRenderer.startWidth = thicknessStart;
        lineRenderer.endWidth = thicknessEnd;

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

        Vector2 direction = posStart - posEnd;
        Vector2 newPosition = posStart - direction.normalized * padding;

        weightTextDefaultPos = newPosition;

        weightText.gameObject.transform.position = newPosition;
    }

    public void HoverWeightText(float weightGrowAmount, float thicknessIncrease, Color weightColor)
    {
        Vector3 posStart = lineRenderer.GetPosition(0);
        Vector3 posEnd = lineRenderer.GetPosition(1);

        Vector2 middlePosition = (posStart + posEnd) / 2;
        weightText.transform.position = middlePosition;

        weightText.fontStyle = FontStyles.Bold;
        weightText.fontSize = fontSize * weightGrowAmount;

        lineRenderer.startWidth *= thicknessIncrease;
        lineRenderer.endWidth *= thicknessIncrease;

        weightText.color = weightColor;
    }



    public void RevokeWeightText()
    {
        weightText.transform.position = weightTextDefaultPos;
        weightText.fontStyle = FontStyles.Normal;

        weightText.fontSize = fontSize;

        lineRenderer.startWidth = thicknessStart;
        lineRenderer.endWidth = thicknessEnd;

        weightText.color = Color.white;

    }


}
