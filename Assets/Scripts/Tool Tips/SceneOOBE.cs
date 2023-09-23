using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//SCRIPT PURPOSE: Attached to the specific scene elements that control the OOBE in here.
public class SceneOOBE : MonoBehaviour
{
    //UI Elements
    public GameObject sceneQuote;   //The scene quote game object
    public GameObject tutorialBox;
    public Text tutorialText;

    //Tutorial box strings and refs to process them.
    public List<string> tutorialStrings;
    public int tutorialIndex;

    public string sceneCode;    //The label for this scene (the scene name)
    public string discoveryCode;

    //List of tool tips in the scene
    public List<ActionTip> toolTips;
    public int tipIndex;
    public int tipsProcessed;

    //SCRIPT REFS
    public OOBEManager oOBEManager;
    public ActionToolTipDelay tipTimer;



    // Start is called before the first frame update
    void Start()
    {
        sceneCode = SceneManager.GetActiveScene().name;
        oOBEManager = FindObjectOfType<OOBEManager>();
        tipTimer = GetComponent<ActionToolTipDelay>();

        //First, if we have accessed that, all is well and this will default to on.  If not, we turn it off.
        sceneQuote.SetActive(oOBEManager.ReturnEncounterStatus(sceneCode));

        tutorialText.text = tutorialStrings[tutorialIndex]; //Set the text to be the blank text.

        oOBEManager.discoveryCodes[discoveryCode] = true;
        if (oOBEManager.tipCodes.ContainsKey(discoveryCode))
        {
            oOBEManager.tipCodes[discoveryCode] = true;
        }

        //Process our list of tool tips against the OOBE manager's list.
        ProcessTipsInDictionary();

        //If this is our first time in this room, let's report that to the OOBE so it can track.

        //IF we have accessed the scene quote and the tutorial box, we can start with a tool tip.  Otherwise, only the tutorial box will activate the next tip.
        if (!tutorialBox.activeSelf && !sceneQuote.activeSelf)
        {
            //If we still have more tips to process in here, turn on the next one.
            InitiateActionTip();
        }
    }

    //Disable the scene quote.  Called by the button on the scene quote.
    public void TurnOffSceneQuote()
    {
        sceneQuote.SetActive(false);
        oOBEManager.FirstTimeEncounter(sceneCode);

        //NEXT, we turn on the tutorialBox.
        tutorialBox.SetActive(true);
    }

    //Move to the next string in the list.
    public void NextTutorialString()
    {
        tutorialIndex++;
        if (tutorialIndex < tutorialStrings.Count)
        {
            tutorialText.text = tutorialStrings[tutorialIndex];
        }
        else
        {
            //Otherwise, we are done with this and we can shut down the box.
            tutorialBox.SetActive(false);
            InitiateActionTip();
        }
    }

    //Turn the box off without processing the tutorial.
    public void SkipTutorialBox()
    {
        tutorialBox.SetActive(false);
        InitiateActionTip();
    }

    //Check the bool values of this list of tool tips against the dictionary.
    public void ProcessTipsInDictionary()
    {
        //Process our list of tool tips against the OOBE manager's list.
        for (int i = 0; i < toolTips.Count; i++)
        {
            string condition = toolTips[i].condition;

            toolTips[i].accessed = oOBEManager.tipCodes[condition];
        }
    }

    //Turns on a tool tip based on what index we're at in the list.
    //Used for natural progression through the list, not the conditionals.
    public void InitiateActionTip()
    {
        //First: is a tip already running?
        bool canRun = true;
        for (int j = 0; j < toolTips.Count; j++)
        {
            if (toolTips[j].gameObject.activeSelf)
            {
                canRun = false;
                break;
            }
        }

        //If no tips are running...
        if (canRun)
        {
            //A tip is already running.  Wait until it is cancelled.
            for (int i = 0; i < toolTips.Count; i++)
            {
                //Have we accessed this tip?
                if (!toolTips[i].accessed)  //If no, check if it is an action tip.
                {
                    if (!toolTips[i].actionTip) //If it is not an action tip, process it.
                    {
                        //We have already processed what tips have been discovered based on functionality, we should not need to do it again.
                        toolTips[i].gameObject.SetActive(true);
                        toolTips[i].accessed = true;

                        //MANUALLY Set the functionality to also accessed now.
                        oOBEManager.tipCodes[toolTips[i].condition] = true;
                        tipIndex = i;
                        tipsProcessed++;
                        break;

                    }
                }
            }
        }
            
    }

    //Turns on a tool tip based on a conditional specifically.
    public void InitiateActionTip(string condition)
    {
        for (int i =0; i < toolTips.Count; i++)
        {
            if (toolTips[i].condition == condition)
            {
                if (!toolTips[i].accessed)
                {
                    toolTips[i].gameObject.SetActive(true);
                    toolTips[i].accessed = true;

                    //MANUALLY Set the functionality to also accessed now.
                    oOBEManager.tipCodes[toolTips[i].condition] = true;
                    tipIndex = i;
                    tipsProcessed++;
                    break;
                }
            }
        }
    }

    public void CloseActionTip()
    {
        toolTips[tipIndex].gameObject.SetActive(false);

        //We also need to tell the overall OOBE manager that we've hit this tip now.
        oOBEManager.MiniTipRead(sceneCode);

        //Now: let's start the timer for the next tip, as long as there are more tips to process.
        if (tipsProcessed < toolTips.Count)
        {
            tipTimer.timerOn = true;
        }
    }

    //An action has been done to click something, and this Scene OOBE has been notified.  Let's check and see if the tip needs to activate.
    public void FunctionClick(string code)
    {
        //If we haven't discovered this function yet...
        if (!oOBEManager.discoveryCodes[code])
        {
            //First, we need to close the current action tip.
            CloseActionTip();
            tipTimer.timerOn = false;
            InitiateActionTip(code);
        }
    }

    //If a button has been clicked, we can report this as accessed now.
    public void ReportClick(string code)
    {
        oOBEManager.discoveryCodes[code] = true;
        //We also need to adjust that tool tip value in the dictionary.
        oOBEManager.MiniTipRead(code);

        //And we need to set that tool tip in the scene.
        for (int i =0; i < toolTips.Count; i++)
        {
            if (toolTips[i].condition == code)
            {
                toolTips[i].accessed = true;
            }
        }
    }

    //Called from the settings menu.
    public void ResetTutorial()
    {
        FindObjectOfType<OOBEManager>().ResetOOBE();
    }

    //Need a little more clarity on what this is going to do, but for now it is disabling the Tool Tips.  Turning it back on will reset them all.
    public void ToggleToolTips()
    {
        FindObjectOfType<OOBEManager>().DisableOOBE();
    }
}
