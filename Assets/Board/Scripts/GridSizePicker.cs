using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class GridSizePicker : MonoBehaviour
{
    [Header("Grid Size Button")]
    [SerializeField] Color defaultSizeButtonColor;
    [SerializeField] Color selectedColor;
    [SerializeField] GameObject gridSizeList;
    [SerializeField] Button gridSizeButton;


    Button[] buttonList;

    public enum GridSize { Small, Medium, Large, Vast, Enormous }
    public GridSize currentSize;
       
    GridSettings gridSettings;

    void Awake()
    {       
        gridSettings = FindObjectOfType<GridSettings>();
    }

    private void Start()
    {
        currentSize = (GridSize)(gridSettings.currentSize - 1);
        GetAllSizes();
        ChangeButtonColor((int)currentSize);
    }
     
      

    public void ChangeGridSize(int status) // 0: Small 1: Medium 2: Large 3: Vast 4: Enormous
    {
        currentSize = (GridSize)status;

        switch (currentSize)
        {
            case GridSize.Small:
                ChangeButtonColor(0);
                break;
            case GridSize.Medium:
                ChangeButtonColor(1);
                break;
            case GridSize.Large:
                ChangeButtonColor(2);
                break;
            case GridSize.Vast:
                ChangeButtonColor(3);
                break;
            case GridSize.Enormous:
                ChangeButtonColor(4);
                break;
            default:
                break;
        }
        ChangeVisibility();
        SetSize(status);
        SceneManager.LoadScene(0);
    }


    void ChangeButtonColor(int indexButton)
    {
        foreach (var button in buttonList)
        {
            button.GetComponent<Image>().color = defaultSizeButtonColor;
        }

        buttonList[indexButton].GetComponent<Image>().color = selectedColor;
    }

    public void GetAllSizes()
    {
         buttonList = gridSizeList.GetComponentsInChildren<Button>();
    }

    public void ChangeVisibility()
    {
        bool isActive = gridSizeList.activeSelf;
        gridSizeList.SetActive(!isActive);
    }


    void SetSize(int buttonIndex)
    {
        gridSettings.currentSize = buttonIndex + 1;
    }




}
