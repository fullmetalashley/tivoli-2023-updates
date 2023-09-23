using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//Script purpose: Control whether or not the hotspot is on at full power.
public class CHotspot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Color full;
    public Color transparency;

    public bool mouseAllowed;

    public Image hotspot;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (mouseAllowed)
        {
            hotspot.color = full;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (mouseAllowed)
        {
            hotspot.color = transparency;
        }
    }

    public void InitializeMouseControl()
    {
        hotspot.color = transparency;
        mouseAllowed = true;
    }
}
