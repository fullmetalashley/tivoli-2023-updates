using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//Script purpose: Track the amount of clicks the cat has gotten.
public class CatClickDetection : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Click!");
        FindObjectOfType<DataManager>().catClicks++;
        FindObjectOfType<CatAudio>().PlayPurr();
    }
}
