using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JewelryMovement : MonoBehaviour
{
    public float travelTime;
    private float currentTime;

    private float normalizedValue;

    public Vector3 hiddenPosition;
    public Vector3 availablePosition;

    private Vector3 startPosition;
    private Vector3 endPosition;

    RectTransform theRect;

    private bool isHidden;
    // Start is called before the first frame update
    void Start()
    {
        theRect = GetComponent<RectTransform>();
        isHidden = true;
    }

    public void StartTransition()
    {
        currentTime = 0;
        if (!isHidden)
        {
            //The object is out in space and needs to reverse positions.
            startPosition = availablePosition;
            endPosition = hiddenPosition;
            isHidden = true;
        }
        else
        {
            startPosition = hiddenPosition;
            endPosition = availablePosition;
            isHidden = false;
        }
        StartCoroutine(LerpMovement());
    }

    IEnumerator LerpMovement()
    {
        while (currentTime <= travelTime)
        {
            currentTime += Time.deltaTime;
            normalizedValue = currentTime / travelTime;
            theRect.anchoredPosition = Vector3.Lerp(startPosition, endPosition, normalizedValue);

            yield return null;
        }
    }
}
