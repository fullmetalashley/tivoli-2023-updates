using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script purpose: Enable each paginator to work based on category changes.
public class PaginationDirector : MonoBehaviour
{
    public List<string> categories;
    public List<PaginationControl> controllers;

    public Dictionary<string, PaginationControl> pages;

    public bool initialized;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    //A safety method to initialize the data, just in case the player has arrived here from an item delivery.
    public void Initialize()
    {
        pages = new Dictionary<string, PaginationControl>();
        for (int i = 0; i < categories.Count; i++)
        {
            pages.Add(categories[i], controllers[i]);
        }
        initialized = true;
    }

    public void CheckPagination(string category)
    {
        if (!initialized)
        {
            Initialize();
        }
        pages[category].CheckForPagination();
    }

    public void DisablePagination(string category)
    {
        if (!initialized)
        {
            Initialize();
        }
        pages[category].TurnOffPagination();
    }

    //Turn off all lists.
    public void SetupControllers()
    {
        for (int i = 0; i < controllers.Count; i++)
        {
            controllers[i].Initialize();
        }
    }

}
