using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OldGameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Text> startMessage;
    public List<Text> endMessage;
    public float typeTime;

    public Text keyPress;

    public CustomerManager cusManager;


    public UnityStandardAssets.Characters.FirstPerson.FirstPersonController fps;
    public Image blackout;

    public AudioSource sfx;

    public float gameTime = 300f;
    public Transform hand;

    IEnumerator typewriter;

    bool cutscene = false;

    bool isEnded = false;

    int potionCount = 0;


    // Start is called before the first frame update
    void Start()
    {

        foreach (Text t in startMessage)
        {
            if (t.enabled)
            {
                t.enabled = false;
            }
        }
        cutscene = false;
        StartCoroutine(StartGame());

        if (sfx.isPlaying)
        {
            sfx.Stop();
        }

        keyPress.enabled = false;

        //typewriter = TypeWriter();
        //StartCoroutine(typewriter);
    }

    // Update is called once per frame
    void Update()
    {
        if (cutscene)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                StopCoroutine(typewriter);

                foreach (Text t in startMessage)
                {
                    if (t.enabled)
                    {
                        t.enabled = false;
                    }
                }
                cutscene = false;
                StartCoroutine(StartGame());

                if (sfx.isPlaying)
                {
                    sfx.Stop();
                }

                keyPress.enabled = false;
            }
        }

        if (!cutscene && !isEnded)
        {

        }

        if (Input.GetKeyDown(KeyCode.F1) && !isEnded)
        {
            StartCoroutine(EndWriter());
        }
    }

    public void IncreasePotCount()
    {
        potionCount += 1;
    }

    IEnumerator TypeWriter()
    {
        keyPress.enabled = true;
        cutscene = true;

        fps.enabled = false;

        string s1 = startMessage[0].text;
        startMessage[0].text = "";

        yield return new WaitForSeconds(0.5f);

        sfx.Play();

        foreach (char c in s1)
        {
            if (Input.anyKeyDown)
            {
                startMessage[0].text = s1;
                yield return new WaitForSeconds(0.1f);
                break;
            }

            startMessage[0].text += c;
            yield return new WaitForSeconds(typeTime);


        }

        sfx.Stop();

        yield return new WaitUntil(() => Input.anyKeyDown);

        startMessage[0].enabled = false;

        s1 = startMessage[1].text;
        startMessage[1].text = "";


        startMessage[1].enabled = true;
        yield return new WaitForSeconds(0.5f);

        sfx.Play();

        foreach (char c in s1)
        {
            if (Input.anyKeyDown)
            {
                startMessage[1].text = s1;
                yield return new WaitForSeconds(0.1f);
                break;
            }

            startMessage[1].text += c;
            yield return new WaitForSeconds(typeTime);


        }

        sfx.Stop();

        yield return new WaitUntil(() => Input.anyKeyDown);

        startMessage[1].enabled = false;

        s1 = startMessage[2].text;
        startMessage[2].text = "";

        startMessage[2].enabled = true;
        yield return new WaitForSeconds(0.5f);

        sfx.Play();

        foreach (char c in s1)
        {
            if (Input.anyKeyDown)
            {
                startMessage[2].text = s1;
                yield return new WaitForSeconds(0.1f);
                break;
            }
            startMessage[2].text += c;
            yield return new WaitForSeconds(typeTime);


        }

        sfx.Stop();

        yield return new WaitUntil(() => Input.anyKeyDown);

        startMessage[2].enabled = false;

        s1 = startMessage[3].text;
        startMessage[3].text = "";

        startMessage[3].enabled = true;
        yield return new WaitForSeconds(0.5f);

        sfx.Play();

        foreach (char c in s1)
        {
            if (Input.anyKeyDown)
            {
                startMessage[3].text = s1;
                yield return new WaitForSeconds(0.1f);
                break;
            }

            startMessage[3].text += c;
            yield return new WaitForSeconds(typeTime);


        }

        sfx.Stop();

        yield return new WaitUntil(() => Input.anyKeyDown);

        startMessage[3].enabled = false;


        StartCoroutine(FadeTo(blackout, 0f, 1f));
        fps.enabled = true;

        cutscene = false;
        keyPress.enabled = false;
        StartCoroutine(StartGame());


    }

    IEnumerator StartGame()
    {
        yield return null;
        StartCoroutine(FadeTo(blackout, 0f, 1f));
        fps.enabled = true;
        //cusManager.CustomerEnter();
        StartCoroutine(Rotate(gameTime));
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


        StartCoroutine(EndWriter());
    }

    IEnumerator EndWriter()
    {

        isEnded = true;
        fps.enabled = false;
        StartCoroutine(FadeTo(blackout, 1f, 1f));

        yield return new WaitForSeconds(1f);
        keyPress.enabled = true;
        string s = endMessage[0].text;
        endMessage[0].text = "";

        endMessage[0].enabled = true;

        sfx.Play();

        foreach (char c in s)
        {
            if (Input.anyKeyDown)
            {
                endMessage[0].text = s;
                yield return new WaitForSeconds(0.1f);
                break;
            }

            endMessage[0].text += c;
            yield return new WaitForSeconds(typeTime);


        }

        sfx.Stop();

        yield return new WaitUntil(() => Input.anyKeyDown);

        endMessage[0].enabled = false;

        s = endMessage[1].text;
        endMessage[1].text = "";

        endMessage[1].enabled = true;

        sfx.Play();

        foreach (char c in s)
        {
            if (Input.anyKeyDown)
            {
                endMessage[1].text = s;
                yield return new WaitForSeconds(0.1f);
                break;
            }

            endMessage[1].text += c;
            yield return new WaitForSeconds(typeTime);


        }

        sfx.Stop();

        s = "..." + potionCount + " potions today.\"";
        endMessage[2].text = "";

        endMessage[2].enabled = true;

        sfx.Play();

        foreach (char c in s)
        {
            if (Input.anyKeyDown)
            {
                endMessage[2].text = s;
                yield return new WaitForSeconds(0.1f);
                break;
            }

            endMessage[2].text += c;
            yield return new WaitForSeconds(typeTime);


        }

        sfx.Stop();

        yield return new WaitUntil(() => Input.anyKeyDown);

        endMessage[1].enabled = false;
        endMessage[2].enabled = false;

        s = endMessage[3].text;
        endMessage[3].text = "";

        endMessage[3].enabled = true;
        yield return new WaitForSeconds(0.5f);

        sfx.Play();

        foreach (char c in s)
        {
            if (Input.anyKeyDown)
            {
                endMessage[3].text = s;
                yield return new WaitForSeconds(0.1f);
                break;
            }

            endMessage[3].text += c;
            yield return new WaitForSeconds(typeTime);


        }

        sfx.Stop();

        yield return new WaitUntil(() => Input.anyKeyDown);
        keyPress.enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("MainMenu");
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
}
