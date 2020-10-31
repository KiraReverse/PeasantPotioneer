using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoralController : MonoBehaviour
{
    public int neutralMin;
    public int neutralMax;

    public Slider moralSlider;
    private int moral = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMoral(int change)
    {
        moral += change;
        UpdateBar();
    }

    public Moral CheckMoral()
    {
        if(moral < neutralMin)
        {
            return Moral.Negative;
        }

        else if (moral > neutralMax)
        {
            return Moral.Positive;
        }

        return Moral.Neutral;
    }

    
    void UpdateBar()
    {
        moralSlider.value = moral;
    }

    public enum Moral
    {
        Positive,
        Neutral,
        Negative
    }
    
}
