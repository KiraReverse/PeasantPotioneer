using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransactionManager : MonoBehaviour
{
    public Text goldDisplay;

    public int hpPrice;
    public int mpPrice;
    public int atkPrice;
    public int spdPrice;
    public int defPrice;


    public int eyePrice;
    public int rootPrice;
    public int shroomPrice;
    public int pawPrice;
    public int crystalPrice;

    int hpCounter = 0;
    int mpCounter = 0;
    int atkCounter = 0;
    int spdCounter = 0;
    int defCounter = 0;

    public Text[] hp;
    public Text[] mp;
    public Text[] atk;
    public Text[] spd;
    public Text[] def;

    public Text qualityText;
    public Text totalText;



    [SerializeField]
    int gold = 200;
    int qualityBonus = 0;

    // Start is called before the first frame update
    void Start()
    {
        UpdateGold();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SellPotion(GameObject potion)
    {
        ProductProperties potProp = potion.GetComponent<ProductProperties>();
        float multiplier = potion.GetComponent<ProductProperties>().GetQuality()/1000f;

        float qualityCurrent = 0;
        int saleGold = 0;
        switch(potProp.productName)
        {
            case "Health Potion":
                saleGold = hpPrice;
                qualityCurrent += hpPrice * multiplier;
                ++hpCounter;
                break;

            case "Mana Potion":
                saleGold = mpPrice;
                qualityCurrent += mpPrice * multiplier;
                ++mpCounter;
                break;

            case "Attack Potion":
                saleGold = atkPrice;
                qualityCurrent += atkPrice * multiplier;
                ++atkCounter;
                break;

            case "Speed Potion":
                saleGold = spdPrice;
                qualityCurrent += spdPrice * multiplier;
                ++spdCounter;
                break;

            case "Defense Potion":
                saleGold = defPrice;
                qualityCurrent += defPrice * multiplier;
                ++defCounter;
                break;

            default:
                Debug.Log("hmm i sold something weird");
                break;
        }

        qualityBonus += (int)qualityCurrent;
        gold += (int)qualityCurrent + saleGold;

        UpdateGold();
    }

    public void BuyIngredient(string ingName)
    {
        switch (ingName)
        {
            case "Eyeball":
                gold -= eyePrice;
                break;

            case "Shroom":
                gold -= shroomPrice;
                break;

            case "Crystal":
                gold -= crystalPrice;
                break;

            case "Catpaw":
                gold -= pawPrice;
                break;

            case "Root":
                gold -= rootPrice;
                break;

            default:
                Debug.Log(ingName);
                break;
        }

        UpdateGold();
    }

    public void UpdateOverview()
    {
        hp[0].text = "x" + hpCounter;
        hp[1].text = (hpPrice * hpCounter).ToString();

        mp[0].text = "x" + mpCounter;
        mp[1].text = (mpPrice * mpCounter).ToString();

        atk[0].text = "x" + atkCounter;
        atk[1].text = (atkPrice * atkCounter).ToString();

        spd[0].text = "x" + spdCounter;
        spd[1].text = (spdPrice * spdCounter).ToString();

        def[0].text = "x" + defCounter;
        def[1].text = (defPrice * defCounter).ToString();

        qualityText.text = qualityBonus.ToString();

        totalText.text = gold.ToString();
    }

    public void UpdateGold()
    {
        goldDisplay.text = gold.ToString();

    }
}
