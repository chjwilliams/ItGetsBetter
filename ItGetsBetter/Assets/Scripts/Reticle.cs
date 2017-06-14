using UnityEngine;
using System.Collections;

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	Reticle: Handles reticle rotation around player						        		*/
/*			Functions:																	*/
/*					public:																*/
/*																						*/
/*					private:															*/
/*						void Start ()													*/
/*						void Update ()													*/
/*																						*/
/*--------------------------------------------------------------------------------------*/
public class Reticle : MonoBehaviour 
{
	//	Public Variables
	public int rotationOffset;			//	Adjust rotation so the reticle points at the mouse

	//	Private Variables
	private PlayerControls _Player;		//	Reference to the player

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	Start: Runs once at the begining of the game. Initalizes variables.					*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	void Start () 
	{
		_Player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerControls> ();
	}
	
	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	Update: Called once per frame														*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	void Update () 
	{
		// Subtracting position of player from the mouse position
		Vector3 difference = Camera.main.ScreenToWorldPoint (Input.mousePosition) - transform.position;
		// Normalizing Vector. Meaning the sum of the Vector will be equal to 1
		difference.Normalize ();
		// Find angles in degrees
		float zRotation = Mathf.Atan2 (difference.y, difference.x) * Mathf.Rad2Deg;

		//	Rotates Reticle
		transform.rotation = Quaternion.Euler (0f, 0f, zRotation + rotationOffset);
		transform.position = new Vector3 (_Player.transform.position.x, _Player.transform.position.y, _Player.transform.position.z);
	}
}
