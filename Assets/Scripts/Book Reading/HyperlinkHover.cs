using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//Script purpose: Control the hover state of the hyperlinks.
public class HyperlinkHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Text hyperlink;
    public string text;
    public string underlineText;

    void Start()
    {
        text = hyperlink.text;
        underlineText = "<i>" + text + "</i>";
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hyperlink.text = underlineText;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hyperlink.text = text;
    }
}
