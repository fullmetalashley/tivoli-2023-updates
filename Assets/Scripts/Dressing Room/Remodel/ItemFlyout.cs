using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemFlyout : MonoBehaviour
{
    //POSITION CONTROLS
    public float travelTime;
    private float currentTime;

    private float normalizedValue;

    public Vector3 hiddenPosition;
    public Vector3 availablePosition;

    private Vector3 startPosition;
    private Vector3 endPosition;

    RectTransform theRect;

    public bool isHidden;

    //COLOR CONTROLS
    public Color transparent;
    public Color full;

    //PARENTING TEST CONTROLS
    public GameObject frontOfCloset;
    public GameObject backOfCloset;

    public bool reset;

    public FlyoutManager thisManager;

    public bool mysteryIcon;

    public bool initialized;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    //If we haven't had a chance to set this thing up, let's do that first.
    public void Init()
    {
        if (!initialized)
        {
            initialized = true;
            theRect = GetComponent<RectTransform>();
            isHidden = true;
        }
    }

    public void StartTransition()
    {
        //A quick check to make sure the transform is set.
        



        currentTime = 0;
        if (!isHidden)
        {
            //The object is out in space and needs to reverse positions.
            //It needs to move to its hidden positions.
            startPosition = availablePosition;
            endPosition = hiddenPosition;
            isHidden = true;

            this.gameObject.transform.SetParent(backOfCloset.transform);

            this.GetComponent<Image>().raycastTarget = false;

            if (reset)
            {
                
              this.gameObject.GetComponent<Button>().interactable = false;
                this.gameObject.GetComponent<Image>().color = transparent;
            }



        }
        else
        {
            if (!reset)
            {
                this.gameObject.GetComponent<Image>().color = full;
            }

            //Object is currently hidden and needs to move forward.
            if (reset)
            {
                this.gameObject.GetComponent<Button>().interactable = false;
            }

            startPosition = hiddenPosition;
            endPosition = availablePosition;
            isHidden = false;

            this.gameObject.transform.SetParent(frontOfCloset.transform);
            if (!mysteryIcon)
            {
                this.GetComponent<Image>().raycastTarget = true;
            }

        }
        StartCoroutine(LerpMovement());
    }


    IEnumerator LerpMovement()
    {
        //AS WE ARE FLYING: Turn off the raycast target here.
        while (currentTime <= travelTime)
        {
            this.GetComponent<Image>().raycastTarget = false;
            currentTime += Time.deltaTime;
            normalizedValue = currentTime / travelTime;
            theRect.anchoredPosition = Vector3.Lerp(startPosition, endPosition, normalizedValue);

            yield return null;

        }

        if (isHidden)
        {
            this.gameObject.GetComponent<Image>().color = transparent;
        }

        if (reset && !isHidden)
        {
            this.gameObject.GetComponent<Image>().color = full;
        }

        //ONCE FLYING IS DONE: Turn on the raycast target.
        if (!mysteryIcon)
        {
            this.GetComponent<Image>().raycastTarget = true;
        }

        thisManager.SignalTransitionComplete();
    }
}
