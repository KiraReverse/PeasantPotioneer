using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProductProperties : MonoBehaviour
{
    public string productName;

    public float decayDelay = 10;
    public float decayTick;

    public Slider qualitySlider;
    
    [SerializeField]
    int quality = 100;



    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Decay());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetQuality()
    {
        return quality;
    }

    IEnumerator Decay()
    {

        yield return new WaitForSeconds(decayDelay);
        while (quality > 0)
        {
            yield return new WaitForSeconds(decayTick);

            --quality;
            qualitySlider.value = quality;
            yield return null;
        }
    }
}
