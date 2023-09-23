using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClothingSlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    //If the pointer has entered the slot, the clothing can be dropped.
    public bool mouseOnSlot;
    //If the slot is currently storing clothing or not.  Not sure if this is as necessary as tracking the dolls to see if they have sprites.
    public bool hasClothing;

    public bool isActive;

    

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
