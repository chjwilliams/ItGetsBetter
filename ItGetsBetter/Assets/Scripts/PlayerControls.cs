using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ChrsUtils.ChrsCamera;

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	PlayerControls: Handles player state in N-gon										*/
/*			Functions:																	*/
/*					public:																*/
/*																						*/
/*					private:															*/
/*						void Start ()													*/
/*						void Move (float dx, float dy)									*/
/*						void Shoot ()													*/
/*						void Update ()													*/
/*																						*/
/*--------------------------------------------------------------------------------------*/

/*
 * 
 *  TODO: Have a singular particle system that moves to the position I clicked
 *        If on other thing it activates it's particle system and it stops moving
 */ 

public class PlayerControls : MonoBehaviour 
{

    public const int LEFT_MOUSE = 0;
    public const int RIGHT_MOUSE = 1;

    //	Public Variabels
    public float moveSpeed = 1.0f;					//	Default movement speed of character
    public FireMover fireMover;

	//	Private Variables
	private Rigidbody2D _Rigidbody2D;				//	Reference to player's rigidbody
	private GameObject _Pointer;					//	Shows were the player is aiming
	private Transform _Point;						//	Where the bullets spawn from
    private Animator _Anim;

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	Start: Runs once at the begining of the game. Initalizes variables.					*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	private void Start () 
	{
		_Rigidbody2D = GetComponent<Rigidbody2D> ();
		_Pointer = GameObject.FindGameObjectWithTag ("Pointer");
		_Point = GameObject.FindGameObjectWithTag ("Point").transform;
        _Anim = GameObject.FindGameObjectWithTag("Pulse").GetComponent<Animator>();

        fireMover = GameObject.Find("FirePos").GetComponent<FireMover>();

	}

	protected virtual void OnPlayerIsDead()
	{

	}

	
	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	Move: moves the player in a direction x and/or y based on axis input				*/
	/*		param:																			*/
	/*			float dx - horizontal axis input											*/
	/*			float dy - vertical axis input												*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	private void Move (float dx, float dy)
	{
		_Rigidbody2D.velocity = new Vector2 (dx * moveSpeed, dy * moveSpeed);
	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	Shoot: Tells Basic Bullet class to get active										*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	private void Shoot ()
	{
		//	Shakes camera when 
		CameraShake.CameraShakeEffect.Shake (0.07f, 0.1f);

		//	Creates a new bullet from prefab using the position of the muzzle and rotation of the reticle
		//  GameObject bullet = (GameObject)Instantiate (PrefabManager.Instance.bulletPrefab, _Muzzle.position, _Reticle.transform.rotation);
		//	Takes instantiated bullet and gets its BasicBullet script
		//  BasicBullet newBullet = bullet.GetComponent<BasicBullet> ();

		//	Adjusts the trajectory of bullet shot to account for unity's coordinates system
		Transform adjustBullettrajectory = _Pointer.transform;
		//	Rotates
		adjustBullettrajectory.Rotate (0.0f, 0.0f, 90.0f);
		float theta = adjustBullettrajectory.rotation.eulerAngles.z;

        //	Moves bullet in appropriate direction
        //  newBullet.dy = Mathf.Sin (theta.toRadians());
        //  newBullet.dx = Mathf.Cos (theta.toRadians());


        //	A list so we can keep track of the bullets
        //_BulletList.Add (newBullet);

        //  Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //  if (Physics.Raycast(ray))
        //  Instantiate(particle, transform.position, transform.rotation);
        
    }

    private void Fire()
    {
        if (!fireMover.burning)
        {
            for (int i = 0; i < fireMover.whiteParticleSystem.Length; i++)
            {
                fireMover.whiteParticleSystem[i].Play();
            }

            fireMover.blackFire.Play();

            StartCoroutine(StopBurning());
            fireMover.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            fireMover.transform.position = new Vector3(fireMover.transform.position.x, fireMover.transform.position.y, 5);
        }
       
    }

    private IEnumerator StopBurning()
    {
        fireMover.burning = true;
        yield return new WaitForSeconds(1.0f);
        fireMover.burning = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Pressure")
        {
            moveSpeed = moveSpeed * 0.8f;

            if(moveSpeed < 1.0f)
            {
                moveSpeed = 1.0f;
            }
        }
    }

    /*--------------------------------------------------------------------------------------*/
    /*																						*/
    /*	Update: Called once per frame														*/
    /*																						*/
    /*--------------------------------------------------------------------------------------*/
    private void Update () 
	{
		//	Take in a float value from Input axes
		float x = Input.GetAxis ("Horizontal");
		float y = Input.GetAxis ("Vertical");

		//	Apply the values in here.
		Move (x, y);

        if(Input.GetKeyDown(KeyCode.G))
        {
            _Anim.speed += 0.3f;
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            _Anim.speed -= 0.3f;
        }

        if(Input.GetKeyDown(KeyCode.Z))
        {
            transform.position = new Vector3(-255, 0, 0);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            transform.position = new Vector3(-145, 0, 0);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            transform.position = new Vector3(0, 0, 0);
        }

        //	If Left or Right mouse button clicked, shoot
        if (Input.GetMouseButton(LEFT_MOUSE) || Input.GetMouseButton(RIGHT_MOUSE))
		{
			Shoot ();
		}

        if(Input.GetMouseButtonDown(LEFT_MOUSE) || Input.GetMouseButtonDown(RIGHT_MOUSE))
        {
            Fire();
        }

	}
}
