using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tv_screen_face : MonoBehaviour {

    float lastDx;

    // Use this for initialization
    void Start ()
    {
        
    }
    
    // Update is called once per frame
    void Update () {
        var p = GameObject.FindGameObjectsWithTag("Player").FirstOrDefault();
        
        if (!p) 
            return;

        float dx = p.transform.position.x - transform.position.x;

        if (lastDx == 0f)
        {
            lastDx = dx;
            return;
        }
        
        if (Mathf.Sign(dx) != Mathf.Sign(lastDx))
        {
            GetComponent<Animator>().Play("teevie_smile_start");
        }

        lastDx = dx;
    }
}
