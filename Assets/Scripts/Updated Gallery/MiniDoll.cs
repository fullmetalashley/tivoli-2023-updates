using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniDoll : MonoBehaviour
{
    public List<string> keys;
    public List<Image> clothing;
    public Image body;


    public bool init;

    public Dictionary<string, Image> clothingBank;

    void Start()
    {
        clothingBank = new Dictionary<string, Image>();
        for (int i = 0; i < keys.Count; i++)
        {
            clothingBank.Add(keys[i], clothing[i]);
        }
    }

    public void SetUpDatabase()
    {
        clothingBank = new Dictionary<string, Image>();
        for (int i = 0; i < keys.Count; i++)
        {
            clothingBank.Add(keys[i], clothing[i]);
        }
    }
}
