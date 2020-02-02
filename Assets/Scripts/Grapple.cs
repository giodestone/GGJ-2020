using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    [SerializeField]
    float maxDistance;
    float currentDistance;

    Vector3 distanceToHook;

    [SerializeField]
    GameObject player;
    [SerializeField]
    GameObject balloon;
    
    GameObject HookHolder;

    [SerializeField]
    float hookTravelSpeed;
    [SerializeField]
    float balloonMoveSpeed;
    bool fired = false;

    //Add in area around balloon where the grapple can be used where it will travel over a certain distnace and check if it comes in contact with an object via raycasting
    //Once it hits an object - turn off IsKinetatic (so no other forces affect it) and pull the balloon towards that point.
    //Once at a certain distance from the point, turn IsKinematic back on and return hook

    // Start is called before the first frame update
    void Start()
    {
        if (this.tag == "HookHold")
        {
            this.GetComponent<Rigidbody>().position = balloon.transform.position + new Vector3(1.0f, 1.0f, 0.0f);
        }

        HookHolder = GameObject.Find("HookHolder");
    }

    // Update is called once per frame
    void Update()
    {
       if (fired == false)
        {
            this.transform.position = HookHolder.transform.position;
        }
    }

    void FireHook()
    {
        if (this.tag == "Hook")
        {
            this.transform.Translate(Vector3.forward * Time.deltaTime * hookTravelSpeed);
            currentDistance = Vector3.Distance(this.transform.position, player.transform.position);

            if (currentDistance >= maxDistance)
            {
                ReturnHook();
            }
        }
    }

    void ReturnHook()
    {
        if (this.tag == "Hook")
        {
            this.transform.position = HookHolder.transform.position;
            fired = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (Input.GetMouseButton(0))
            {
                FireHook();
                fired = true;
            }
        }

        if(other.tag == "Cloud")
        {
            balloon.GetComponent<Rigidbody>().isKinematic = false;
            distanceToHook.x = this.transform.position.x - player.transform.position.x;
            distanceToHook.y = this.transform.position.y - player.transform.position.y;
            distanceToHook.z = this.transform.position.z - player.transform.position.z;

            //convert current distance to vector3 somehow and put in place of currentDistance
            balloon.GetComponent<Rigidbody>().MovePosition(distanceToHook * Time.deltaTime * balloonMoveSpeed);

            if (distanceToHook.x < 1 || distanceToHook.y < 1 || distanceToHook.z < 1)
            {
                balloon.GetComponent<Rigidbody>().isKinematic = true;
            }
        }
    }
}
