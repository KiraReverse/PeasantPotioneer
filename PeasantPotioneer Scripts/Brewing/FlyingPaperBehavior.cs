using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingPaperBehavior : MonoBehaviour
{
    public float x;
    public float y;
    public float z;

    public float timer;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, timer);
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition += new Vector3(x,y,z);
    }
}
