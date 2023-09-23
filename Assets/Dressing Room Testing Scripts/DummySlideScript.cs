using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummySlideScript : MonoBehaviour
{
    public GameObject stage;

    public Transform leftStage;
    public Transform centerStage;

    public GameObject hats;
    public GameObject dresses;

    public float timeOfTravel = 5; //time after object reach a target place 
    public float currentTime = 0; // actual floting time
    public float normalizedValue = 0;

    public float currentTimeInvis = 0;
    public float normalizedValueInvis = 0;

    public bool closetVisible;

    public GameObject returnSlide;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    public void ToggleScreens(string category)
    {
        switch (category){
            case "Hats":
                hats.SetActive(true);
                dresses.SetActive(false);
                break;
            case "Dresses":
                dresses.SetActive(true);
                hats.SetActive(false);
                break;
            default:
                break;
        }

        currentTime = 0;
        normalizedValue = 0;

        currentTimeInvis = 0;
        normalizedValueInvis = 0;


        if (closetVisible)
        {
            //This means we need to move to left stage, and we are at center stage.
            StartCoroutine(LerpToVisible(stage, centerStage, leftStage));

        }
        else
        {
            returnSlide.SetActive(false);
            StartCoroutine(LerpToVisible(stage, leftStage, centerStage));

        }
        closetVisible = !closetVisible;

    }



    IEnumerator LerpToVisible(GameObject moving, Transform hidden, Transform visible)
    {

        while (currentTime <= timeOfTravel)
        {
            currentTime += Time.deltaTime;
            normalizedValue = currentTime / timeOfTravel; // we normalize our time 

            moving.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(hidden.GetComponent<RectTransform>().anchoredPosition, visible.GetComponent<RectTransform>().anchoredPosition, normalizedValue);
            yield return null;
        }
        returnSlide.SetActive(!closetVisible);
    }
}
