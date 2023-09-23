using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//Script purpose: When the mouse goes over this object, trigger the hint icon.
public class ImageMouseDetect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public CharmUI mainCharm;

    // Start is called before the first frame update
    void Start()
    {
        mainCharm = GetComponentInParent<CharmUI>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mainCharm.HintOn();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mainCharm.HintOff();
    }
}


