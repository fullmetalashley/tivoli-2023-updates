using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PostcardControls : MonoBehaviour
{
    public List<WordArtPanel> targetPanels;
    public GameObject postcardModal;

    public List<Button> removalButtons;
    public List<GameObject> highlights;
    private DataManager thePlayer;

    public Text dateSaved;

    public Image mainPost;

    public GameObject removeButton;
    public Image dragBlocker;

    public bool removing;

    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<DataManager>();

        //If signingPostcards is true, we have just come from the dressing room to sign, so set the image as the 
        //last saved tableau and turn this bool off.

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //This turns the modal on and off.
    public void TogglePostcards()
    {
        postcardModal.SetActive(!postcardModal.activeSelf);
    }

    //This works with the Word Art being dragged.
    //It returns the index of the panel that the mouse is currently on.
    //If the mouse is not on a panel, it will return -1.
    public int SetPanelImage()
    {
        for (int i = 0; i < targetPanels.Count; i++)
        {
            if (targetPanels[i].mouseOnSlot)
            {
                return i;
            }
        }
        return -1;
    }

    //This turns off any panel without art on it.
    public void PanelsOff()
    {
        Color transparent = new Color(1, 1, 1, 0);
        for (int i = 0; i < targetPanels.Count; i++)
        {
            if (!targetPanels[i].hasArt)
            {
                targetPanels[i].panelArt.color = transparent;
            }
        }
    }

    //This is the first display that sets the main postcard image as the tableau based on the passed index.
    public void InitialDisplay(int index)
    {
        mainPost.sprite = thePlayer.savedTableaus[index];
        dateSaved.text = thePlayer.tableauDates[index].Month + "/" + thePlayer.tableauDates[index].Day + "/" + thePlayer.tableauDates[index].Year;
        TogglePostcards();
    }

    //Sets a panel's color as full if the panel has artwork.
    public void PanelsOn()
    {
        Color on = new Color(1, 1, 1, 1);
        for (int i = 0; i < targetPanels.Count; i++)
        {
            if (!targetPanels[i].hasArt)
            {
                targetPanels[i].panelArt.color = on;
            }
        }
    }

    public void ToggleRemoval()
    {
        dragBlocker.enabled = !dragBlocker.enabled;

            //Go through each panel, and if the panel has artwork, turn the highlight on and turn the removal button on.
            for (int i = 0; i < removalButtons.Count; i++)
            {
                if (targetPanels[i].hasArt)
                {
                    removalButtons[i].enabled = !removalButtons[i].enabled;
                    highlights[i].SetActive(!highlights[i].activeSelf);
                }
            }

        

    }

    //Turns this particular panel off.
    public void ClearPanel(int index)
    {
        Color transparent = new Color(1, 1, 1, 0);

        targetPanels[index].hasArt = false;
        targetPanels[index].panelArt.sprite = null;
        targetPanels[index].panelArt.color = transparent;
        highlights[index].SetActive(false);
        removalButtons[index].enabled = false;
    }
}
