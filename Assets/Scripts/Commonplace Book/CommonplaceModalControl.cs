using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script purpose: Control the individual commonplace modals and toggle them on / off.
public class CommonplaceModalControl : MonoBehaviour
{
    //A list of the modal game objects to toggle between.
    public List<GameObject> modals;

    //The two credits modals.
    public GameObject credits1;
    public GameObject credits2;

    //The text header for the credits.
    public Text creditsHeader;

    //Change the modal based on what button is clicked.
    public void ChangeModal(int index)
    {
        for (int i =0; i < modals.Count; i++)
        {
            if (i != index)
            {
                modals[i].SetActive(false);
            }
            else
            {
                modals[i].SetActive(true);
            }
        }
    }

    //Toggle between the two pages of the credits.
    public void ChangeCreditsPage()
    {
        //Toggle between the two pages.
        credits1.SetActive(!credits1.activeSelf);
        credits2.SetActive(!credits2.activeSelf);

        //Change the header.
        if (credits1.activeSelf)
        {
            creditsHeader.text = "Credits";
        }
        else
        {
            creditsHeader.text = "Acknowledgements";
        }
    }
}
