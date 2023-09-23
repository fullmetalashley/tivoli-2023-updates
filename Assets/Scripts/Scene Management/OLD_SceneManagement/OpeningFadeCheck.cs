using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningFadeCheck : MonoBehaviour
{
    private DataManager playerData;
    public GameObject thisFade;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        thisFade.SetActive(false);
    }
}
