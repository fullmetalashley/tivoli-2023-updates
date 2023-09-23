using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreviewTableau : MonoBehaviour
{
    //References to the dolls and to the tableau controller.
    private TableauController tableauMain;
    private DollManager dollSystem;

    public GameObject previewMenu;

    public GameObject Jane;
    public GameObject Elizabeth;
    public GameObject dollSwap;

    public Image tableauPreview;

    //Is it better to move the dolls around with an array of positions, or is it better to use the actual tableau assets to show a preview of the finished tableau?  
    //If we show the preview of the actual tableau, I'm not sure how necessary the preview feature is.

    // Start is called before the first frame update
    void Start()
    {
   
    }

}
