using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniToolTipControl : MonoBehaviour
{

    public bool autoActivate;
    public float secondsToLast;

    public GameObject thisButton;

    public Image mainScreen;

    public bool running;

    Animator anim;




    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        
    }

    public void EarlyShutDown()
    {
        StopAllCoroutines();
        running = false;
        anim.Play("FadeToHidden");
        anim.SetBool("active", false);

    }

    //This will be called from a main controller and will start the animation.
    public void StartAnimation()
    {

        anim = GetComponent<Animator>();
        anim.SetBool("active", true);
        StartCoroutine(TimerDelay(secondsToLast));
        
    }

    IEnumerator TimerDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        anim.SetBool("active", false);
        running = false;
    }
}
