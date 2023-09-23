using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NavController : MonoBehaviour
{
    Animator anim;
    public bool slideActive;

    public List<string> scenes;
    public List<Sprite> icons;



    private Dictionary<string, Sprite> buttonIcons;
    public List<Image> buttonImages;

    private DataManager playerData;

 //   public GameObject tableauSelection;

    public Button activator;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        buttonIcons = new Dictionary<string, Sprite>();
        playerData = FindObjectOfType<DataManager>();


        for (int i = 0; i < scenes.Count; i++)
        {
            buttonIcons.Add(scenes[i], icons[i]);
        }
    }


    public void ToggleSlide()
    {
        slideActive = !slideActive;
        anim.SetBool("SlideToActive", slideActive);

 /*       if (!playerData.navGlowEncountered)
        {
            playerData.navGlowEncountered = true;
            FindObjectOfType<NavParticleDetect>().gameObject.SetActive(false);
        }
        */

        if (slideActive)
        {
            //Run through the list of what sprites need to be toggled on.
            //If Dressing Room: Mirror, Tableau, Story Map
            //If Mirror: Wardrobe, Tableau, Story Map
            //If Gallery: Wardrobe, Mirror, Story Map
            string currentScene = SceneManager.GetActiveScene().name;
            Debug.Log("CURRENT SCENE: " + currentScene);
            if (currentScene == "Doll Dressing Room Remodel")
            {
                buttonImages[0].sprite = buttonIcons["Jewelry Room"];
                buttonImages[0].GetComponent<SceneLoader>().SceneToLoad = "Jewelry Room";

                buttonImages[1].sprite = buttonIcons["Gallery"];
                buttonImages[1].GetComponent<SceneLoader>().SceneToLoad = "Gallery";

                buttonImages[2].sprite = buttonIcons["Story Map"];
                buttonImages[2].GetComponent<SceneLoader>().SceneToLoad = "Story Map";

            }
            else if (currentScene == "Jewelry Room")
            {
                buttonImages[0].sprite = buttonIcons["Doll Dressing Room Remodel"];
                buttonImages[0].GetComponent<SceneLoader>().SceneToLoad = "Doll Dressing Room Remodel";

                buttonImages[1].sprite = buttonIcons["Gallery"];
                buttonImages[1].GetComponent<SceneLoader>().SceneToLoad = "Gallery";

                buttonImages[2].sprite = buttonIcons["Story Map"];
                buttonImages[2].GetComponent<SceneLoader>().SceneToLoad = "Story Map";

            }
            else if (currentScene == "Gallery")
            {
                buttonImages[0].sprite = buttonIcons["Doll Dressing Room"];
                buttonImages[0].GetComponent<SceneLoader>().SceneToLoad = "Doll Dressing Room Remodel";

                buttonImages[1].sprite = buttonIcons["Jewelry Room"];
                buttonImages[1].GetComponent<SceneLoader>().SceneToLoad = "Jewelry Room";

                buttonImages[2].sprite = buttonIcons["Story Map"];
                buttonImages[2].GetComponent<SceneLoader>().SceneToLoad = "Story Map";

            }
        }
    }
    public void DisableButton()
    {
        Debug.Log("Deactivated!");
        activator.interactable = false;
    }

    public void EnableButton()
    {
        Debug.Log("Activated!");
        activator.interactable = true;
    }
}
