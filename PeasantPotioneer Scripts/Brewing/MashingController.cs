using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MashingController : MonoBehaviour
{
    public float mashPercent;

    public Transform mashWP;
    public GameObject progressBar;
    public Slider mashProgress;

    public AudioSource sfx;
    public AudioSource completeSfx;

    public Animator anim;

    GameObject mashing;
    bool canMash;
    bool isComplete = false;

    // Start is called before the first frame update
    void Start()
    {
        progressBar.SetActive(false);
        canMash = false;
        mashing = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            canMash = true;
            anim.SetBool("isReady", true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (canMash && mashing != null)
            {
                Mashing();
                
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            canMash = false;
            anim.SetBool("isReady", false);
            anim.SetBool("isPounding", false);
        }
    }

    void Mashing()
    {
        if (mashing.GetComponent<IngredientProperties>().prepPercent >= 100)
        {
            mashing.GetComponent<IngredientProperties>().prepState = State.mashed;
            mashing.GetComponent<IngredientProperties>().prepPercent = 100;

            if(!isComplete)
            {
                completeSfx.Play();
            }

            isComplete = true;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!anim.GetBool("isPounding"))
                {
                    anim.SetBool("isPounding", true);
                    mashing.GetComponent<IngredientProperties>().prepState = State.mashing;
                    mashing.GetComponent<IngredientProperties>().prepPercent += mashPercent;

                    mashing.transform.localScale -= new Vector3(0.02f, 0.02f, 0.02f);

                    mashProgress.value = mashing.GetComponent<IngredientProperties>().prepPercent;

                    if (!sfx.isPlaying)
                    {
                        sfx.Play();
                    }
                }
                
            }
        }

    }

    public bool CheckPlace()
    {
        if (mashing == null)
        {
            return true;
        }

        return false;
    }

    public void Place(GameObject held)
    {
        if (mashing == null)
        {
            isComplete = false;
            mashing = held;
            mashing.transform.localRotation = Quaternion.identity;
            mashing.transform.parent = mashWP;
            mashing.transform.position = mashWP.position;

            mashProgress.value = mashing.GetComponent<IngredientProperties>().prepPercent;
            progressBar.SetActive(true);
        }
    }

    public GameObject Collect()
    {
        if(mashing != null)
        {
            GameObject mashed = mashing;
            mashing = null;

            progressBar.SetActive(false);
            return mashed;
        }

        return null;
    }

    public bool CheckCollect()
    {
        if (mashing != null)
        {
            return true;
        }

        return false;
    }

    public void PoundDone()
    {
        anim.SetBool("isPounding", false);
    }
}
