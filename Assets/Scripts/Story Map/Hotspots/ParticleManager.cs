using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script purpose: Control the particle systems within the Story Map for appropriate triggers / animations.
public class ParticleManager : MonoBehaviour
{
    public List<string> hotspotNames;
    public List<GameObject> hotspotParticles;

    public Dictionary<string, GameObject> particles;

    public List<HotspotMouseOver> hotspotControls;

    // Start is called before the first frame update
    void Start()
    {
        particles = new Dictionary<string, GameObject>();

        //Set up the dictionary with all of the hotspots.
        for (int i = 0; i < hotspotNames.Count; i++)
        {
            particles.Add(hotspotNames[i], hotspotParticles[i]);
        }

        if (!FindObjectOfType<DataManager>().particlesOn)
        {
            DisableParticles();
        }
    }

    public void EstablishPlayerSettings(bool setting)
    {
        for (int i = 0; i < hotspotControls.Count; i++)
        {
            hotspotControls[i].particleAllowed = setting;
        }
    }

    //Turns that particular particle system on or off based on its current state.
    public void ToggleParticle(string hotspot)
    {
        if (particles[hotspot] != null)
        {
            particles[hotspot].SetActive(!particles[hotspot].activeSelf);
        }
    }

    //Toggles the particles on and off.
    public void DisableParticles()
    {
        for (int i =0; i < hotspotControls.Count; i++)
        {
            hotspotControls[i].particleAllowed = !hotspotControls[i].particleAllowed;
        }
    }
}
