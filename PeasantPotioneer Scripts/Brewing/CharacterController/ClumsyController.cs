using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class ClumsyController : MonoBehaviour
{
    public Transform head;
    public Rigidbody leftFoot;
    public Rigidbody rightFoot;
    public Rigidbody leftKnee;
    public Rigidbody rightKnee;

    public float footForce = 400f;
    public float kneeForce = 200f;
    bool isLeft = false;

    public float stepSize;
    float stepTime = 0;

    public Rigidbody neck;
    public float headForce = 200;


    public Rigidbody leftUpperArm;
    public Rigidbody rightUpperArm;
    public Rigidbody leftLowerArm;
    public Rigidbody rightLowerArm;

    public float armForce = 200f;
    public float handForce = 100f;


    public ConstantForce headLift;
    public ConstantForce pelvisLift;

    public GroundCheck[] groundCheck;

    bool rising = false;
    bool fallen = false;
    bool walking = false;
    Coroutine wakeUp = null;


    public AudioClip[] grunts;
    public AudioSource gruntSfx;

    public AudioClip[] groans;
    public AudioSource groanSfx;

    public AudioClip[] pains;
    public AudioSource painSfx;

    int gCounter = 2;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        stepTime -= Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (!rising)
        {
            if (Input.GetAxis("Vertical") != 0 && stepTime <= 0)
            {
                AlternateFoot();
                walking = true;
            }

            else
            {
                walking = false;
            }

            if (Input.GetAxis("Horizontal") != 0)
            {
                TurnHead();
            }

            if (Input.GetKey(KeyCode.Mouse0))
            {
                RaiseLeftArm();
            }

            if(Input.GetKey(KeyCode.Mouse1))
            {
                RaiseRightArm();
            }

            CheckFeet();
        }
    }

    void AlternateFoot()
    {
        if(gCounter > 3)
        {

            PlayGrunt();
            gCounter = 0;
        }

        ++gCounter;

        if (isLeft)
        {

            leftKnee.AddForce(head.transform.right*-1 * kneeForce * Input.GetAxis("Vertical"), ForceMode.Force);
            leftFoot.AddForce(head.transform.up * footForce * Input.GetAxis("Vertical"), ForceMode.Force);
            isLeft = false;
        }

        else
        {

            rightKnee.AddForce(head.transform.right*-1 * kneeForce * Input.GetAxis("Vertical"), ForceMode.Force);
            rightFoot.AddForce(head.transform.up * footForce * Input.GetAxis("Vertical"), ForceMode.Force);
            isLeft = true;
        }
        

        
        stepTime = stepSize;
    }

    void TurnHead()
    {
        neck.AddTorque(head.transform.right * -1 * headForce * Input.GetAxis("Horizontal"), ForceMode.VelocityChange);
    }

    void RaiseLeftArm()
    {
        leftUpperArm.AddTorque(head.transform.up * armForce, ForceMode.Force);
        leftLowerArm.AddTorque(head.transform.forward * handForce, ForceMode.Force);
    }

    void RaiseRightArm()
    {
        rightUpperArm.AddTorque(head.transform.up * armForce, ForceMode.Force);
        rightLowerArm.AddTorque(head.transform.forward * handForce, ForceMode.Force);
    }

    void CheckFeet()
    {
        if (!groundCheck[0].grounded && !groundCheck[1].grounded && !rising && !fallen)
        {
            fallen = true;
            headLift.enabled = false;
            pelvisLift.enabled = false;
        }

        if(fallen && !rising)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                wakeUp = StartCoroutine(Rising());
            }
        }

        
    }

    void PlayGrunt()
    {
        int i = Random.Range(1, grunts.Length);
        gruntSfx.clip = grunts[i];
        gruntSfx.PlayOneShot(gruntSfx.clip);

        grunts[0] = grunts[i];
        grunts[0] = gruntSfx.clip;

    }

    void PlayGroan()
    {
        int i = Random.Range(1, groans.Length);
        groanSfx.clip = groans[i];
        groanSfx.PlayOneShot(groanSfx.clip);

        groans[0] = groans[i];
        groans[0] = groanSfx.clip;

    }

    public void PlayPain()
    {
        int i = Random.Range(1, pains.Length);
        painSfx.clip = pains[i];
        painSfx.PlayOneShot(painSfx.clip);

        pains[0] = pains[i];
        pains[0] = painSfx.clip;

    }

    public bool GetWalking()
    {
        return walking;
    }

    public bool GetFallen()
    {
        return fallen;
    }

    IEnumerator Rising()
    {
        PlayGroan();
        rising = true;
        
        wakeUp = null;
        Vector3 force = headLift.force;
        headLift.force = Vector3.zero;

        headLift.enabled = true;
        pelvisLift.enabled = true;

        float time = 0;
        while ( time < 2f)
        {
            time += Time.deltaTime;
            headLift.force = Vector3.Lerp(headLift.force, force, 5*Time.deltaTime);

            yield return null;
        }
        headLift.force = force;
        fallen = false;
        rising = false;
    }
}
