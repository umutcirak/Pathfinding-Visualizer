using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GraphPauseButton : MonoBehaviour
{

    [SerializeField] Button pauseButton;
    [SerializeField] TextMeshProUGUI pauseTMP;
    public Color pauseColor;
    public Color playColor;
    bool isPause = true;


    GraphManager graphManager;

    private void Awake()
    {
        graphManager = FindObjectOfType<GraphManager>();
    }
    public void ChangePause()
    {
        if (isPause)
        {
            graphManager.isPaused = true;

            pauseButton.GetComponent<Image>().color = playColor;
            pauseTMP.text = "Play";
            isPause = false;
        }
        else
        {
            graphManager.isPaused = false;

            pauseButton.GetComponent<Image>().color = pauseColor;
            pauseTMP.text = "Pause";
            isPause = true;
        }

    }




}
