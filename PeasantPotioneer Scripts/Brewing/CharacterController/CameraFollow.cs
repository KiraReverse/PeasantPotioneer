using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform dolly;
    public Transform head;
    public Transform look;

    Vector3 camOffset;
    public Vector3 standCamOffset = new Vector3(0f, 0.7f, 0f);
    public Vector3 fallCamOffset = Vector3.zero;
    float distance;
    public float standDistance = 2f;
    public float fallDistance = 2.5f;
    float height;
    public float standHeight = 2f;
    public float fallHeight = 2.5f;
    
    public float rotDamp;
    public float standHeightDamp = 0.5f;
    public float fallHeightDamp = 5f;
    float heightDamp;

    public float standCamSpeed = 1f;
    public float fallCamSpeed = 5f;
    float camSpeed;

    public float shakeMagnitude;

    //Transform lookTarget;

    public ClumsyController clumCon;

    RaycastHit hit;
    Collider blocking;
    public LayerMask layerM;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (clumCon.GetFallen())
        {
            heightDamp = fallHeightDamp;
            camSpeed = fallCamSpeed;
            height = fallHeight;
            distance = fallDistance;
            camOffset = fallCamOffset;
            
        }

        else
        {
            heightDamp = standHeightDamp;
            camSpeed = standCamSpeed;
            height = standHeight;
            distance = standDistance;
            camOffset = standCamOffset;
        }

        var targetRotation = Quaternion.LookRotation((head.transform.position + camOffset) - transform.position);
        
        // Smoothly rotate towards the target point.
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotDamp * Time.deltaTime);

        
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(head.rotation.z, head.rotation.x, head.rotation.y), rotDamp * Time.deltaTime);

        float wantedHeight = Mathf.Lerp(transform.position.y, head.position.y + height, heightDamp * Time.deltaTime);

        transform.position = dolly.position;

        Quaternion heightRot = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        transform.position -= heightRot * Vector3.forward * distance;

        transform.position = new Vector3(transform.position.x, wantedHeight, transform.position.z);

        Vector3 dir = Vector3.Normalize(look.position - transform.position);

        float dist = Vector3.Distance(head.position, transform.position) - 0.1f;

        if (Physics.Linecast(transform.position, transform.position + (dir * dist), out hit, layerM))
        {
            Debug.DrawLine(transform.position, transform.position + (dir * dist), Color.red);

            if (hit.collider != null)
            {
                blocking = hit.collider;

                Color temp = blocking.gameObject.GetComponent<Renderer>().material.color;
                temp.a = 0.4f;

                blocking.gameObject.GetComponent<Renderer>().material.color = temp;
            }

        }
        else
        {
            Debug.DrawLine(transform.position, transform.position + (dir * dist), Color.green);

            if (blocking != null)
            {
                Color temp = blocking.gameObject.GetComponent<Renderer>().material.color;
                temp.a = 1f;

                blocking.gameObject.GetComponent<Renderer>().material.color = temp;

                blocking = null;

            }
        }
    }

    public void ShakeCam()
    {
        StartCoroutine(Shake(1f, shakeMagnitude));
    }

    IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 orignalPosition = transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.position = new Vector3((transform.position.x +x), (transform.position.y + y), transform.position.z);
            elapsed += Time.deltaTime;
            yield return 0;
        }
        transform.position = orignalPosition;
    }
}
