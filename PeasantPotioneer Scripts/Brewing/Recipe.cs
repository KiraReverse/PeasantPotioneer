using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Recipe", menuName = "Recipe/Recipe")]

public class Recipe : ScriptableObject
{
    public List<FoodObject> recipe;
    public GameObject potion;
}
