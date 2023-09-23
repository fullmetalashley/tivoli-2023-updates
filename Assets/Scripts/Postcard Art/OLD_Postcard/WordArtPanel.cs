using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WordArtPanel : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Image panelArt;
    public bool hasArt;
    public bool mouseOnSlot;

    public void OnDrop(PointerEventData eventData)
    {
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOnSlot = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOnSlot = false;
    }
}
