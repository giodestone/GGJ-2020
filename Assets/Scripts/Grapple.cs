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
    BalloonMove balloon;

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
        
    }

    // Update is called once per frame
    void Update()
    {
       if (fired == false)
        {
            this.transform.position = player.transform.position;
        }
    }

    void FireHook()
    {
        this.transform.Translate(Vector3.forward * Time.deltaTime * hookTravelSpeed);
        currentDistance = Vector3.Distance(this.transform.position, player.transform.position);

        if (currentDistance >= maxDistance)
        {
            ReturnHook();
        }
    }

    void ReturnHook()
    {
        this.transform.position = player.transform.position;
        fired = false;
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
            currentDistance = Vector3.Distance(this.transform.position, player.transform.position);

            //convert current distance to vector3 somehow and put in place of currentDistance
            balloon.GetComponent<BalloonMove>().transform.Translate(distanceToHook * Time.deltaTime * balloonMoveSpeed);
        }
    }
}
