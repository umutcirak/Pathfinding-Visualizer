using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class CameraStretch : MonoBehaviour
{
    [SerializeField] float shiftVertical;
    [SerializeField] float cameraSize;

    Camera mainCamera;
    Grid grid;


    private void Awake()
    {
        mainCamera = Camera.main;
        grid = FindObjectOfType<Grid>();
    }

    void Start()
    {
        StretchCamera();
    }

    private void Update()
    {
        StretchCamera();
    }

    void StretchCamera()
    {        
        float size= grid.gridSize * cameraSize;
        float posX = (grid.Width - 1) * 0.5f;
        float posY = (grid.Heigth - 1) * 0.5f;

        float shiftBottom = grid.gridSize * shiftVertical;

        Vector3 pos = new Vector3(posX, posY - shiftBottom, -1f);

        mainCamera.transform.position = pos;

        mainCamera.orthographicSize = size;

    }






}
