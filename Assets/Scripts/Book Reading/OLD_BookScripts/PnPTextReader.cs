using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PnPTextReader : MonoBehaviour
{
    public TextAsset theFile;
    public string breakValue;

    public Text testArea;

    public List<string> pages;
    public int pageIndex = -1;
    public int volume;
    public int chapter;
     

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayFile()
    {

        pageIndex++;
        if (pageIndex >= pages.Count)
        {
            pageIndex = 0;
        }
        testArea.text = pages[pageIndex];
    }

    public void BreakText()
    {
        string theText = theFile.text;
        string[] splitText = theText.Split(breakValue[0]);

        for(int i = 0; i < splitText.Length; i++)
        {
            pages.Add(splitText[i]);
        }

    }
}
