using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelfController : MonoBehaviour
{
    public GameObject[] store;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Place(GameObject item, Vector3 pos)
    {
        //for(int i = 0; i<store.Length; ++i)
        //{
        //    if(store[i].transform.childCount == 0)
        //    {
        //        item.transform.parent = store[i].transform;

        //        item.transform.localRotation = Quaternion.identity;
        //        item.transform.parent = store[i].transform;
        //        item.transform.position = store[i].transform.position;
        //        break;

        //    }
           
        //}

        GameObject temp = null;
        float minDist = Mathf.Infinity;

        for (int i = 0; i < store.Length; ++i)
        {
            if (store[i].transform.childCount == 0)
            {
                float dist = Vector3.Distance(pos, store[i].transform.position);
                if (dist < minDist)
                {
                    temp = store[i];
                    minDist = dist;
                }
            }
        }



        item.transform.parent = temp.transform;
        item.transform.localRotation = Quaternion.identity;
        item.transform.parent = temp.transform;
        item.transform.position = temp.transform.position;

    }

    public bool CheckPlace()
    {
        for (int i = 0; i < store.Length; ++i)
        {
            if (store[i].transform.childCount == 0)
            {
                return true;

            }

        }

        return false;
    }
    

    public bool CheckGrab()
    {

        int counter = 0;
        for (int i = 0; i < store.Length; ++i)
        {
            if (store[i].transform.childCount != 0)
            {
                ++counter;
            }

        }

        if(counter == 0)
        {
            return false;
        }

        return true;
    }

    public GameObject GrabItem(Vector3 pos)
    {

        GameObject temp = null;
        float minDist = Mathf.Infinity;

        for (int i = 0; i< store.Length; ++i)
        {
            if (store[i].transform.childCount != 0)
            {
                float dist = Vector3.Distance(pos, store[i].transform.position);

                if (dist < minDist)
                {
                    temp = store[i].transform.GetChild(0).gameObject;
                    minDist = dist;
                }
            }
        }

        return temp;
    }
}
