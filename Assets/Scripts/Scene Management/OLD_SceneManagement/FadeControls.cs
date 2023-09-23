using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeControls : MonoBehaviour
{
    Animator fadeAnim;
    public string sceneToLoad;
    public bool loadingScene;

    public GameObject theFade;

    private DataManager playerData;

    // Start is called before the first frame update
    void Start()
    {
        playerData = FindObjectOfType<DataManager>();
        fadeAnim = GetComponent<Animator>();

        if (playerData.dataExists)
        {
            theFade.SetActive(false);
            if (SceneManager.GetActiveScene().name == "Doll Dressing Room")
            {
                StartCoroutine(FadeIn(0f));
            }
        }
        else
        {
            if (SceneManager.GetActiveScene().name == "Story Map")  //We have loaded the story map.
            {
                Debug.Log("You're in the story map!");

            }
            if (SceneManager.GetActiveScene().name == "Doll Dressing Room" || SceneManager.GetActiveScene().name == "Jewelry Room")  //We have loaded the dressing room or the jewelry room.
            {

            }           
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator FadeIn(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        fadeAnim.SetBool("Fading Out", true);
        fadeAnim.SetBool("Fading In", false);
    }

    IEnumerator FadeOut(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        fadeAnim.SetBool("Fading In", true);
        fadeAnim.SetBool("Fading Out", false);
    }

    public void FadeToBlack(float seconds)
    {
        fadeAnim = GetComponent<Animator>();

        StartCoroutine(FadeOut(seconds));
    }

    public void TurnOffFade(float seconds)
    {
        fadeAnim = GetComponent<Animator>();
        StartCoroutine(FadeIn(seconds));
        
    }

    public void LoadScene()
    {
        if (loadingScene)
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else { 
            StartCoroutine(FadeIn(1.0f));
        }
    }
}
