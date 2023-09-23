using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script purpose: Control the randomization of the cat animation.
public class CatAnimControls : MonoBehaviour
{
    //Tail values
    [Header("Tail")]
    public Animator tailAnim;
    public float tailCountdown;
    public float minTailValue;
    public float maxTailValue;

    //Head values
    [Header("Head")]
    public Animator headAnim;
    public float headCountdown;
    public float minHeadValue;
    public float maxHeadValue;


    private void Start()
    {
        tailCountdown = maxTailValue;
        headCountdown = maxHeadValue;
    }

    // Update is called once per frame
    void Update()
    {
        //If the tail has not hit zero yet...
        if (tailCountdown > 0)
        {
            tailAnim.SetBool("active", false);
            tailCountdown -= Time.deltaTime;
        }
        else
        {
            //Run the tail animation.
            tailAnim.SetBool("active", true);

            //Randomize the tailCountdown again.
            tailCountdown = Random.Range(minTailValue, maxTailValue);
        }

        //If the head has not hit zero yet...
        if (headCountdown > 0)
        {
            headAnim.SetBool("active", false);
            headCountdown -= Time.deltaTime;
        }
        else
        {
            //Run the tail animation.
            headAnim.SetBool("active", true);

            //Randomize the tailCountdown again.
            headCountdown = Random.Range(minHeadValue, maxHeadValue);
        }
    }
}
