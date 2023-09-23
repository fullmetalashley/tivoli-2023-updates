using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorChange : MonoBehaviour
{

    public Image background;
    public int colorTracker;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeColor()
    {
        colorTracker++;
        if (colorTracker < 5)
        {
            if (colorTracker == 1)
            {
                background.color = Color.yellow;
            }else if (colorTracker == 2)
            {
                background.color = Color.blue;
            }else if (colorTracker == 3)
            {
                background.color = Color.red;
            }else if (colorTracker == 4)
            {
                background.color = Color.green;
            }
        }
        else
        {
            background.color = Color.white;
            colorTracker = 0;
        }
    }
}
