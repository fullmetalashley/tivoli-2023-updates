using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script purpose: Control the new eReader using a solid scroll.
public class EReaderControllerV2 : MonoBehaviour
{
    //CHAPTER CONTROLS
    //List of each chapter game object.
    public List<ChapterContent> chapters;

    //Floating header content
    public Text floatingHeader;   //The permanent header that will be replaced by each chapter.
    public float buffer;

    //SCROLL CONTROLS
    public ScrollRect scrollRect;
    public List<float> contentPositions;

    //Chapter refs
    public int chapterIndex;  //Corresponds to list index in player data.

    //Page elements
    public GameObject fixedScrollPage;
    public GameObject scrollingPage;
    public GameObject ToC;
    public GameObject scrollingContent;

    //Fixed scroll elements
    public Text fixedHeader;
    public Text fixedSummary;
    public Text fixedBody;

    //SCRIPT REFS
    DataManager playerData;

    //DEBUGGING
    public float prevChapUpper;
    public float prevChapLower;

    public float upChapUpper;
    public float upChapLower;

    public float currentScrollPos;

    //Refs to the previous and upcoming chapter positions.
    public float upChapPos;
    public float prevChapPos;
    public float currChapPos;

    //Refs to the indexes.
    public int prevChapIndex;
    public int upChapIndex;

    // Start is called before the first frame update
    void Start()
    {
        playerData = FindObjectOfType<DataManager>();

        if (playerData != null)
        {
            if (playerData.bookRead)
            {
                chapterIndex = playerData.listIndex;
            }
        }

        FixedPageUpdate();
        ChapterToggle();
        RecalculateMetrics();
        UpdateScroll();
    }

    //Set the fixed page content to match the current chapter.
    public void FixedPageUpdate()
    {
        //Set the header
        fixedHeader.text = chapters[chapterIndex].GetComponent<ChapterContent>().chapterHeader.GetComponent<Text>().text;
        //Set the summary
        fixedSummary.text = chapters[chapterIndex].GetComponent<ChapterContent>().chapterSummary.text;
        //Set the body
        fixedBody.text = chapters[chapterIndex].GetComponent<ChapterContent>().chapterBody.text;
    }

    //ONLY keep current, previous, and future chapter on.
    //All other chapters are turned off.
    public void ChapterToggle()
    {
        for (int i =0; i < chapters.Count; i++)
        {
            //IF: our chapter index is i, i = chapterindex-1, i=chapterindex +1
            if (i == chapterIndex || i == chapterIndex - 1 || i == chapterIndex + 1)
            {
                chapters[i].gameObject.SetActive(true);

                //If this is the SPECIFIC chapter, turn off it's header.
                if (i == chapterIndex)
                {
                    chapters[i].GetComponent<ChapterContent>().chapterHeader.SetActive(false);
                }
                else
                {
                    chapters[i].GetComponent<ChapterContent>().chapterHeader.SetActive(true);
                }
            }
            else
            {
                chapters[i].gameObject.SetActive(false);
            }         
        }
    }

    //Determine what chapters are upcoming and what ones aren't, and get positions based on those.
    public void RecalculateMetrics()
    {
        //Our next chapters
        prevChapIndex = chapterIndex - 1;
        upChapIndex = chapterIndex + 1;

        //If we don't have a previous chapters...
        if (prevChapIndex < 0)
        {
            prevChapIndex = 0;
        }

        //If we don't have an upcoming chapter...
        if (upChapIndex == chapters.Count)
        {
            upChapIndex--;
        }

        //So now we have our upcoming chapters.  Let's get their metrics.
        prevChapPos = contentPositions[prevChapIndex];
        upChapPos = contentPositions[upChapIndex];

        //When we are higher up on the screen, we are more negative.  When we are lower on the screen, we are positive.
        //So the position + buffer is for the lower threshold.  The position - buffer is for the upper position.
        prevChapUpper = prevChapPos - buffer;
        prevChapLower = prevChapPos + buffer;

        upChapUpper = upChapPos - buffer;
        upChapLower = upChapPos + buffer;
    }

    //Load a current chapter if a ToC link is clicked.
    public void LoadChapter(int index)
    {
        chapterIndex = index;
        ChapterToggle();
        RecalculateMetrics();

        FixedPageUpdate();
        //Set the scroll content at the appropriate position.
        scrollingContent.GetComponent<RectTransform>().localPosition = new Vector2(scrollingContent.GetComponent<RectTransform>().localPosition.x, contentPositions[index]);
        floatingHeader.text = chapters[chapterIndex].GetComponent<ChapterContent>().chapterHeader.GetComponent<Text>().text;
        UpdateScroll();
    }

    //As the scroll is changed, run detection to see if we need to update the chapter header.
    public void UpdateScroll()
    {
        //We have a list of scroll values.  If our current value is near one of those, we need to set that as our index.
        float scrollingPosition = scrollingContent.GetComponent<RectTransform>().localPosition.y;

        currentScrollPos = scrollingPosition;
        RecalculateMetrics();

        //IF: case 1, we have entered the upcoming boundary.
        if (scrollingPosition >= upChapUpper && scrollingPosition <= upChapLower)
        {
            //Set the chapter index.
            chapterIndex = upChapIndex;
            ChapterToggle();
            floatingHeader.text = chapters[chapterIndex].GetComponent<ChapterContent>().chapterHeader.GetComponent<Text>().text;
            RecalculateMetrics();
        }

        else if (scrollingPosition >= prevChapUpper && scrollingPosition <= prevChapLower)
        {
            //Set the chapter index.
            chapterIndex = prevChapIndex;
            ChapterToggle();
            floatingHeader.text = chapters[chapterIndex].GetComponent<ChapterContent>().chapterHeader.GetComponent<Text>().text;
            RecalculateMetrics();
        }
        
        
    }

    //When the page preview is pressed, maximize to the full scroll window.
    public void MaximizeView()
    {
        ToC.SetActive(false);
        fixedScrollPage.SetActive(false);
        scrollingPage.SetActive(true);
    }

    //Turn the eReader off, set back to the original view.
    //Called by the Close button of the scrolling page.
    public void TurnOff()
    {
        //Save our positions to the player data.
        playerData.scrollValue = scrollRect.verticalNormalizedPosition;
        playerData.bookRead = true;

        ToC.SetActive(true);
        fixedScrollPage.SetActive(true);
        scrollingPage.SetActive(false);

        FixedPageUpdate();
    }
}
