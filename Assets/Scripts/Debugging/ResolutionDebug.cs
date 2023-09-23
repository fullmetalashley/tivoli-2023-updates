using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionDebug : MonoBehaviour
{
    public Text resolution;
    // Start is called before the first frame update
    void Start()
    {
        Debug.LogError("Making the console pop up!");
        resolution.text = "Resolution: " + Screen.currentResolution;
    }

    public void RecheckText()
    {
        resolution.text = "Resolution: " + Screen.currentResolution;
    }

    void Update()
    {
        RecheckText();
    }
}
