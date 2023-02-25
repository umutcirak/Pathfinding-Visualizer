using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphSpeedPicker : MonoBehaviour
{
    [Header("Graph Speed")]
    [SerializeField] Color defaultSizeButtonColor;
    [SerializeField] Color selectedColor;
    [SerializeField] GameObject graphSpeedList;
    [SerializeField] Button graphSpeedButton;


    public enum GraphSpeed { Fast, Slow, Turtle}
    public GraphSpeed currentSpeed;

    public float[] speeds = new float[] { 0f, 0.25f, 0.5f};


    Button[] buttonList;

    
    GraphSettings graphSettings;
    GraphVisualizer graphVisualizer;

    void Awake()
    {
        graphSettings = FindObjectOfType<GraphSettings>();
        graphVisualizer = FindObjectOfType<GraphVisualizer>();

    }

    private void Start()
    {
        currentSpeed = (GraphSpeed)(graphSettings.currentSpeed);
        GetAllSpeed();
        ChangeButtonColor((int)currentSpeed);

        SetSpeed(0);
    }


    public void ChangeGraphSpeed(int status) // 0: Fast 1: Slow 2: Turtle
    {
        currentSpeed = (GraphSpeed)status;

        switch (currentSpeed)
        {
            case GraphSpeed.Fast:
                ChangeButtonColor(0);
                break;
            case GraphSpeed.Slow:
                ChangeButtonColor(1);
                break;
            case GraphSpeed.Turtle:
                ChangeButtonColor(2);
                break;     
              
            default:
                break;
        }
        ChangeVisibility();
        SetSpeed(status);
    }

    void ChangeButtonColor(int indexButton)
    {
        foreach (var button in buttonList)
        {
            button.GetComponent<Image>().color = defaultSizeButtonColor;
        }

        buttonList[indexButton].GetComponent<Image>().color = selectedColor;
    }

    void SetSpeed(int speedIndex)
    {
        graphSettings.speedIndex = speedIndex;
        graphSettings.currentSpeed = speeds[speedIndex];
        graphVisualizer.waitForExplore = speeds[speedIndex];
    }


    public void GetAllSpeed()
    {
        buttonList = graphSpeedList.GetComponentsInChildren<Button>();
    }


    public void ChangeVisibility()
    {
        bool isActive = graphSpeedList.activeSelf;
        graphSpeedList.SetActive(!isActive);
    }



}



