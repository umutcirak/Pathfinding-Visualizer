using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphSettings : MonoBehaviour
{
    public int currentSize;

    public int speedIndex;
    public float currentSpeed;


    public int firstCircleDensity;
    public int secondCircleDensity;
    public int thirdCircleDensity;

    void Awake()
    {
        ManageSingleton();
    }


    void Start()
    {
        currentSize = 1;

        firstCircleDensity = 30;
        secondCircleDensity = 15;
        thirdCircleDensity = 15;

    }

    void ManageSingleton()
    {
        int instanceCount = FindObjectsOfType(GetType()).Length;

        if (instanceCount > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);

        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }


}
