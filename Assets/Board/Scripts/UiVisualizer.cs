using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class UiVisualizer : MonoBehaviour
{
    public Color defaultButtonColor;
    [Header("Stop Button")]
    [SerializeField] Button stopButton;
    [SerializeField] TextMeshProUGUI stopTMP;
    public Color stopRemoveColor;
    public Color stopPlaceColor;
   
    private string stopAdd = "Add Stop";
    private string stopRemove = "Remove Stop";

    [Header("Pause Button")]
    [SerializeField] Button pauseButton;
    [SerializeField] TextMeshProUGUI pauseTMP;
    public Color pauseColor;
    public Color playColor;
    bool isPause = true;
     


    GameManager gameManager;
    MovePoint pointMover;
               

    public enum Speed { } // TO DO


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


    public void ChangePause()
    {
        if(isPause)
        {
            gameManager.isPaused = true;           

            pauseButton.GetComponent<Image>().color = playColor;
            pauseTMP.text = "Play";
            isPause = false;
        }
        else
        {
            gameManager.isPaused = false;
            
            pauseButton.GetComponent<Image>().color = pauseColor;
            pauseTMP.text = "Pause";
            isPause = true;
        }

    }

    public void OpenGraph()
    {
        SceneManager.LoadScene(1);
    }


}
