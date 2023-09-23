using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataReset : MonoBehaviour
{

    private DataManager thePlayer;
    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<DataManager>();
    }

    public void Reset()
    {
        thePlayer.GetComponent<SaveSystem>().ClearData();
    }
}
