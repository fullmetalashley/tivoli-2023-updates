using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenPostcards : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Signing()
    {
        DressingRoomUI dressing = FindObjectOfType<DressingRoomUI>();
  //      dressing.ScreenshotTableau();

        DataManager thePlayer = FindObjectOfType<DataManager>();

        StartCoroutine(StartLoad(2.0f));
    }

    IEnumerator StartLoad(float seconds) {
        yield return new WaitForSeconds(seconds);
        GetComponent<SceneLoader>().LoadScene();
    }
}
