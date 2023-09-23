using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class AndroidScreenshot : MonoBehaviour
{
    //Where files are saved.
    public string ScreenCapDirectory;

    //The current index of our photo.
    public int photoIndex;

    //The primary text messaging box.
    public Text messaging;

    // Start is called before the first frame update
    void Start()
    {
        photoIndex = 0;
        ScreenCapDirectory = Application.persistentDataPath;
    }

    public void SaveScreen()
    {
        //Clear the message box.
        messaging.text = "";

        if (CheckToLoad("Ash Test " + photoIndex + ".png"))
        {
            photoIndex++;
            SaveScreen();
        }
        else
        {
 /*           string fileName = "Ash Test " + photoIndex;
                       ScreenCapture.CaptureScreenshot("/Ash Test " + photoIndex + ".png");     //Save the photo in a PNG format.
                       messaging.text += "\nWe successfully saved the screenshot!  The name is Ash Test " + photoIndex + ".png\n " +
            
            "And the location is " + ScreenCapDirectory + "/Ash Test " + photoIndex + ".png";

            string newFilePath = ScreenCapDirectory + "/Ash Test " + photoIndex + ".png";

            //Using external plugin...

*/

            //Let's focus on trying to do the entire thing with the plugin.
            NativeGallery.RequestPermission();
            if (NativeGallery.CheckPermission() == NativeGallery.Permission.Granted)
            {

                StartCoroutine(TakeScreenshotAndSave());
            }
            photoIndex++;    //Increase the index.
        }      
    }

    //This takes the screenshot as a texture and exports it to the designated folder.
    private IEnumerator TakeScreenshotAndSave()
    {
        yield return new WaitForEndOfFrame();

        Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        ss.Apply();

        DateTime currentDate = new DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day);

        string fileName = "Tableau " + photoIndex + " " + currentDate.Month + "-" + currentDate.Day + "-" + currentDate.Year + ".png";
        //Saving to Gallery / Photos
        messaging.text = "Permission result: " + NativeGallery.SaveImageToGallery(ss, "Tivoli Tableaus", fileName);

        //To avoid memory leaks
        Destroy(ss);

    }

    //Checks to see if that file is already in the directory.
    //Adjust it so it takes a file name in, and returns true if that file already exists.
    public bool CheckToLoad(string fileName)
    {
        messaging.text += "We are checking to see if the file exists...";
        for (int i = 0; i < Directory.GetFiles(ScreenCapDirectory).Length; i++)
        {
            if (Directory.GetFiles(ScreenCapDirectory)[i].Contains(fileName))
            {
                return true;
            }
        }
        return false;
    }

    //Clear all screenshots.
    public void ClearScreenshots()
    {
        messaging.text = "";
        int deleteCount = 0;
        string[] fileNames = Directory.GetFiles(Application.persistentDataPath);
        for (int i = 0; i < fileNames.Length; i++)
        {
            if (fileNames[i].Contains("Ash Test"))
            {
                messaging.text += "\nA test file exists here!";
                File.Delete(fileNames[i]);
                deleteCount++;
            }
        }
        messaging.text += "We deleted " + deleteCount + " screenshots.";
    }
}
