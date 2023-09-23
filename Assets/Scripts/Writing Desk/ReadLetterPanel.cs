using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script purpose: Sets up a letter panel that will have connections to the letter values in the UI.
public class ReadLetterPanel : MonoBehaviour
{
    //Used for the character letters.
    public Text sender;
    public Text dateSent;

    //Used for the regency letters.
    public Text header;

    //Used for the final letter in the list.
    public Text body;

    public int letterIndex; //The corresponding index of the letter value.
}
