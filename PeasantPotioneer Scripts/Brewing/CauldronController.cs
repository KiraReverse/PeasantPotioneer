using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CauldronController : MonoBehaviour
{
    [SerializeField]
    IngredientProperties[] ingredientList = new IngredientProperties[3];

    public GameObject barObj;
    public GameObject heatbarObj;
    public Transform potionStore;
    public Transform ingredientStore;
    public GameObject[] flames;
    public GameObject explosion;
    public CustomRenderTexture[] potRef;
    public RawImage[] potStorage;
    public Image[] potbg;

    public Transform explodeWP;

    public Color overheated;
    Color cooled;
    Renderer mesh;
    IEnumerator overheatFunc;
    IEnumerator coolingFunc;
    bool overheating = false;
    bool cooling = false;

    Slider progressBar;
    public int cookTime;

    Slider overheatBar;
    public float heatMultiplier;
    public float stirTime;
    public float detonateTimer;

    public Animator anim;

    [SerializeField]
    float heatValue = 0f;

    [SerializeField]
    float cookValue = 0f;
    bool cooking;
    

    //0 - Health
    //1 - Mana
    //2 - Attack
    //3 - Speed
    //4 - Defence
    public GameObject[] potions;

    RecipeManager rM;

    public AudioSource bubblingSfx;
    public AudioSource overheatSfx;
    public AudioSource coolingSfx;
    public AudioSource ding;
    public AudioSource explosionSfx;
    public AudioSource flameSfx;
    public AudioSource addIngredientSfx;
    public AudioSource extinguishSfx;

    bool isComplete = false;
    // Start is called before the first frame update
    void Start()
    {
        barObj.SetActive(false);
        heatbarObj.SetActive(false);
        progressBar = barObj.GetComponent<Slider>();
        overheatBar = heatbarObj.GetComponent<Slider>();
        cooking = false;

        progressBar.maxValue = cookTime;

        rM = GetComponent<RecipeManager>();
        mesh = GetComponent<Renderer>();
        cooled = mesh.material.color;
        overheatFunc = Overheat();
        coolingFunc = Cooled();

        SwitchFlames(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(cooking)
        {
            heatValue += Time.deltaTime * heatMultiplier;
            cookValue += Time.deltaTime * overheatBar.value;

            if(!bubblingSfx.enabled)
            {
                bubblingSfx.enabled = true;
                flameSfx.enabled = true;
            }

            flameSfx.volume = overheatBar.value;
            bubblingSfx.pitch = overheatBar.value;

            if (CountIngredients() == 3 && cookValue >= cookTime * 3 && !isComplete)
            {
                if(!ding.isPlaying)
                {
                    ding.Play();
                }

                isComplete = true;
            }
        }

        else
        {
            if(bubblingSfx.enabled)
            {
                bubblingSfx.enabled = false;
                flameSfx.enabled = false;
            }
        }
        

        if (cookValue > progressBar.maxValue)
        {
            cookValue = progressBar.maxValue;
        }

        if (heatValue > overheatBar.maxValue)
        {
            heatValue = overheatBar.maxValue;
        }

        progressBar.value = cookValue;
        overheatBar.value = heatValue;
        ScaleFlames();

        if ((overheatBar.value >= overheatBar.maxValue) && !cooling)
        {
            if(!overheating)
            {
                overheating = true;
                StartCoroutine(overheatFunc);
            }
        }
    }

    public bool CheckStir()
    {
        if(cooking)
        {
            return true;
        }

        return false;
    }

    public void StirPot()
    {
        if (cooking)
        {

            

            heatValue -= stirTime;

            if(heatValue <0)
            {
                heatValue = 0;
            }

            if(overheating && !cooling)
            {
                cooling = true;
                
                StartCoroutine(coolingFunc);
                Debug.Log("stopped");
            }

            if(!extinguishSfx.isPlaying)
            {
                extinguishSfx.Play();
            }
            anim.SetBool("isStirring", true);
        }
    }

    public void StirEnd()
    {
        anim.SetBool("isStirring", false);
    }

    public bool CheckAdd(GameObject ingredient)
    {
        if (ingredient.GetComponent<IngredientProperties>() == null)
        {
            return false;
        }

        IngredientProperties iP = ingredient.GetComponent<IngredientProperties>();

        if (iP.prepState == State.distilling || iP.prepState == State.mashing || iP.tag == "Product")
        {
            return false;
        }

        if(ingredientList[0] != null && cookValue < progressBar.maxValue)
        {
            return false;
        }

        return true;
    }

    public void AddIngredient(GameObject ingredient)
    {
        if(!addIngredientSfx.isPlaying)
        {
            addIngredientSfx.Play();
        }
        IngredientProperties iP = ingredient.GetComponent<IngredientProperties>();

        for (int i = 0; i<ingredientList.Length  ; ++i)
        {
            if (ingredientList[i] == null)
            {
                ingredient.transform.parent = ingredientStore;
                ingredient.transform.localPosition = Vector3.zero;
                ingredient.GetComponent<Rigidbody>().isKinematic = true;

                ingredientList[i] = iP;
                potStorage[i].texture = GetImage(iP.ingredientName);
                potStorage[i].enabled = true;
                potbg[i].enabled = true;
                UpdateBar(CountIngredients());
                return;
            }
        }
    }


    public GameObject Collect()
    {
        if(CountIngredients() == 3 && cookValue >= cookTime*3)
        {
            GameObject potion = Instantiate(rM.GetPotion(ingredientList), potionStore);
            if (overheating && !cooling)
            {
                cooling = true;
                StopCoroutine(overheatFunc);
                StartCoroutine(coolingFunc);
            }
            Empty();
            return potion;
        }

        return null;
    }

    public bool CheckCollect()
    {


        if (CountIngredients() == 3 && cookValue >= cookTime * 3)
        {
            return true;
        }

        return false;
    }

    public void Empty()
    {
        isComplete = false;
        Array.Clear(ingredientList, 0, ingredientList.Length);

        foreach(Transform t in ingredientStore)
        {
            Destroy(t.gameObject);
        }

        for(int i = 0; i<potStorage.Length; ++i)
        {
            potStorage[i].texture = GetImage("nil");
            potStorage[i].enabled = false;
            potbg[i].enabled = false;
        }

        UpdateBar(CountIngredients());
    }

    int CountIngredients()
    {
        int counter = 0;
        foreach(IngredientProperties s in ingredientList)
        {
            if(s != null)
            {
                counter++;
            }
        }

        return counter;
    }

    void UpdateBar(int counter)
    {
        switch (counter)
        {
            case 1:
                SwitchFlames(true);
                barObj.SetActive(true);
                heatbarObj.SetActive(true);
                cooking = true;
                progressBar.maxValue = cookTime;
                break;
            case 2:
                if(cookTime > progressBar.maxValue)
                {
                    cookValue = progressBar.maxValue;
                }
                progressBar.maxValue = cookTime * 2;
                
                break;
            case 3:
                if (cookTime > progressBar.maxValue)
                {
                    cookValue = progressBar.maxValue;
                }
                progressBar.maxValue = cookTime * 3;
                break;

            default:
                cooking = false;
                progressBar.value = 0;
                overheatBar.value = 0;
                heatValue = 0;
                cookValue = 0;
                barObj.SetActive(false);
                heatbarObj.SetActive(false);
                SwitchFlames(false);
                break;
        }
    }

    void SwitchFlames(bool on)
    {
        foreach (GameObject g in flames)
        {
            g.SetActive(on);
        }
    }

    void ScaleFlames()
    {
        foreach (GameObject g in flames)
        {
            g.transform.localScale = new Vector3(1f, 1f, 1f) * overheatBar.value;
        }
    }

    CustomRenderTexture GetImage(string name)
    {
        switch (name)
        {
            case "Eyeball":
                return potRef[0];
            case "Shroom":
                return potRef[1];
            case "Crystal":
                return potRef[2];
            case "Root":
                return potRef[3];
            case "Catpaw":
                return potRef[4];
            default:
                return potRef[5];
        }
    }



    IEnumerator Overheat()
    {
        if(anim.GetBool("isStirring"))
        {
            anim.SetBool("isStirring", false);
        }

        anim.SetBool("isOverheating", true);

        if(!overheatSfx.isPlaying)
        {
            overheatSfx.Play();
        }
        overheating = true;
        float time = 0;
        while (time < detonateTimer)
        {
            time += Time.deltaTime;
            Debug.Log(time);
            mesh.material.color = Color.Lerp(mesh.material.color, overheated, time / detonateTimer);
            yield return null;
        }
        
        Debug.Log("exploooooooosion");

        anim.SetBool("isOverheating", false);
        GameObject explode = Instantiate(explosion, explodeWP);
        if(!explosionSfx.isPlaying)
        {
            explosionSfx.Play();
        }

        anim.SetBool("isFlipping", true);
        Destroy(explode, 2f);
        Empty();
        cooling = true;
        yield return new WaitForSeconds(1f);
        anim.SetBool("isFlipping", false);
        StartCoroutine(coolingFunc);
    }

    IEnumerator Cooled()
    {
        anim.SetBool("isOverheating", false);

        if (overheatSfx.isPlaying)
        {
            overheatSfx.Stop();
        }

        if(!coolingSfx.isPlaying)
        {
            coolingSfx.Play();
        }

        StopCoroutine(overheatFunc);
        overheatFunc = Overheat();

        float time = 0;
        while (time < 1)
        {
            time += Time.deltaTime;

            mesh.material.color = Color.Lerp(mesh.material.color, cooled, time / 1f);
            yield return null;
        }

        overheating = false;
        cooling = false;

        coolingFunc = Cooled();
        Debug.Log("cooled");
    }

    
}
