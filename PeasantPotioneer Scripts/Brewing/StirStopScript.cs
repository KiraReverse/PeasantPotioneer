using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StirStopScript : MonoBehaviour
{
    public CauldronController cCon;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StopStir()
    {
        cCon.StirEnd();
    }
}
