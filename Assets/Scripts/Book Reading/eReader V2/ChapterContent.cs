using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script purpose: Toggle chapter content on and off as chapters scroll into view.
public class ChapterContent : MonoBehaviour
{
    //UI Elements
    public GameObject chapterHeader;
    public Text chapterBody;
    public Text chapterSummary;

    public int index;
}
