using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TableauSlider : MonoBehaviour
{

    Animator anim;
    public bool active;

    public Button slideActive;
    public Button slideClosed;

   

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetAnimator()
    {
        active = !active;
        anim.SetBool("active", active);

        //If the tableau is slid to the active position, we need to turn on the doll previews.
    }

    public void ToggleButtons()
    {
        slideActive.interactable = !slideActive.interactable;
        slideClosed.interactable = !slideClosed.interactable;
    }
}
