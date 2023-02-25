using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{   



    public void RestartGraph()
    {
        SceneManager.LoadScene(1);
    }
    public void OpenGrid()
    {
        SceneManager.LoadScene(0);
    }

}
