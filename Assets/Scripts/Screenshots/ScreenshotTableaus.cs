using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

//This script specifically takes the visible tableau and saves it to the player's commonplace book.
//It also saves it locally to the player's project database.
public class ScreenshotTableaus : MonoBehaviour
{
    //The first half of the location where the images will be saved.  
    //Most likely directs to the application file.
    public string persistentPath;
    //This bool will be used to help with debugging and saving the image in other locations.
    public bool useOtherPath;
    //This is the directory where all of the images will be saved.
    public string ScreenCapDirectory;

    public string MacDirectory;

    //This tracks the number of photos saved so we can adjust the file name to avoid overlap.
    //Might be better to save it with a date in the future.
    public int photoIndex;

    //A reference to the player data, in order to add the sprite to it.
    private DataManager playerData;
    
    //A string storing where the screenshot was placed.
    private string currentScreenshotName;

    // Start is called before the first frame update
    void Start()
    { 

        playerData = FindObjectOfType<DataManager>();
        ScreenCapDirectory = playerData.tableauDirectory;
    }

    //This saves the file directly to the directory.
    public void SaveScreen()
    {
        //If the file already exists, adjust the photo index and try again.
        if (CheckToLoad("Austen" + photoIndex + ".png"))
        {
            photoIndex++;
            SaveScreen();
        }
        else
        {
            ScreenCapture.CaptureScreenshot(ScreenCapDirectory + "/Austen" + photoIndex + ".png");     //Save the photo in a PNG format.
            currentScreenshotName = "Austen" + photoIndex + ".png";
            photoIndex++;    //Increase the index.

            FindObjectOfType<TableauManager>().InitiateDelay();
        }
    }

    //Checks to see if that file is already in the directory.
    //Adjust it so it takes a file name in, and returns true if that file already exists.
    public bool CheckToLoad(string fileName)
    {
        for (int i = 0; i < Directory.GetFiles(ScreenCapDirectory).Length; i++)
        {
            if (Directory.GetFiles(ScreenCapDirectory)[i].Contains(fileName))
            {
                return true;
            }
        }
        return false;
    }

    public void AddToList()
    {

        //First: Are we at 9?  If so, we need to replace the oldest tableau.
        if (playerData.savedTableaus.Count == 9)
        {
            //First, sort the list.  Sort by name.
            playerData.savedTableaus.Sort();

            //Replace the oldest one with the new tableau.
            if (CheckToLoad(currentScreenshotName))
            {
                playerData.savedTableaus[0] = LoadNewSprite(ScreenCapDirectory + "/" + currentScreenshotName);
            }
        }
        else
        {
            if (CheckToLoad(currentScreenshotName))
            {
                playerData.savedTableaus.Add(LoadNewSprite(ScreenCapDirectory + "/" + currentScreenshotName));
            }
        }
    }

    // Load a PNG or JPG image from disk to a Texture2D, assign this texture to a new sprite and return its reference.
    public Sprite LoadNewSprite(string FilePath, float PixelsPerUnit = 100.0f)
    {
        Texture2D SpriteTexture = LoadTexture(FilePath);
        Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0), PixelsPerUnit);
        return NewSprite;
    }

    //A PNG is loaded from disk into a Texture2D
    //Will return null if the load fails.
    public static Texture2D LoadTexture(string FilePath)
    {
        Texture2D Tex2D;
        byte[] FileData;

        if (File.Exists(FilePath))
        {
            FileData = File.ReadAllBytes(FilePath);
            Tex2D = new Texture2D(2, 2);           // Create new "empty" texture
            if (Tex2D.LoadImage(FileData))           // Load the imagedata into the texture (size is set automatically)
                return Tex2D;                 // If data = readable -> return texture
        }
        else
        {
            Debug.Log("The file didn't exist.");
        }
        return null;                     // Return null if load failed
        
    }
}
