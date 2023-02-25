using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GraphDensityAdjuster : MonoBehaviour
{

    
    [SerializeField] Button graphDensityButton;
    [SerializeField] GameObject densityList;
    [SerializeField] Button applyButton;
    [SerializeField] Slider sliderFirst;
    [SerializeField] Slider sliderSecond;
    [SerializeField] Slider sliderThird;

    [Header("Area Multiplier")]
    [SerializeField] float firstMultiplier = 50f;
    [SerializeField] float secondMultiplier = 50f;
    [SerializeField] float thirdMultiplier = 50f;




    GraphSettings graphSettings;
    private void Awake()
    {
        graphSettings = FindObjectOfType<GraphSettings>();
    }



    private void Start()
    {
        sliderFirst.value = graphSettings.firstCircleDensity / 50f;
        sliderSecond.value = graphSettings.secondCircleDensity / 50f;
        sliderThird.value = graphSettings.thirdCircleDensity / 50f;
    }   


    public void ApplySettings()
    {
        Debug.Log("First: " + sliderFirst.value);
        Debug.Log("Second: " + sliderSecond.value);
        Debug.Log("Thirds: " + sliderThird.value);


        graphSettings.firstCircleDensity = (int) (sliderFirst.value * firstMultiplier);
        graphSettings.secondCircleDensity = (int)( sliderSecond.value * secondMultiplier);
        graphSettings.thirdCircleDensity = (int) (sliderThird.value * thirdMultiplier);

        SceneManager.LoadScene(1);
    }

    public void ChangeVisibility()
    {
        bool isActive = densityList.activeSelf;
        densityList.SetActive(!isActive);
    }

}
