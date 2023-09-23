using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class CBTableauLoading : MonoBehaviour
{
    //The directory of the tableaus.
    public string ScreenCapDirectory;

    //The starting name of tableaus.
    public string tableauNamingHeader;

    //Reference to the player data.
    private DataManager playerData;

    //The initial X location of the prefabs.
    public int xStarting;
    public int yStarting; //Initial Y location
    public int xBuffer; //The distance we need to add for the x position
    public int yBuffer; //The distance we need to add for the y position

    public int currentRow; //The current row we're on.  Rows should always be 2 objects.

    //The tableau prefab.
    public GameObject tableauPrefab;

    //The common book page, which needs to be the parent of these objects.
    public GameObject tableauParent;

    // Start is called before the first frame update
    void Start()
    {
        playerData = FindObjectOfType<DataManager>();
        currentRow = 0;
    }

    public void LoadTableaus()
    {
        Debug.Log("Now loading...");
        for (int i = 0; i < playerData.savedTableaus.Count; i++)
        {
            Debug.Log("Loading object " + i);
            int prefabX = 0;
            int prefabY = 0;
            if (i%2 == 0) //If the index is even, x is always equal to the start point.
            {
                prefabX = xStarting;
            }
            else //If the index is odd, x is always equal to the initial buffer + start point.
            {
                prefabX = xStarting + xBuffer;
            }
            prefabY = yStarting - (currentRow * yBuffer);

            //Instantiate prefab.
            GameObject newTableau = Instantiate(tableauPrefab, new Vector3(prefabX, prefabY, 0), Quaternion.identity);
            newTableau.transform.SetParent(tableauParent.transform);
            newTableau.GetComponent<RectTransform>().localPosition = new Vector3(prefabX, prefabY, 0);

            newTableau.GetComponent<Image>().sprite = playerData.savedTableaus[i];
            newTableau.GetComponent<PostcardActivation>().index = i;
            newTableau.GetComponentInChildren<Text>().text = playerData.tableauDates[i].Month + "/" + playerData.tableauDates[i].Day + "/" +
                playerData.tableauDates[i].Year;

            //After this, change the current row if necessary.
            if (i%2 != 0)
            {
                currentRow++;
            }

        }
    }

    
}
