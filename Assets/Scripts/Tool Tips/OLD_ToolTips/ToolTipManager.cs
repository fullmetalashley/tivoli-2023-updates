using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTipManager : MonoBehaviour
{
    public static ToolTipManager control;

    public bool tipOneRead;
    public bool tipTwoRead;
    public bool dressingTipRead;
    public bool jewelryTipRead;

    public List<bool> tutorialArrowStatus;

    void Awake()
    {
        if (control == null)
        {
            DontDestroyOnLoad(gameObject);
            control = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ClearAllTips()
    {
        tipOneRead = false;
        tipTwoRead = false;
        dressingTipRead = false;
        jewelryTipRead = false;
        for (int i = 0; i < tutorialArrowStatus.Count; i++)
        {
            tutorialArrowStatus[i] = true;
        }
    }

    public void AllTipsRead()
    {
        tipOneRead = true;
        tipTwoRead = true;
        dressingTipRead = true;
        jewelryTipRead = true;
        for (int i = 0; i < tutorialArrowStatus.Count; i++)
        {
            tutorialArrowStatus[i] = false;
        }
    }

    public bool CheckTipStatus(string tipName)
    {
        if (tipName == "Tip One")
        {
            return tipOneRead;
        }else if (tipName == "Tip Two")
        {
            return tipTwoRead;
        }else if (tipName == "Dressing Tip")
        {
            return dressingTipRead;
        }
        else
        {
            return jewelryTipRead;
        }
    }

    public void TipRead(string tipName)
    {
        if (tipName == "Tip One")
        {
            tipOneRead = true;
        }
        else if (tipName == "Tip Two")
        {
            tipTwoRead = true;
        }
        else if (tipName == "Dressing Tip")
        {
            dressingTipRead = true;
        }
        else
        {
            jewelryTipRead = true;
        }
    }
}
