using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyManager : MonoBehaviour
{

    AudioSource sfx;
    public ClumsyController clumsyCon;
    // Start is called before the first frame update
    void Start()
    {
        sfx = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            if (clumsyCon.GetFallen())
            {
                if (!sfx.isPlaying)
                {
                    sfx.Play();
                }
            }
        }
        
    }
}
