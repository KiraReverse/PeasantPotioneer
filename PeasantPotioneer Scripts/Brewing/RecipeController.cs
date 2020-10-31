using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeController : MonoBehaviour
{
    public GameObject paperPrefab;

    public List<GameObject> recipes;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FlipPage()
    {
        GameObject flying = Instantiate(paperPrefab, transform);
        flying.transform.localPosition = new Vector3(0f, 1f, 0f);

        for(int i = 0; i< recipes.Count; ++i)
        {
            if(recipes[i].activeSelf)
            {
                recipes[i].SetActive(false);

                int j = i + 1;

                if (j >= recipes.Count)
                {
                    recipes[0].SetActive(true);

                }
                else
                {
                    recipes[i + 1].SetActive(true);
                }

                break;
            }
        }
    }
}
