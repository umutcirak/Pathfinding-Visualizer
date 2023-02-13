using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UiVisualizer : MonoBehaviour
{
    public Color defaultButtonColor;
    [Header("Stop Button")]   
    public Color stopRemoveColor;
    public Color stopPlaceColor;
    [SerializeField] TextMeshProUGUI stopTMP;
    private string stopAdd = "Add Stop";
    private string stopRemove = "Remove Stop";



    [SerializeField] Button stopButton;


    GameManager gameManager;
    MovePoint pointMover;


    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        pointMover = FindObjectOfType<MovePoint>();
    }


    
    public void ChangeStopButton(int status) // 0: default, 1: Place , 2: Remove
    {
        switch (status)
        {
            case 0:
                stopButton.GetComponent<Image>().color = defaultButtonColor;
                stopTMP.text = stopAdd;
                break;
            case 1:
                stopButton.GetComponent<Image>().color = stopPlaceColor;                
                break;
            case 2:
                stopButton.GetComponent<Image>().color = stopRemoveColor;
                stopTMP.text = stopRemove;
                break;
            default:
                break;
        }
       
        
    }


}
