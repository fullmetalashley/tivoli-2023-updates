using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CBControls : MonoBehaviour
{
    public List<GameObject> bookPages;
    public GameObject nextArrow;
    public GameObject closeCb;
    public int index;

    public List<Sprite> historyPages;
    public int historicalIndex;

    public Image bookContent;

    private CBFashionsDisplay theFashions;

    public GameObject commonplaceBook;
    public GameObject pnpObject;


    private CBTableauLoading tableauLoader;

    private DataManager playerData;

    // Start is called before the first frame update
    void Start()
    {
        theFashions = FindObjectOfType<CBFashionsDisplay>();
        tableauLoader = GetComponent<CBTableauLoading>();
        playerData = FindObjectOfType<DataManager>();



    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangePageHistorical()
    {
        historicalIndex++;
        if (historicalIndex >= historyPages.Count)
        {
            historicalIndex = 0;
        }
        bookContent.sprite = historyPages[historicalIndex];
    }

    

    public void InitialDisplay()
    {
        bookPages[0].SetActive(true);
        nextArrow.SetActive(true);
        closeCb.SetActive(true);
        commonplaceBook.SetActive(true);
        theFashions.ParseList();
    }

    public void CloseCommon()
    {
        for (int i = 0; i < bookPages.Count; i++)
        {
            bookPages[i].SetActive(false);
        }
        nextArrow.SetActive(false);
        closeCb.SetActive(false);
        commonplaceBook.SetActive(false);
        index = 0;
    }

    public void NextPage()
    {
        index++;
        historicalIndex = 0;
        if (index >= bookPages.Count)
        {
            index = 0;
        }
        for (int i = 0; i < bookPages.Count; i++)
        {
            if (i == index)
            {
                bookPages[i].SetActive(true);
                
            }
            else
            {
                bookPages[i].SetActive(false);
            }
        }
        if (index == 2)
        {
            tableauLoader.LoadTableaus();
        }
    }
}
