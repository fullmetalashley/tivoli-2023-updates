using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script purpose: Controls the eReader within the scene itself.
public class PnPController : MonoBehaviour
{
    private PnPDatabase pnpNovel;
    private DataManager playerData;
    private eReaderController eReader;

    [Header("Static block settings")]
    //Settings for the eReader screen.
    public Text staticHeader;
    public Text staticSummary;
    public Text staticBody;

    [Header("Scroll block settings")]
    public Text scrollHeader;
    public Text scrollSummary;
    public Text scrollBody;
    public GameObject summaryLeft;
    public GameObject summaryRight;

    [Header("Content buffers / empty space")]
    [TextArea(15, 20)]
    public string topBuffer;
    [TextArea(15, 20)]
    public string endBuffer;

    //Private reference so we can track the section we need to update to.
    private int section;

    // Start is called before the first frame update
    void Start()
    {
        pnpNovel = FindObjectOfType<PnPDatabase>();
        playerData = FindObjectOfType<DataManager>();
        eReader = GetComponent<eReaderController>();

        DisplayChapter(false);

        if (playerData.novelogueTransfer)
        {
            NovelogueOverride();
        }
    }

    //We have been sent here by a novelogue notification.
    public void NovelogueOverride()
    {
        DisplayChapter(true);
        playerData.novelogueTransfer = false;
    }

    //Set the content to list all of the info from the current chapter.
    public void DisplayChapter(bool openPage)
    {
        string sectionContents = "";
        string summaryText = pnpNovel.chapters[playerData.listIndex].synopsis;
        summaryLeft.SetActive(true);
        summaryRight.SetActive(true);

        //If we are section 0 or 1, we can show the summary.
        //If we are 2 or more, hide the summary.
        if (pnpNovel.chapters[playerData.listIndex].section >= 2)
        {
            summaryText = "";
            summaryLeft.SetActive(false);
            summaryRight.SetActive(false);
        }

        //We know the chapter based on the player data.
        staticHeader.text = "Chapter " + pnpNovel.chapters[playerData.listIndex].chapterNumber + sectionContents;
        scrollHeader.text = "Chapter " + pnpNovel.chapters[playerData.listIndex].chapterNumber + sectionContents;

        staticSummary.text = summaryText;
        scrollSummary.text = summaryText;

        staticBody.text = pnpNovel.chapters[playerData.listIndex].bodyText;
        scrollBody.text = topBuffer + pnpNovel.chapters[playerData.listIndex].bodyText + endBuffer;

        //Are we ready to open to the detail view?
        if (openPage)
        {
            //These seem to be necessary to completely reset the scroll windows prior to the next chapter.
            eReader.TurnOff();
            eReader.NewChapterScrollReset();
            eReader.MaximizeView();
        }

        if (playerData.pnpChapter == (pnpNovel.chapters.Count - 1))
        {
            //This means we are on the last available chapter.  Turn off the next button.
            eReader.nextButton.SetActive(false);
        }
    }

    //Update the chapter content based on the current chapter.
    //Called by the next chapter button.
    public void NextChapter()
    {
        //Increase the chapter.
        playerData.listIndex++;
        SetPlayerBookData();
        DisplayChapter(true);
    }

    public void SetSection(int section)
    {
        this.section = section;
    }

    //When a chapter is selected specifically from the ToC hyperlinks.
    public void ChapterSelect(int chapterValue)
    {
        int actualChap = chapterValue;
        chapterValue--; //Take the chapter value down by one to get the actual index within the list.
        if (chapterValue < pnpNovel.chapters.Count)
        {
            //Make sure we get the right section!
            for (int i =0; i < pnpNovel.chapters.Count; i++)
            {
                if (pnpNovel.chapters[i].section == section && pnpNovel.chapters[i].chapterNumber == actualChap)
                {
                    //This is the one we can use to set our chapter.
                    playerData.listIndex = i;
                    SetPlayerBookData();
                    break;                  
                }
            }

//            playerData.listIndex = chapterValue;
            DisplayChapter(false);
        }
    }

    //Sets the player's chapter and section based on their current list index.
    public void SetPlayerBookData()
    {
        //We utilize the list index for this.
        playerData.pnpChapter = pnpNovel.chapters[playerData.listIndex].chapterNumber;
        playerData.pnpSection = pnpNovel.chapters[playerData.listIndex].section;
    }
}
