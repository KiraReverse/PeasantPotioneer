using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTransparency : MonoBehaviour
{

    Color start;
    // Start is called before the first frame update
    void Start()
    {
        start = GetComponent<Renderer>().material.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "MainCamera")
        {
            GetComponent<Renderer>().material.color = new Color(start.r, start.g, start.b, 0.3f);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "MainCamera")
        {
            GetComponent<Renderer>().material.color = start;
        }
    }
}
