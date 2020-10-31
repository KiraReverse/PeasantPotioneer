using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    public List<Recipe> allRecipies;
    public GameObject junkPotion;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public GameObject GetPotion(IngredientProperties[] props)
    {
        List<Recipe> result = new List<Recipe>();

        for (int i = 0; i < props.Length; ++i)
        {
            if(i == 0)
            {
                result = AddRecipeToList(props[i], allRecipies);
            }
            else
            {
                result = AddRecipeToList(props[i], result);
            }
        }

        if(result.Count > 0)
        {
            return result[0].potion;
        }
        else
        {
            return junkPotion;
        }
    }
   

    private List<Recipe> AddRecipeToList(IngredientProperties i, List<Recipe> rInput)
    {
        List<Recipe> result = new List<Recipe>();

        foreach(Recipe r in rInput)
        {
            foreach (FoodObject fo in r.recipe)
            {
                if (fo.ingredientName == i.ingredientName && fo.ingredientState == i.prepState)
                {
                    result.Add(r);
                }
            }
        }

        return result;
    }

}
