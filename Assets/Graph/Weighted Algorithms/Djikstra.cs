using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Djikstra : MonoBehaviour
{
    Graph graph;



    private void Awake()
    {
        graph = FindObjectOfType<Graph>();
    }


}
