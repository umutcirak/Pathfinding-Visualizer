using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlgorithmPicker : MonoBehaviour
{
    public enum AlgorithmType { None, A_Star, BFS, DFS}
    AlgorithmType selectedAlgorithm = AlgorithmType.None;


    public void SelectAlgorithm(int type)
    {
        this.selectedAlgorithm = (AlgorithmType)type;
    }
    

    /*
     public void SelectAlgorithm(string algorithm)
    {
        switch (algorithm)
        {
            case "A*":
                selectedAlgorithm = AlgorithmType.A_Star;
                break;
            case "BFS":
                selectedAlgorithm = AlgorithmType.A_Star;
                break;
            case "DFS":
                selectedAlgorithm = AlgorithmType.DFS;
                break;
            default:
                selectedAlgorithm = AlgorithmType.None;
                break;
        }
    }
     */






}
