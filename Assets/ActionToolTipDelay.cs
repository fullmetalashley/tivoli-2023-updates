using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script purpose: If the player has encountered a tool tip, run a delay so that the player can encounter the next one afterwards.
public class ActionToolTipDelay : MonoBehaviour
{
    //SCRIPT REFS
    public SceneOOBE oobeControls;

    //Bool to determine if timer should run
    public bool timerOn;

    //Timer values
    public float currentTimer;
    public float baseTimer;

    // Start is called before the first frame update
    void Start()
    {
        oobeControls = GetComponent<SceneOOBE>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timerOn)
        {
            currentTimer -= Time.deltaTime;

            if (currentTimer <= 0)
            {
                //We can start the next timer.
                oobeControls.InitiateActionTip();
                currentTimer = baseTimer;
                timerOn = false;
            }
        }
    }
}
