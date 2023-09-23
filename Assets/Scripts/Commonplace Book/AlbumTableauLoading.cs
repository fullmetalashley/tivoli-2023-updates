using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script purpose: Populate the tableaus into the album.
public class AlbumTableauLoading : MonoBehaviour
{
    //The tableau images
    public List<Image> tableauTargets;

    //SCRIPT REFS
    private DataManager playerData;

    //refs to the preview modal objects
    public GameObject previewModal;
    public Image previewScreen;
    public GameObject deleteModal;

    //The current photo that has been selected
    public int currentPhoto;

    // Start is called before the first frame update
    void Start()
    {
        playerData = FindObjectOfType<DataManager>();

        //Loop through the player's saved tableaus.
        PopulateTableaus();
    }

    public void PopulateTableaus()
    {
        for (int i = 0; i < playerData.savedTableaus.Count; i++)
        {
            //Make sure we're in the range of the available tableaus.
            if (i < tableauTargets.Count)
            {
                tableauTargets[i].sprite = playerData.savedTableaus[i];
                tableauTargets[i].GetComponent<Button>().interactable = true;
            }
        }
        for (int i = playerData.savedTableaus.Count; i< tableauTargets.Count; i++)
        {
            tableauTargets[i].sprite = null;
            tableauTargets[i].GetComponent<Button>().interactable = false;
        }
    }

    //Toggle the preview on and off.
    public void TogglePreview()
    {
        previewModal.SetActive(!previewModal.activeSelf);

        if (!previewModal.activeSelf)
        {
            currentPhoto = -1;
        }
    }

    //Set the main preview to match the clicked tableau.
    public void SetImage(int index)
    {
        previewScreen.sprite = tableauTargets[index].sprite;
        TogglePreview();

        currentPhoto = index;
    }

    //Removes a tableau from the player's data and shifts the other tableaus to accomodate.
    public void DeleteImage()
    {
        //Take the tableau out of the list.
        if (currentPhoto != -1) //A caution check to make sure a tableau is selected; this shouldn't actually happen, but it's a catch.
        {
            playerData.savedTableaus.Remove(playerData.savedTableaus[currentPhoto]);
            PopulateTableaus();

            currentPhoto = -1;

            TogglePreview();
            ToggleDeleteModal();
        }
    }

    //Turn the delete modal on and off as necessary.
    public void ToggleDeleteModal()
    {
        deleteModal.SetActive(!deleteModal.activeSelf);
    }
}
