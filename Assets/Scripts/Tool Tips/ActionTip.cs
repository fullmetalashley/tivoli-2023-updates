using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script purpose: Contain the references for the action tool tip so it can be reskinned appropriately.
public class ActionTip : MonoBehaviour
{
    //UI Elements
    public Text contentBody;
    public GameObject arrowIndicator;

    //Actual content refs.
    public string content;

    //Checks to see what condition must be met / whether or not this tip has been accessed.
    public string condition;

    public bool accessed;    //Have we discovered this tip yet?  Defaults to false.

    public bool actionTip;  //Whether or not this is an action tip

    void Start()
    {
        contentBody.text = content;
    }

    private void Update()
    {
        //If this tool tip is on, and we click the mouse, turn it off.
        if (this.gameObject.activeSelf)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
            {
                FindObjectOfType<SceneOOBE>().CloseActionTip(); //Close this tip, then establish the next tip.
            }
        }
    }
}
