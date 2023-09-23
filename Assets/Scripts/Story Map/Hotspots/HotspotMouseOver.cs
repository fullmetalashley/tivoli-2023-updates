using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//Calls the particle manager to toggle hotspot particles on and off as necessary.
public class HotspotMouseOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool particleAllowed;    //HAs the user enabled particle settings?
    public string thisHotspot;  //Identifier for the particle manager.

    //When the pointer enters a hotspot, the manager is called.
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (particleAllowed)
        {
            FindObjectOfType<ParticleManager>().ToggleParticle(thisHotspot);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (particleAllowed)
        {
            if (FindObjectOfType<ParticleManager>() != null)
            {
                FindObjectOfType<ParticleManager>().ToggleParticle(thisHotspot);
            }
        }

    }

    public void InterruptParticles()
    {
        if (particleAllowed)
        {
            FindObjectOfType<ParticleManager>().ToggleParticle(thisHotspot);
        }
    }
}
