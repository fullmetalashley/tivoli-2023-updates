using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Script purpose: Control the subnavigation menu and allow the player to transition between scenes.
//Subnav is currently static, so it does not need to be animated.
public class SubnavigationControl : MonoBehaviour
{
    public List<Image> highlights;
    public List<string> sceneNames;

    public Dictionary<string, Image> highlightImages;

    // Start is called before the first frame update
    void Start()
    {
        highlightImages = new Dictionary<string, Image>();

        for (int i =0; i < highlights.Count; i++)
        {
            highlightImages.Add(sceneNames[i], highlights[i]);
        }

        //Turn all images off at the start.
        foreach(KeyValuePair<string, Image> pair in highlightImages)
        {
            pair.Value.enabled = false;
        }

        EnableHighlight();
    }

    //Turn on the highlight corresponding to this scene.
    public void EnableHighlight()
    {
        highlightImages[SceneManager.GetActiveScene().name].enabled = true;
    }

    //Load a scene based on the button pressed.
    public void ChangeScenes(string scene)
    {
        this.gameObject.GetComponent<SceneLoader>().LoadScene(scene);
    }
}
