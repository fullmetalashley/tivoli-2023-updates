using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TableauController : MonoBehaviour
{
    //These objects will turn on and off whenever the menu toggle method is called via button.
    public GameObject dressingRoom;
    public GameObject tableauMenu;

    //This image is the main tableau, and will be set when a tableau is selected.
    public Image mainTableau;
    public GameObject tableauObject;

    //This holds all of the potential tableau images.
    public List<Sprite> tableauDatabase;

    //This holds the corresponding highlights for the tableau images.
    public List<Image> highlights;

    public int selectedTableau;

    public Transform doll1Start;
    public Transform doll2Start;

    public GameObject dressingArea;
    public GameObject background;

    public Transform[] doll1Movement;
    public Transform[] doll2Movement;

    public GameObject Jane;
    public GameObject Elizabeth;
    public GameObject dollSwap;

    public PreviewTableau tableauPrev;

    public DollManager dollControls;
    

    //A tableau is selected, but not confirmed yet.  It's selected for the preview.
    public void SelectTableau(int index)
    {
        for (int i = 0; i < highlights.Count; i++)
        {
            if (i != index)
            {
                //Highlights are turned off if they're not at the right button.
                highlights[i].enabled = false;
            }
            else
            {
                highlights[i].enabled = true;
                selectedTableau = index;
            }
        }
        tableauPrev.tableauPreview.sprite = tableauDatabase[selectedTableau];

    }


}
