using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TickerText : MonoBehaviour
{
    //The UI element for the text
    public Text theText;
    public string theNarrative;

    //How quickly the text shows up
    public float speed;

    //The close button.
    public GameObject closeButton;

    // Use this for initialization
    void Start()
    {
        theNarrative = theText.text;    //Set the text string to be our text value.
        StartCoroutine(ShowText());
    }

    IEnumerator ShowText()
    {
        for (int i = 0; i < theNarrative.Length + 1; i++)
        {
            theText.text = theNarrative.Substring(0, i);
            yield return new WaitForSeconds(speed);
        }
        closeButton.SetActive(true);
    }
}
