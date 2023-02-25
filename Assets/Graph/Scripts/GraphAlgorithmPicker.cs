using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GraphAlgorithmPicker : MonoBehaviour
{
    [SerializeField] GameObject listAlgorithms;
    [SerializeField] TextMeshProUGUI visualizeText;

    public enum AlgorithmType { None, Djikstra }
    public AlgorithmType selectedAlgorithm = AlgorithmType.None;


    private void Start()
    {
        SetVisualizeText();
    }

    public void SelectAlgorithm(int type)
    {
        this.selectedAlgorithm = (AlgorithmType)type;

        DeActiveList();
        SetVisualizeText();
    }

    public void DeActiveList()
    {
        listAlgorithms.SetActive(false);
    }

    public void ChangeVisibility()
    {
        bool isActive = listAlgorithms.activeSelf;
        listAlgorithms.SetActive(!isActive);
    }


    private void SetVisualizeText()
    {
        switch (selectedAlgorithm)
        {
            case AlgorithmType.None:
                visualizeText.text = "Visualize";
                break;
            case AlgorithmType.Djikstra:
                visualizeText.text = "Run Djikstra";
                break;            
            default:
                break;
        }
    }




}
