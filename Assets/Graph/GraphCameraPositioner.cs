using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphCameraPositioner : MonoBehaviour
{

    [SerializeField] float shiftVertical;
    [SerializeField] float cameraSize;

    Camera mainCamera;
    Graph graph;


    private void Awake()
    {
        mainCamera = Camera.main;
        graph = FindObjectOfType<Graph>();
    }

    void Start()
    {
        StretchCamera();
    }

    private void Update()
    {
        //StretchCamera();
    }

    void StretchCamera()
    {
        float size = graph.GraphSize * cameraSize;
        float posX = (graph.UpperHorizontalPos - 1) * 0.5f;
        float posY = (graph.UpperVerticalPos - 1) * 0.5f;

        float shiftBottom = graph.GraphSize * shiftVertical;

        Vector3 pos = new Vector3(posX, posY - shiftBottom, -1f);

        mainCamera.transform.position = pos;

        mainCamera.orthographicSize = size;

    }



}
