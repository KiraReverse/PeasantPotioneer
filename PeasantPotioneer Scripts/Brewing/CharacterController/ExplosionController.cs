using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    public float blastForce;
    public float upwardForce;
    public Rigidbody player;
    public GameObject flame;
    public Vector3 flameScale;
    public ClumsyController clumCon;

    AudioSource sfx;
    CameraFollow cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.GetComponent<CameraFollow>();
        sfx = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {


            Vector3 dir = other.transform.position - transform.position;
            sfx.Play();
            player.AddExplosionForce(blastForce, transform.position, 5f, upwardForce, ForceMode.Impulse);
            cam.ShakeCam();
            StartCoroutine(Flare());

            GetComponent<Collider>().enabled = false;
            clumCon.PlayPain();

        }
    }

    IEnumerator Flare()
    {
        float t = 0;
        while(t < 3)
        {
            t += Time.deltaTime;
            flame.transform.localScale = Vector3.Lerp(flame.transform.localScale, flameScale, t / 3f);
            yield return null;
        }


        t = 0;

        while (t < 1)
        {
            t += Time.deltaTime;
            flame.transform.localScale = Vector3.Lerp(flame.transform.localScale, Vector3.zero, t / 1f);
            yield return null;
        }

        flame.SetActive(false);
        gameObject.SetActive(false);
    }
}
