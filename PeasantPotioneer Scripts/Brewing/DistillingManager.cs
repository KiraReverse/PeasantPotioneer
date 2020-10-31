using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistillingManager : MonoBehaviour
{
    public float distillPercent;
    public float distillSpeed;

    public Transform distillWp;
    public Transform flaskWP;

    public GameObject progressBar;
    public Slider distillProgress;

    public GameObject[] distillFlasks;

    public Animator anim;

    GameObject distilling = null;
    bool canDistill = false;
    bool isComplete = false;

    public AudioSource sfx;
    public AudioSource completeSfx;

    // Start is called before the first frame update
    void Start()
    {
        progressBar.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if(canDistill)
        {
            if (Input.GetKey(KeyCode.E))
            {
                distillPercent += Time.deltaTime* distillSpeed;

                

                if (!sfx.isPlaying && !isComplete)
                {
                    sfx.Play();
                }

                if (distillPercent >= distillProgress.maxValue)
                {
                    distillProgress.value = distillProgress.maxValue;

                    if(!isComplete)
                    {
                        anim.SetBool("isDistilling", false);
                        anim.SetBool("isEnded", true);
                        completeSfx.Play();
                        sfx.Stop();
                    }

                    isComplete = true;

                  
                }

                else
                {
                    if(!anim.GetBool("isDistilling"))
                    {
                        anim.SetBool("isDistilling", true);
                    }
                }

            }

            if (Input.GetKeyUp(KeyCode.E))
            {
                if (distillPercent < distillProgress.maxValue)
                {
                    anim.SetBool("isDistilling", false);
                    anim.SetBool("isEnded", true);
                    distillPercent = 0f;
                }

                if(sfx.isPlaying && !isComplete)
                {
                    sfx.Stop();
                }
            }

            distillProgress.value = distillPercent;
        }
    }

    public bool CheckAdd(GameObject ingredient)
    {
        if (ingredient.GetComponent<IngredientProperties>() == null || distilling != null)
        {
            return false;
        }

        IngredientProperties iP = ingredient.GetComponent<IngredientProperties>();

        if (iP.prepState != State.raw)
        {
            return false;
        }

        return true;
    }

    public void AddIngredient(GameObject ingredient)
    {

        IngredientProperties iP = ingredient.GetComponent<IngredientProperties>();

        if (distilling == null)
        {
            isComplete = false;
            distilling = ingredient;
            distilling.transform.parent = distillWp;
            distilling.transform.localPosition = Vector3.zero;
            distilling.GetComponent<Rigidbody>().isKinematic = true;
            
            progressBar.SetActive(true);
        }

    }


    public GameObject Collect()
    {
        if (distillPercent >= 100)
        {
          

            GameObject ingredient = Instantiate(GetFlask(distilling.GetComponent<IngredientProperties>().ingredientName), flaskWP);
            Empty();
            return ingredient;
        }

        return null;
    }

    public bool CheckCollect()
    {
        if (distillPercent >= 100)
        {
            return true;
        }

        return false;
    }

    public void Empty()
    {
        Destroy(distilling);
        distillPercent = 0f;
        progressBar.SetActive(false);
    }

    GameObject GetFlask(string name)
    {
        switch (name)
        {
            case "Eyeball":
                return distillFlasks[0];
            case "Shroom":
                return distillFlasks[1];
            case "Crystal":
                return distillFlasks[2];
            case "Root":
                return distillFlasks[3];
            case "Catpaw":
                return distillFlasks[4];
            default:
                return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            canDistill = true;
            if (!anim.GetBool("isReady"))
            {
                anim.SetBool("isReady", true);
                anim.SetBool("isEnded", false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            canDistill = false;

            if (distillPercent < 100)
            {
                distillPercent = 0;
                distillProgress.value = distillPercent;
            }

            if (anim.GetBool("isReady"))
            {
                anim.SetBool("isReady", false);
            }
            anim.SetBool("isDistilling", false);
            anim.SetBool("isEnded", false);

        }
    }

    public void DistillEnd()
    {
        anim.SetBool("isEnded", false);
    }
}
