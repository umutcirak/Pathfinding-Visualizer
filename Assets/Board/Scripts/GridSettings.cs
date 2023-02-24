using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSettings : MonoBehaviour
{
    public int currentSize;
    
    public int speedIndex;
    public float currentSpeed;

     void Awake()
    {
        ManageSingleton();
    }


    void Start()
    {
        currentSize = 2;

        speedIndex = 0;
        currentSpeed = 0;
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
