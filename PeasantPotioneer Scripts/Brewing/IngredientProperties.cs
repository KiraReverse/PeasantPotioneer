using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    raw,
    distilling,
    distilled,
    mashing,
    mashed
}

public class IngredientProperties : MonoBehaviour
{
    public string ingredientName;
    public State prepState = State.raw;
    public float prepPercent = 0f;

}
