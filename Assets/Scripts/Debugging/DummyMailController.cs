using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyMailController : MonoBehaviour
{
    public List<GameObject> content;
    private DataManager playerData;

    public GameObject newMailIcon;

    public int contentIndex;

    public GameObject nextLetterButton;
    public GameObject closeLetterButton;

    public GameObject buffer;

    public GameObject mailList;

    // Start is called before the first frame update
    void Start()
    {
        contentIndex = 0;
        playerData = FindObjectOfType<DataManager>();
        if (playerData.mailAccessed)
        {
            newMailIcon.SetActive(false);
        }
        else
        {
            newMailIcon.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleMailList()
    {
        mailList.SetActive(!mailList.activeSelf);
    }

    public void OpenContent(int index)
    {
        buffer.SetActive(true);
        for (int i = 0; i < content.Count; i++)
        {
            if (i == index)
            {
                content[i].SetActive(true);
            }
            else
            {
                content[i].SetActive(false);
            }
        }
//        closeLetterButton.SetActive(true);
    }

    public void InitialContent()
    {
        if (playerData.mailAccessed)
        {
            content[0].SetActive(false);
            buffer.SetActive(false);
            nextLetterButton.SetActive(false);
            closeLetterButton.SetActive(false);
        }
        else
        {
            playerData.mailAccessed = true;
            newMailIcon.SetActive(false);
            content[0].SetActive(true);
            nextLetterButton.SetActive(true);
        }

    }

    public void CloseContent()
    {
        for (int i = 0; i < content.Count; i++)
        {
            content[i].SetActive(false);
        }
        closeLetterButton.SetActive(false);
        buffer.SetActive(false);
    }

    public void NextContent()
    {
        if (contentIndex < content.Count)
        {
            contentIndex++;
        }
        else
        {
        }

        content[contentIndex].SetActive(true);
        for (int i = 0; i < content.Count; i++)
        {
            if (i == contentIndex)
            {
                content[i].SetActive(true);
            }
            else
            {
                content[i].SetActive(false);
            }
        }

        if (contentIndex + 1 == content.Count)
        {
            //We have hit the end of the list.
 //           closeLetterButton.SetActive(true);
            nextLetterButton.SetActive(false);
        }
        else
        {
            closeLetterButton.SetActive(false);
            nextLetterButton.SetActive(true);
        }
    }
}
