using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class CustomerController : MonoBehaviour
{
    public int saleMoral;
    public int rejectMoral;

    //public GameObject orderBox;
    //public GameObject[] orderText;

    public Transform[] queueWP;
    public Transform exitWp;

    public CustomerManager cusManager;

    public GameManager gMan;

    NavMeshAgent nav;
    AudioSource sfx;
    TransactionManager tscManager;
    MoralController mControl;
    Animator anim;

    string order;
    bool isBuying = false;
    bool isMoving = true;
    bool isExiting = false;

    //CusCanvas
    public GameObject cusCanvas;
    public GameObject gameUI;
    public Text newOrderText;

    public UnityStandardAssets.Characters.FirstPerson.FirstPersonController fps;
    PickupController puCon;


    // Start is called before the first frame update
    void Start()
    {
        sfx = GetComponent<AudioSource>();
        nav = GetComponent<NavMeshAgent>();
        tscManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<TransactionManager>();
        mControl = GameObject.FindGameObjectWithTag("GameManager").GetComponent<MoralController>();
        anim = GetComponentInChildren<Animator>();
        puCon = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PickupController>();
       
    }

    // Update is called once per frame
    void Update()
    {
        if(isMoving && CheckIfReached() && !isExiting)
        {
            isMoving = false;
            anim.SetBool("isMoving", isMoving);

            if (isBuying)
            {
                StartCoroutine(FaceCounter());
                isExiting = true;
            }
        }

        if(isMoving && CheckIfReached() && isExiting)
        {
            isExiting = false;
            cusManager.ReadyToShop(gameObject);
        }
        
    }

    public bool CheckOrder(GameObject product)
    {
        if (product.tag != "Product")
        {
            return false;
        }

        if(product.GetComponent<ProductProperties>().productName == order)
        {
            return true;
        }

        return false;
    }

    public void OrderDenied()
    {
        //orderBox.SetActive(false);

        EndChat();
        //foreach (GameObject g in orderText)
        //{
        //    g.SetActive(false);
        //}
        Exit();
        cusManager.CustomerExit(gameObject);
        mControl.SetMoral(rejectMoral);
        isBuying = false;
    }

    public void OrderComplete(GameObject product)
    {
        if(product.tag != "Product")
        {
            return;
        }

        if(product.GetComponent<ProductProperties>().productName == order)
        {

            tscManager.SellPotion(product);
            sfx.Play();
            //orderBox.SetActive(false);
            //foreach (GameObject g in orderText)
            //{
            //    g.SetActive(false);
            //}
            mControl.SetMoral(saleMoral);
            Exit();
            cusManager.CustomerExit(gameObject);
            isBuying = false;
           
        }
    }

    bool CheckIfReached()
    {
        if(nav.remainingDistance <= 0.001f)
        {
            return true;
        }
        return false;
    }

    public void Exit()
    {
        isMoving = true;
        anim.SetBool("isMoving", isMoving);

        nav.SetDestination(exitWp.position);
    }
    public void MoveTo(int wpNum)
    {
        nav.SetDestination(queueWP[wpNum].position);
        isMoving = true;
        anim.SetBool("isMoving", isMoving);
        isExiting = false;
        isBuying = true;
    }

    public void Order()
    {
        order = GetOrder();
        switch (order)
        {
            case "Health Potion":
                //orderText[0].SetActive(true);
                newOrderText.text = "Hello, I would like to buy a Health Potion,\nbecause I don't feel so good..";
                saleMoral = 10;
                rejectMoral = -30;
                break;
            case "Mana Potion":
                //OrderText[1].SetActive(true);
                newOrderText.text = "Hello, I would like to buy a Mana Potion,\nbecause I want to practice some magic spells.";
                saleMoral = 0;
                rejectMoral = 0;
                break;
            case "Attack Potion":
                //orderText[2].SetActive(true);
                newOrderText.text = "Hello, I would like to buy an Attack Potion,\nI need to take revenge on some guards.";
                saleMoral = -30;
                rejectMoral = 10;
                break;
            case "Speed Potion":
                //orderText[3].SetActive(true);
                newOrderText.text = "Hello, I would like to buy a Speed Potion,\nI need to run away faster..";
                saleMoral = 10;
                rejectMoral = -10;
                break;
            case "Defense Potion":
                //orderText[4].SetActive(true);
                newOrderText.text = "Hello, I would like to buy a Defense Potion,\nSomeone wants to beat me up..";
                saleMoral = 30;
                rejectMoral = -50;
                break;
        }
        //orderBox.SetActive(true);
    }

    string GetOrder()
    {
        int ran = Random.Range(0, 5);

        switch(ran)
        {
            case 0:
                return "Health Potion";
            case 1:
                return "Mana Potion";
            case 2:
                return "Attack Potion";
            case 3:
                return "Speed Potion";
            case 4:
                return "Defense Potion";
        }

        return null;
    }

    IEnumerator FaceCounter()
    {
        Order();

        while (!IsApproximate(transform.rotation, Quaternion.Euler(0, 180, 0), 0.0000004f))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 180, 0), Time.deltaTime * 4);
            yield return null;
        }


        
    }

    public bool IsApproximate(Quaternion q1, Quaternion q2, float precision)
    {
        return Mathf.Abs(Quaternion.Dot(q1, q2)) >= 1 - precision;
    }


    public void SpeakToCustomer()
    {
        cusCanvas.SetActive(true);
        fps.enabled = false;
        gameUI.SetActive(false);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        puCon.SetCast(false);
    }

    public void EndChat()
    {
        cusCanvas.SetActive(false);
        fps.enabled = true;
        gameUI.SetActive(true);
        Cursor.visible = false;
        puCon.SetCast(true);
        Cursor.lockState = CursorLockMode.Locked;
    }
}
