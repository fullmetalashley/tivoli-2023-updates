using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script purpose: Track whether or not the closet has been accessed for the first time and control the hospots accordingly.
public class ClosetHotspots : MonoBehaviour
{
    //SCRIPT REFS
    private DataManager playerData;

    //Hotspots
    public List<CHotspot> hotspots;

    // Start is called before the first frame update
    void Start()
    {
        playerData = FindObjectOfType<DataManager>();

        //All hotspots can be controlled by the mouse.
        for (int i = 0; i < hotspots.Count; i++)
        {
            hotspots[i].InitializeMouseControl();
        }
    }
}
