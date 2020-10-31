using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThudController : MonoBehaviour
{
    AudioSource sfx; 

    // Start is called before the first frame update
    void Start()
    {
        sfx = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!sfx.isPlaying)
        {
            sfx.Play();
        }
    }
}
