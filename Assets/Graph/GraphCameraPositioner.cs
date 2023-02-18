using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphCameraPositioner : MonoBehaviour
{
    [Header("Camera Movement")]
    [SerializeField] float movementSpeed;
    [SerializeField] float zoomAmount;

    [Header("Initial Setup")]
    [SerializeField] float shiftVertical;
    [SerializeField] float cameraSize;

    private float sizeBorder;


    Camera mainCamera;
    Graph graph;

    private Vector3 mouseDownPosition;
    


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
        Zoom();
        MoveCameraPosition();
    }


    void MoveCameraPosition()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseDownPosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 difference = Input.mousePosition - mouseDownPosition;
            mainCamera.transform.position += new Vector3(-difference.x, -difference.y, 0f) * movementSpeed * Time.deltaTime;
            mouseDownPosition = Input.mousePosition;
        }
    }



    void Zoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0)
        {
            Camera.main.orthographicSize -= zoomAmount * Time.deltaTime;
        }
        else if (scroll < 0)
        {
            Camera.main.orthographicSize += zoomAmount * Time.deltaTime;
        }

        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 10f, sizeBorder);
    }


    void StretchCamera()
    {
        float size = graph.GraphSize * cameraSize;
        sizeBorder = size;
        float posX = (graph.UpperHorizontalPos - 1) * 0.5f;
        float posY = (graph.UpperVerticalPos - 1) * 0.5f;

        float shiftBottom = graph.GraphSize * shiftVertical;

        Vector3 pos = new Vector3(posX, posY - shiftBottom, -1f);

        mainCamera.transform.position = pos;

        mainCamera.orthographicSize = size;

    }



}
