using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundMovement : MonoBehaviour
{

    //private BalloonMove Balloon;
    bool InBalloon = false;
    public bool IsMovementActive = true;

    [SerializeField] private float MoveSpeed = 15.0f;
    [SerializeField] private float JumpVel = 10.0f;
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private bool grounded;

    Vector3 movement;
    // Start is called before the first frame update
    void Start()
    {
        //Balloon = GameObject.Find("Balloon").GetComponent<BalloonMove>();  

    private BalloonMove Balloon;
    bool InBalloon = true;

    [SerializeField] private float MoveSpeed = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        Balloon = GameObject.Find("Balloon").GetComponent<BalloonMove>();  

    }

    // Update is called once per frame
    public void Update()
    {

        CalculateMovement();
    }

    public void CalculateMovement()
    {

        if (IsMovementActive == false) return;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        movement = transform.right * x + transform.forward * z;

        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            rb.velocity = Vector3.up * JumpVel;
            grounded = false;
        }

    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + (movement * MoveSpeed * Time.deltaTime));
    }

    private void OnCollisionEnter(Collision Collider)
    {
        //TODO Ground collision

        if (Collider.transform.position.y < transform.position.y)
        {
            grounded = true;
        float HorizontalInput = Input.GetAxis("Horizontal");
        float VerticalInput = Input.GetAxis("Vertical");

        Vector3 Direction = new Vector3(HorizontalInput, VerticalInput);

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

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Owner1" && Input.GetKey(KeyCode.A))
        {
            //NPC 1 dialogue. Will add soon
        }

        if (other.tag == "Owner2" && Input.GetKey(KeyCode.A))
        {
            //NPC 2 dialogue. Will add soon
        }

        if (other.tag == "Owner3" && Input.GetKey(KeyCode.A))
        {
            //NPC 3 dialogue. Will add soon
        }

    }

}

}
