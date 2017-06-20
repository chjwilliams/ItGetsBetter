using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItGetsBetterGameEvents;

public class GameManager : MonoBehaviour
{


    
	// Use this for initialization
	void Start ()
    {

        
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Services.Events.Fire(new ApplyPressureEvent());
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
