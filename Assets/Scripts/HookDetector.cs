using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookDetector : MonoBehaviour
{

    [SerializeField]
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player.GetComponent<GrapplingHook>().hooked = false;
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Cloud")
        {
            player.GetComponent<GrapplingHook>().hooked = true;
            player.GetComponent<GrapplingHook>().HookedObject = other.gameObject;
        }
    }
}
