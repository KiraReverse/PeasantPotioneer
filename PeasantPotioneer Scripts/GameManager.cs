using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public CustomerManager cusManager;
    public UnityStandardAssets.Characters.FirstPerson.FirstPersonController fps;
    public Image blackout;
    public Transform hand;

    public GameObject gameUI;
    public GameObject overviewUI;

    //in seconds
    public float prepTime;
    public float workTime;

    float time = 0;

    bool isWorking = false;
    bool shopOpen = false;

    TransactionManager tscManager;
    // Start is called before the first frame update
    void Start()
    {
        tscManager = GetComponent<TransactionManager>();
        StartCoroutine(FadeTo(blackout, 0f, 1f));
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            StartWork();
        }

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            EndWork();
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (isWorking)
        {
            time += Time.deltaTime;

            if (time > prepTime && !shopOpen)
            {
                shopOpen = true;
                cusManager.CustomerEnter();
                Debug.Log("day start!");
                time = 0;
            }

            if (time > workTime && shopOpen)
            {
                //end work day

                EndWork();

            }

         
        }
    }



    public bool IsShopOpen()
    {
        return shopOpen;
    }

    IEnumerator Overview()
    {
        StartCoroutine(FadeTo(blackout, 0f, 1f));
        tscManager.UpdateOverview();
        yield return new WaitForSeconds(1f);
        //turn on UI object
        gameUI.SetActive(false);
        overviewUI.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(FadeTo(blackout, 1f, 0f));
        yield return new WaitForSeconds(1f);
        
    }

    public void EndWork()
    {
        Debug.Log("day end!");
        StartCoroutine(Overview());
        shopOpen = false;
        isWorking = false;
        fps.enabled = false;
    }

    //call start work to start a brand new day
    public void StartWork()
    {
        isWorking = true;
        fps.enabled = true;
        StartCoroutine(Rotate(prepTime + workTime));
    }

    IEnumerator FadeTo(Image material, float targetOpacity, float duration)
    {
        yield return new WaitForSeconds(0.5f);
        // Cache the current color of the material, and its initiql opacity.
        Color color = material.color;
        float startOpacity = color.a;

        // Track how many seconds we've been fading.
        float t = 0;

        while (t < duration)
        {
            // Step the fade forward one frame.
            t += Time.deltaTime;
            // Turn the time into an interpolation factor between 0 and 1.
            float blend = Mathf.Clamp01(t / duration);

            // Blend to the corresponding opacity between start & target.
            color.a = Mathf.Lerp(startOpacity, targetOpacity, blend);

            // Apply the resulting color to the material.
            material.color = color;

            // Wait one frame, and repeat.
            yield return null;
        }
    }

    IEnumerator Rotate(float duration)
    {
        float startRotation = hand.eulerAngles.z;
        float endRotation = startRotation + 360.0f;
        float t = 0.0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float zRotation = Mathf.Lerp(startRotation, endRotation, t / duration) % 360.0f;
            hand.eulerAngles = new Vector3(hand.eulerAngles.x, hand.eulerAngles.y, -1 * zRotation);
            yield return null;
        }
    }
}
