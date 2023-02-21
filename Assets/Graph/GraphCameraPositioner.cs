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
    private float start_pos_x;
    private float start_pos_y;


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
            RestrictCamera();
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
        RestrictCamera();
    }


    void RestrictCamera()
    {
        Vector3 currentPos = mainCamera.transform.position;

        float ratio = mainCamera.orthographicSize / sizeBorder;

        float x_change = start_pos_x - (start_pos_x * ratio) + 1f; //1f debug gap

        float x_lower = start_pos_x - x_change;
        float x_upper = start_pos_x + x_change;
               

        float new_x = mainCamera.transform.position.x;
        new_x = Mathf.Clamp(new_x, x_lower, x_upper);

        Vector3 newPos = new Vector3(new_x, currentPos.y, currentPos.z);

        mainCamera.transform.position = newPos;
    }

    void StretchCamera()
    {
        float size = graph.GraphSize * cameraSize;
        sizeBorder = size;
        float posX = (graph.UpperHorizontalPos - 1) * 0.5f;
        start_pos_x = posX;
        float posY = (graph.UpperVerticalPos - 1) * 0.5f;
        start_pos_y = posY;

        float shiftBottom = graph.GraphSize * shiftVertical;

        Vector3 pos = new Vector3(posX, posY - shiftBottom, -1f);

        mainCamera.transform.position = pos;

        mainCamera.orthographicSize = size;

    }



}
