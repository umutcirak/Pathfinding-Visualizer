using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GraphSizePicker : MonoBehaviour
{

    [Header("Graph Size Button")]
    [SerializeField] Color defaultSizeButtonColor;
    [SerializeField] Color selectedColor;
    [SerializeField] GameObject graphSizeList;
    [SerializeField] Button graphSizeButton;


    Button[] buttonList;

    public enum GraphSize { Small, Large}
    public GraphSize currentSize;



    GraphSettings graphSettings;


    void Awake()
    {
        graphSettings = FindObjectOfType<GraphSettings>();
    }

    private void Start()
    {
        currentSize = (GraphSize)(graphSettings.currentSize - 1);
        GetAllSizes();
        ChangeButtonColor((int)currentSize);
    }



    public void ChangeGraphSize(int status) // 0: Small 1: Large
    {
        currentSize = (GraphSize)status;

        switch (currentSize)
        {
            case GraphSize.Small:
                ChangeButtonColor(0);
                break;
            case GraphSize.Large:
                ChangeButtonColor(1);
                break;            
            default:
                break;
        }
        ChangeVisibility();
        SetSize(status);
        SceneManager.LoadScene(1);
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
        buttonList = graphSizeList.GetComponentsInChildren<Button>();
    }


    public void ChangeVisibility()
    {
        bool isActive = graphSizeList.activeSelf;
        graphSizeList.SetActive(!isActive);
    }


    void SetSize(int buttonIndex)
    {
        graphSettings.currentSize = buttonIndex + 1;
    }


}
