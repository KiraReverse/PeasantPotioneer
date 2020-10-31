using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickupController : MonoBehaviour
{
    public float range;
    public LayerMask layerM;
    public LayerMask lM;

    Camera cam;
    

    public float throwForce;
    public GameObject held;
    GameObject holding;

    GameObject looked = null;

    public Image promptBg;
    public Text cauldronWarning;
    public Text givingWarning;

    bool isHolding;
    bool isWarned = false;

   TransactionManager tscManager;

    public AudioSource pickupSfx;
    public AudioSource placeSfx;
    public AudioSource throwSfx;
    public AudioSource paperSfx;

    public GameObject[] prompts;
    /// <summary>
    /// 0 - Pickup
    /// 1 - Give
    /// 2 - Place
    /// 3 - Throw
    /// 4 - Empty
    /// 5 - Pound
    /// 6 - Distill
    /// 7 - Flip
    /// 8 - Stir
    /// </summary>
    public GameObject[] keys;
    /// <summary>
    /// 0 - LMB
    /// 1 - RMB
    /// 2 - E
    /// 3 - Q
    /// </summary>

    bool canCast = true;
 

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        tscManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<TransactionManager>();
        isHolding = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        HitScan();
       
        if (Input.GetKeyDown(KeyCode.Mouse1) && holding != null)
        {
            Yeet();
        }

    }

    public void HitScan()
    {
        RaycastHit hit;
        GameObject currLook;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range) && canCast)
        {
            Debug.DrawRay(cam.transform.position, cam.transform.forward * range, Color.red);
            print("I'm looking at " + hit.transform.name);

            

            if (hit.collider.gameObject.tag == "Source")
            {
                if (Input.GetKeyDown(KeyCode.Mouse0) && !isHolding)
                {
                    if(!pickupSfx.isPlaying)
                    {
                        pickupSfx.Play();
                    }
                    isHolding = true;
                    hit.collider.gameObject.GetComponent<Outline>().enabled = false;

                    holding = Instantiate(hit.collider.gameObject, held.transform);


                    if (holding.GetComponent<IngredientProperties>() != null)
                    {
                        holding.transform.tag = "Ingredient";
                    }
                    else
                    {
                        holding.transform.tag = "Product";
                        holding.transform.GetChild(0).gameObject.SetActive(true);
                    }
                    holding.transform.localPosition = new Vector3(0, 0, 0);
                    //holding.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f); 
                    //set to ignore raycast layer
                    holding.layer = 2;


                    if (holding.GetComponent<IngredientProperties>() == null)
                    {
                        Debug.Log("This is a potion!");
                    }

                    else
                    {
                        tscManager.BuyIngredient(holding.GetComponent<IngredientProperties>().ingredientName);
                    }

                    //LMB PICK UP OFF
                    keys[0].SetActive(false);
                    prompts[0].SetActive(false);
                }

                if (!isHolding)
                {
                    //LMB PICK UP ON
                    keys[0].SetActive(true);
                    prompts[0].SetActive(true);
                }
            }

            else if ((hit.collider.gameObject.tag == "Ingredient" || hit.collider.gameObject.tag == "Product") && !isHolding)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0) && holding == null)
                {
                    Hold(hit.collider.gameObject);
                }

                //LMB PICK UP ON
                keys[0].SetActive(true);
                prompts[0].SetActive(true);
            }

            else if (hit.collider.gameObject.tag == "Cauldron" )
            {
                CauldronController cauCon = hit.collider.gameObject.GetComponent<CauldronController>();

                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                   
                    if (isHolding)
                    {
                        if (holding != null && cauCon.CheckAdd(holding))
                        {
                            if (!placeSfx.isPlaying)
                            {
                                placeSfx.Play();
                            }
                            cauCon.AddIngredient(holding);
                            isHolding = false;
                            holding = null;

                            //LMB PLACE OFF
                            keys[0].SetActive(false);
                            prompts[2].SetActive(false);

                            //RMB THROW OFF
                            keys[1].SetActive(false);
                            prompts[3].SetActive(false);
                        }

                        else
                        {
                            if (!isWarned)
                            {
                                StartCoroutine(Prompt(promptBg, cauldronWarning));
                            }
                        }
                    }

                    else
                    {
                        if (cauCon.CheckCollect())
                        {
                            Hold(cauCon.Collect());
                            holding.transform.GetChild(0).gameObject.SetActive(true);
                            //holding.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

                            //LMB PICK UP OFF
                            keys[0].SetActive(false);
                            prompts[0].SetActive(false);
                        }
                    }
                } 

                if (Input.GetKeyDown(KeyCode.E))
                {
                    cauCon.StirPot();
                }

                if (Input.GetKeyDown(KeyCode.Q) && !cauCon.CheckCollect())
                {
                    cauCon.Empty();
                }


                if (isHolding)
                {
                    if (cauCon.CheckAdd(holding))
                    {
                        //LMB PLACE ON
                        keys[0].SetActive(true);
                        prompts[2].SetActive(true);
                    }
                }

                else
                {
                    if (cauCon.CheckCollect())
                    {
                        //LMB PICK UP ON
                        keys[0].SetActive(true);
                        prompts[0].SetActive(true);
                    }
                }

                //Q EMPTY ON
                keys[3].SetActive(true);
                prompts[4].SetActive(true);

                //E Stir ON 
                keys[2].SetActive(true);
                prompts[8].SetActive(true);
            }

            else if (hit.collider.gameObject.tag == "Pestle")
            {
                MashingController mashCon = hit.collider.gameObject.GetComponentInParent<MashingController>();

                if (Input.GetKeyDown(KeyCode.Mouse0))
                {

                    if (isHolding)
                    {
                        if (holding != null && mashCon.CheckPlace())
                        {
                            if (!placeSfx.isPlaying)
                            {
                                placeSfx.Play();
                            }

                            mashCon.Place(holding);
                            isHolding = false;
                            holding = null;

                            //LMB PLACE OFF
                            keys[0].SetActive(false);
                            prompts[2].SetActive(false);

                            //E POUND ON 
                            keys[2].SetActive(true);
                            prompts[5].SetActive(true);

                            //RMB THROW OFF
                            keys[1].SetActive(false);
                            prompts[3].SetActive(false);
                        }
                    }

                    else
                    {
                        if (mashCon.CheckCollect())
                        {
                            Hold(mashCon.Collect());
                            //LMB PICK UP OFF
                            keys[0].SetActive(false);
                            prompts[0].SetActive(false);

                            //E POUND OFF 
                            keys[2].SetActive(false);
                            prompts[5].SetActive(false);
                        }
                    }
                }

                if (isHolding)
                {
                    if (holding != null && mashCon.CheckPlace())
                    {
                        //LMB PLACE ON
                        keys[0].SetActive(true);
                        prompts[2].SetActive(true);
                    }

                }

                else
                {
                    if (mashCon.CheckCollect())
                    {
                        //LMB PICK UP ON
                        keys[0].SetActive(true);
                        prompts[0].SetActive(true);
                    }

                    if(!mashCon.CheckPlace())
                    {
                        //E POUND ON 
                        keys[2].SetActive(true);
                        prompts[5].SetActive(true);
                    }
                }
            }

            else if (hit.collider.gameObject.tag == "Filter")
            {
                DistillingManager disCon = hit.collider.gameObject.GetComponentInParent<DistillingManager>();

                if (Input.GetKeyDown(KeyCode.Mouse0))
                {

                    if (isHolding)
                    {
                        if (holding != null && disCon.CheckAdd(holding))
                        {
                            if (!placeSfx.isPlaying)
                            {
                                placeSfx.Play();
                            }

                            disCon.AddIngredient(holding);
                            isHolding = false;
                            holding = null;

                            //LMB PLACE OFF
                            keys[0].SetActive(false);
                            prompts[2].SetActive(false);

                            //RMB THROW OFF
                            keys[1].SetActive(false);
                            prompts[3].SetActive(false);

                            //E DISTILL ON
                            keys[2].SetActive(true);
                            prompts[6].SetActive(true);
                        }
                    }

                    else
                    {
                        if (disCon.CheckCollect())
                        {
                            Hold(disCon.Collect());
                            //holding.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

                            //LMB PICK UP ON
                            keys[0].SetActive(true);
                            prompts[0].SetActive(true);


                        }
                    }
                }

                if (isHolding)
                {
                    if (disCon.CheckAdd(holding))
                    {
                        //LMB PLACE ON
                        keys[0].SetActive(true);
                        prompts[2].SetActive(true);
                    }
                }

                else
                {
                    if (disCon.CheckCollect())
                    {
                        //LMB PICK UP ON
                        keys[0].SetActive(true);
                        prompts[0].SetActive(true);
                    }
                }

            }

            else if(hit.collider.gameObject.tag == "Recipe")
            {
                RecipeController recCon = hit.collider.gameObject.GetComponent<RecipeController>();
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    if(!paperSfx.isPlaying)
                    {
                        paperSfx.Play();
                    }
                    recCon.FlipPage();
                }

                //LMB FLIP ON
                keys[0].SetActive(true);
                prompts[7].SetActive(true);
            }

            else if(hit.collider.gameObject.tag == "Customer")
            {
                CustomerController cusCon = hit.collider.gameObject.GetComponentInParent<CustomerController>();

                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    if (isHolding)
                    {
                        if (cusCon.CheckOrder(holding))
                        {
                            cusCon.OrderComplete(holding);

                            Destroy(holding);
                            holding = null;
                            isHolding = false;

                            //LMB GIVE OFF 
                            keys[0].SetActive(false);
                            prompts[1].SetActive(false);
                        }

                        else
                        {
                            if (!isWarned)
                            {
                                StartCoroutine(Prompt(promptBg, givingWarning));
                            }
                        }
                    }

                    else
                    {
                        cusCon.SpeakToCustomer();
                    }
                }

                

                if(isHolding)
                {
                    //LMB GIVE ON
                    keys[0].SetActive(true);
                    prompts[1].SetActive(true);
                }
            }

            else if(hit.collider.gameObject.tag == "Shelf")
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    ShelfController shelfCon = hit.collider.gameObject.GetComponent<ShelfController>();

                    if (isHolding)
                    {
                        if (shelfCon.CheckPlace())
                        {
                            shelfCon.Place(holding, hit.point);
                            isHolding = false;
                            holding = null;
                        }
                    }

                    else
                    {
                        if(shelfCon.CheckGrab())
                        {
                            Hold(shelfCon.GrabItem(hit.point));
                        }
                    }
                }
            }

            else if(hit.collider.gameObject.tag == "Newspaper")
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    NewspaperController paperCon = hit.collider.gameObject.GetComponent<NewspaperController>();

                    if(isHolding)
                    {

                    }

                    else
                    {
                        paperCon.OpenPaper();
                    }
                }
            }


            if (isHolding)
            {
                //RMB THROW ON
                keys[1].SetActive(true);
                prompts[3].SetActive(true);
            }




            currLook = hit.collider.gameObject;
            if (currLook == looked)
            {
                return;
            }

            if (currLook && currLook != looked)
            {
                if (looked && looked.GetComponent<Outline>() != null)
                {
                    looked.GetComponent<Outline>().enabled = false;
                }

            }

            if (currLook)
                looked = currLook;
            else
                return;

            if (looked && looked.GetComponent<Outline>() != null)
            {
                looked.GetComponent<Outline>().enabled = true;
            }
        }
        else
        {
            Debug.DrawRay(cam.transform.position, cam.transform.forward * range, Color.green);
            print("I'm looking at nothing!");

            if (looked && looked.GetComponent<Outline>() != null)
            {
                looked.GetComponent<Outline>().enabled = false;
                looked = null;
            }

            foreach (GameObject g in prompts)
            {
                if(isHolding &&g == prompts[3])
                {
                    continue;
                }

                g.SetActive(false);
            }

            foreach(GameObject g in keys)
            {
                if (isHolding && g == keys[1])
                {
                    continue;
                }

                g.SetActive(false);
            }
        }
        
    }

    void Hold(GameObject ingredient)
    {
        if (!pickupSfx.isPlaying)
        {
            pickupSfx.Play();
        }

        isHolding = true;

        holding = ingredient;

        holding.transform.parent = held.transform;

        holding.GetComponent<Rigidbody>().isKinematic = true;
        holding.transform.localPosition = new Vector3(0, 0, 0);
        //holding.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        //set to ignore raycast layer
        holding.layer = 2;
    }

    void Yeet()
    {
        if (!throwSfx.isPlaying)
        {
            throwSfx.Play();
        }

        isHolding = false;

        GameObject temp = holding;
        holding = null;

        //set layer back to 0
        temp.layer = 0;
        temp.transform.SetParent(null);
        temp.GetComponent<Outline>().enabled = false;
        temp.GetComponent<Rigidbody>().isKinematic = false;
        temp.GetComponent<Rigidbody>().AddForce(transform.forward * throwForce, ForceMode.Impulse);

        //RMB THROW OFF
        keys[1].SetActive(false);
        prompts[3].SetActive(false);
    }

    IEnumerator Prompt(Image bg, Text text)
    {

        isWarned = true;

        Color bgColor = bg.color;
        float startbgAlpha = bgColor.a;

        Color textColor = text.color;
        float startTextAlpha = textColor.a;

        float t = 0;

        while (t < 2f)
        {
            t += Time.deltaTime;

            float blend = Mathf.Clamp01(t / 1f);

            bgColor.a = Mathf.Lerp(0f, 0.4f, blend);
            textColor.a = Mathf.Lerp(0f, 1f, blend);

            bg.color = bgColor;
            text.color = textColor;

            yield return null;
        }

        yield return new WaitForSeconds(2f);

        t = 0;

        while (t < 2f)
        {
            t += Time.deltaTime;

            float blend = Mathf.Clamp01(t / 1f);

            bgColor.a = Mathf.Lerp(0.4f, 0f, blend);
            textColor.a = Mathf.Lerp(1f, 0f, blend);

            bg.color = bgColor;
            text.color = textColor;

            yield return null;
        }


        isWarned = false;

    }

    public void SetCast(bool can)
    {
        canCast = can;
    }
}
