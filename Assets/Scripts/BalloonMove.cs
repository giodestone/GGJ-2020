using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonMove : MonoBehaviour
{
    [SerializeField]
    private float MoveSpeed = 5.0f;

    private bool InBalloon = true;

    [SerializeField]
    private GroundMovement GroundPlayer; 

    // Start is called before the first frame update
    void Start()
    {
        GroundPlayer = GameObject.Find("GroundedPlayer").GetComponent<GroundMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
    }

    void CalculateMovement()
    {
        if (InBalloon == true)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Translate(0.0f, 0.0f, MoveSpeed * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.Translate(0.0f, 0.0f, -MoveSpeed * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                transform.Translate(MoveSpeed * Time.deltaTime, 0.0f, 0.0f);
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                transform.Translate(MoveSpeed * Time.deltaTime, 0.0f, 0.0f);
            }
            else if (Input.GetKey(KeyCode.Space))
            {
                transform.Translate(0.0f, MoveSpeed * Time.deltaTime, 0.0f);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "cloud" && InBalloon == true)
        {
            InBalloon = false;
        }
        else if (InBalloon == false)
        {
            InBalloon = true;
        }
    }
}
