using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script purpose: Toggle the various settings found in the global settings manager.
public class SettingsControls : MonoBehaviour
{
    public Toggle music;
    public Toggle sfx;
    public Toggle toolTip;

    public void Start()
    {
        //We need to re-establish this each time we open the scene.
        FindObjectOfType<PersistentCrossfade>().EstablishPlayerSettings(FindObjectOfType<DataManager>().musicOn);
        FindObjectOfType<SFXController>().EstablishPlayerSettings(FindObjectOfType<DataManager>().SFXOn);

        
        music.SetIsOnWithoutNotify(FindObjectOfType<DataManager>().musicOn);
        sfx.SetIsOnWithoutNotify(FindObjectOfType<DataManager>().SFXOn);
    }

    //Mute the persistent audio if it is currently running, otherwise turn it back on.
    public void ToggleMusic()
    {
        FindObjectOfType<DataManager>().musicOn = !FindObjectOfType<DataManager>().musicOn;
        FindObjectOfType<PersistentCrossfade>().EstablishPlayerSettings(FindObjectOfType<DataManager>().musicOn);
    }

    //Toggle sound effects.
    public void ToggleSFX()
    {
        FindObjectOfType<DataManager>().SFXOn = !FindObjectOfType<DataManager>().SFXOn;
        FindObjectOfType<SFXController>().EstablishPlayerSettings(FindObjectOfType<DataManager>().SFXOn);
    }

    //Toggles particle settings on the hot spots.
    public void ToggleParticles()
    {
        FindObjectOfType<DataManager>().particlesOn = !FindObjectOfType<DataManager>().particlesOn;
        FindObjectOfType<ParticleManager>().EstablishPlayerSettings(FindObjectOfType<DataManager>().particlesOn);
    }

}
