using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public GameObject menuModal;
    public GameObject menuButtons;

    public List<GameObject> windows;
    public List<string> windowCategories;

    public Dictionary<string, GameObject> menus;

    public void Start()
    {
        menus = new Dictionary<string, GameObject>();
        for (int i =0; i < windowCategories.Count; i++)
        {
            menus.Add(windowCategories[i], windows[i]);
        }
    }
    //Turn the main menu on and off.
    public void ToggleModal()
    {
        menuModal.SetActive(!menuModal.activeSelf);
        menuButtons.SetActive(menuModal.activeSelf);

        //If we are in the Story Map, turn off the particles.
        if (FindObjectOfType<PassiveElementManager>() != null)
        {
            FindObjectOfType<PassiveElementManager>().ToggleParticles(menuModal.activeSelf);
        }
    }

    //Toggle an individual window within the global menu menu on and off.
    public void ToggleWindow(string window)
    {
        menus[window].SetActive(!menus[window].activeSelf);
        menuButtons.SetActive(!menus[window].activeSelf);
    }
}
