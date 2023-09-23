using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Script purpose: Load a new scene.
public class SceneLoader : MonoBehaviour
{
    //Set this string in the Unity Editor.
    public string SceneToLoad;

    public void LoadScene()
    {

        //Establish the last scene in the Player Data as this current scene.
        //6/4/2020: Not as necessary now with the updated nav system; could be removed.
        if (SceneManager.GetActiveScene().name == "Doll Dressing Room Remodel")
        {
            FindObjectOfType<DollSkin>().SaveDolls();
        }

        if (SceneManager.GetActiveScene().name == "Jewelry Room")
        {
            FindObjectOfType<JewelryDollSkin>().SaveDolls();
        }


        //       FindObjectOfType<PersistentAudio>().ChangeMusic(SceneToLoad);
        FindObjectOfType<PersistentCrossfade>().ChangeMusic(SceneToLoad);

        SceneManager.LoadScene(SceneToLoad);
    }

    public void LoadScene(string scene)
    {
        SceneToLoad = scene;

        if (SceneManager.GetActiveScene().name == "Doll Dressing Room Remodel")
        {
            FindObjectOfType<DollSkin>().SaveDolls();
        }

        if (SceneManager.GetActiveScene().name == "Jewelry Room")
        {
            FindObjectOfType<JewelryDollSkin>().SaveDolls();
        }

        //        FindObjectOfType<PersistentAudio>().ChangeMusic(SceneToLoad);
        FindObjectOfType<PersistentCrossfade>().ChangeMusic(SceneToLoad);
        FindObjectOfType<SFXController>().StopOneShot();

        SceneManager.LoadScene(scene);

    }


}
