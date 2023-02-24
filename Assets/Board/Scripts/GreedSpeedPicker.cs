using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GreedSpeedPicker : MonoBehaviour
{
    [Header("Grid Speed Button")]
    [SerializeField] Color defaultSizeButtonColor;
    [SerializeField] Color selectedColor;
    [SerializeField] GameObject gridSpeedList;
    [SerializeField] Button gridSpeedButton;

    public enum GridSpeed { Fast, Decent, Slow, Tortoise }
    public GridSpeed currentSpeed;

    public float[] speeds = new float[] { 0f, 0.25f, 0.5f, 1f }; 


    Button[] buttonList;


    GameManager gameManager;
    GridSettings gridSettings;

    void Awake()
    {
        gridSettings = FindObjectOfType<GridSettings>();
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        currentSpeed = (GridSpeed)(gridSettings.currentSpeed);
        GetAllSpeed();
        ChangeButtonColor((int)currentSpeed);

        SetSpeed(0);
    }


    public void ChangeGridSpeed(int status) // 0: Fast 1: Decent 2: Slow 3: Tortoise
    {
        currentSpeed = (GridSpeed)status;

        switch (currentSpeed)
        {
            case GridSpeed.Fast:
                ChangeButtonColor(0);
                break;
            case GridSpeed.Decent:
                ChangeButtonColor(1);
                break;
            case GridSpeed.Slow:
                ChangeButtonColor(2);
                break;
            case GridSpeed.Tortoise:
                ChangeButtonColor(3);
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

    public void GetAllSpeed()
    {
        buttonList = gridSpeedList.GetComponentsInChildren<Button>();
    }


    public void ChangeVisibility()
    {
        bool isActive = gridSpeedList.activeSelf;
        gridSpeedList.SetActive(!isActive);
    }


    void SetSpeed(int speedIndex)
    {
        gridSettings.speedIndex = speedIndex;
        gridSettings.currentSpeed = speeds[speedIndex];
        gameManager.searchWait = speeds[speedIndex];
    }

}
