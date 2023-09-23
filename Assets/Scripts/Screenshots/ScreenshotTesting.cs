using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class ScreenshotTesting : MonoBehaviour
{
    //We are going to use this if a different location is preferred.
    public string persistentPath;
    public bool useOtherPath;

    public string ScreenCapDirectory;

    public Image theScreenshot;

    public List<Sprite> loadedImages;

    

    // Start is called before the first frame update
    void Start()
    {
        if (useOtherPath)
        {
            ScreenCapDirectory = persistentPath;
        }
        else
        {
            ScreenCapDirectory = Application.persistentDataPath;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveScreen()
    {

        ScreenCapture.CaptureScreenshot(ScreenCapDirectory + "/Ash Test.png");
        Debug.Log("ScreenCapture Taken!");
        Debug.Log("ScreenCap Location: " + ScreenCapDirectory);
        LoadScreenshot();
    }

    public void CheckToLoad()
    {
        for (int i = 0; i < Directory.GetFiles(ScreenCapDirectory).Length; i++)
        {
            if (Directory.GetFiles(ScreenCapDirectory)[i].Contains("Ash Test.png"))
            {
                Debug.Log("Something exists!");
                LoadScreenshot();
            }
        }
    }

    public void LoadScreenshot()
    {
         for (int i = 0; i < Directory.GetFiles(ScreenCapDirectory).Length; i++)
        {
            if (Directory.GetFiles(ScreenCapDirectory)[i].Contains("Ash Test.png"))
            {
                theScreenshot.sprite = LoadNewSprite(ScreenCapDirectory + "/Ash Test.png", 100.0f);
            }
        }
    }

    public Sprite LoadNewSprite(string FilePath, float PixelsPerUnit = 100.0f)
    {

        // Load a PNG or JPG image from disk to a Texture2D, assign this texture to a new sprite and return its reference

        Texture2D SpriteTexture = LoadTexture(FilePath);
        Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0), PixelsPerUnit);

        return NewSprite;
    }

    public static Texture2D LoadTexture(string FilePath)
    {

        // Load a PNG or JPG file from disk to a Texture2D
        // Returns null if load fails

        Texture2D Tex2D;
        byte[] FileData;

        if (File.Exists(FilePath))
        {
            FileData = File.ReadAllBytes(FilePath);
            Tex2D = new Texture2D(2, 2);           // Create new "empty" texture
            if (Tex2D.LoadImage(FileData))           // Load the imagedata into the texture (size is set automatically)
                return Tex2D;                 // If data = readable -> return texture
        }
        return null;                     // Return null if load failed
    }
}
