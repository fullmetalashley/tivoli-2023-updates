using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostcardActivation : MonoBehaviour
{
    public int index;
    private PostcardControls postControls;
    // Start is called before the first frame update
    void Start()
    {
        postControls = FindObjectOfType<PostcardControls>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPostcard()
    {
        postControls.InitialDisplay(index);
    }
}
