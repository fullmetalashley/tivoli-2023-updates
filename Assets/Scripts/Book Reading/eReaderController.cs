using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script purpose: Controls the UI elements of the EReader, not the actual content.
public class eReaderController : MonoBehaviour
{
    //SCRIPT REFS
    private DataManager playerData;
    private PnPController controller;
    private PnPDatabase pnpDatabase;

    //UI elements
    public GameObject nextButton;   //At the bottom of each chapter   

    //Page elements
    public GameObject fixedScrollPage;
    public GameObject scrollingPage;
    public GameObject ToC;

    //Scroll elements
    public ScrollRect scrollingWindow;
    public RectTransform textWindow;
    public GameObject bookmark;
    public float scrollPosition = 1f;

    public float normalizedBookmarkIncrement;

    //Next button buffer
    public float nextButtonBuffer;
    public float lastScrollPos;
    public float currentScrollPos;
    public float ratio;
    public float increment;

    //Thresholds for bookmark.
    public float maxPos;
    public float minPos;
    public Vector2 startingPos;
   

    // Start is called before the first frame update
    void Start()
    {
        playerData = FindObjectOfType<DataManager>();
        controller = GetComponent<PnPController>();
        pnpDatabase = FindObjectOfType<PnPDatabase>();

        ratio = (maxPos - minPos);
        lastScrollPos = 1f;


    }

    //If we have previously saved data, set the positions to those.
    public void EstablishPreviousPositions()
    {
        scrollPosition = playerData.scrollValue;
        bookmark.GetComponent<RectTransform>().localPosition = new Vector2(bookmark.GetComponent<RectTransform>().localPosition.x,
            playerData.bookmarkPosition);

        Debug.Log("Actual pos at end of establish: " + bookmark.GetComponent<RectTransform>().localPosition.y);
    }

    //When the page preview is pressed, maximize to the full scroll window.
    public void MaximizeView()
    {
        if (playerData.bookRead)
        {
            EstablishPreviousPositions();
        }

        ToC.SetActive(false);
        fixedScrollPage.SetActive(false);
        scrollingPage.SetActive(true);

        ResetScroll();

        PositionNextButton();
    }

    //Turn the eReader off, set back to the original view.
    //Called by the Close button of the scrolling page.
    //Also called by the next chapter function.
    public void TurnOff()
    {
        //Save our positions to the player data.
        playerData.scrollValue = scrollingWindow.verticalNormalizedPosition;
        playerData.bookmarkPosition = bookmark.GetComponent<RectTransform>().localPosition.y;
        playerData.bookRead = true;

        ToC.SetActive(true);
        fixedScrollPage.SetActive(true);
        scrollingPage.SetActive(false);

        Debug.Log("Bookmark pos at end of turn off: " + playerData.bookmarkPosition);
    }

    //Set the next button at the bottom of the text.
    public void PositionNextButton()
    {
        //This will be set at the bottom of the text box.
        //How about we get the height of the text box?
        float height = textWindow.rect.height;
        nextButton.transform.localPosition = new Vector2(nextButton.transform.localPosition.x, -1 * ((height / 2) + nextButtonBuffer));

    }

    //We need to reset the scroll window each time the scroll page is turned on.
    public void ResetScroll()
    {
        scrollingWindow.verticalNormalizedPosition = scrollPosition;
    }

    //When the player hits the next button, reset the data so they can go back to the start of the chapter.
    public void NewChapterScrollReset()
    {
        playerData.scrollValue = 1f;
        playerData.bookmarkPosition = maxPos;
        Debug.Log("Bookmark pos at end of new chapter reset: " + playerData.bookmarkPosition);
    }

    //Changes the value of the bookmark as the scroll happens.
    public void AdjustPosition()
    {
        currentScrollPos = scrollingWindow.verticalNormalizedPosition;

        increment = currentScrollPos - lastScrollPos;

        lastScrollPos = currentScrollPos;

        float movement = increment * ratio;


        //Let's add our catch to make sure we do not go above or below the max and min values of the page.
        float currentPos = bookmark.GetComponent<RectTransform>().localPosition.y;
        float movementValue = currentPos + movement;


        //If either of these occurs, we are out of bounds, and we need to reset.
        if (movementValue > maxPos || movementValue < minPos)
        {
            if (movementValue > maxPos)
            {
                movementValue = maxPos;
            }
            else
            {
                movementValue = minPos;
            }
        }

        bookmark.GetComponent<RectTransform>().localPosition = new Vector2(bookmark.GetComponent<RectTransform>().localPosition.x,
    (movementValue));
    }
}
