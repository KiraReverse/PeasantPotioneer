using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    public float customerRotationTime;
    //public float resetTime;
    public List<GameObject> customers;

    public GameManager gameManager;

    [SerializeField]
    GameObject[] queue = new GameObject[3];

    //Customer UI Stuff
    
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator CustomerEntry()
    {
        if(!gameManager.IsShopOpen())
        {
            yield break;
        }

        

        int ran = Random.Range(0, customers.Count);

        for (int i = 0; i < queue.Length; ++i)
        {
            if (queue[i] == null)
            {
                queue[i] = customers[ran];
                queue[i].GetComponent<CustomerController>().MoveTo(i);
                break;
            }
        }
        customers.RemoveAt(ran);


        Invoke("CustomerEnter", customerRotationTime);
    }

    public void CustomerEnter()
    {
        StartCoroutine(CustomerEntry());
    }

    public void CustomerExit(GameObject customer)
    {
        for(int i = 0; i<queue.Length; ++i)
        {
            if(queue[i] == customer)
            {
                queue[i] = null;
            }
        }
    }

    public void ReadyToShop(GameObject customer)
    {
        customers.Add(customer);
    }


}
