using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TableauDatabase : MonoBehaviour
{

    public static TableauDatabase control;

    public List<ActiveTableauList> activeTableaus;
    public List<InactiveTableauList> inactiveTableaus;


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

    public void UpdateLists(DateTime currentTime)
    {
        MailController theMail = FindObjectOfType<MailController>();

        List<InactiveTableauList> toBeRemoved = new List<InactiveTableauList>();
        foreach (InactiveTableauList tableau in inactiveTableaus)
        {
            if ((tableau.dateActive - currentTime).TotalDays < 0 || (tableau.dateActive - currentTime).TotalDays == 0)
            {
                activeTableaus.Add(new ActiveTableauList(tableau.tableau, tableau.signifier));
                toBeRemoved.Add(tableau);
            }
        }
        foreach (InactiveTableauList inTableau in toBeRemoved)
        {
            
            inactiveTableaus.Remove(inTableau);
        }
    }
}
