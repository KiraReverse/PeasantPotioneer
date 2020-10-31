using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewspaperController : MonoBehaviour
{
    public GameObject paperUI;
    public GameObject gameUI;

    public UnityStandardAssets.Characters.FirstPerson.FirstPersonController fps;
    PickupController puCon;

    // Start is called before the first frame update
    void Start()
    {
        puCon = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PickupController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenPaper()
    {
        paperUI.SetActive(true);
        gameUI.SetActive(false);

        fps.enabled = false;
        puCon.SetCast(false);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ClosePaper()
    {
        paperUI.SetActive(false);
        gameUI.SetActive(true);

        fps.enabled = true;
        puCon.SetCast(true);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
