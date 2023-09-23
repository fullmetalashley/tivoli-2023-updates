using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugClear : MonoBehaviour
{

    private DataManager thePlayer;
    private SaveSystem theSave;
    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<DataManager>();
        theSave = thePlayer.GetComponent<SaveSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClearData()
    {
        theSave.ClearData();
        SceneManager.LoadScene("Start Screen");
    }
}
