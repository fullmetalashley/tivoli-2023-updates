using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PnPDatabase : MonoBehaviour
{
    public static PnPDatabase control;

    public List<Chapter> chapters;

    void Awake()
    {
        if (control == null)
        {
            DontDestroyOnLoad(gameObject);
            control = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
