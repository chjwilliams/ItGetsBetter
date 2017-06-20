using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireMover : MonoBehaviour
{
    public bool burning;
    public ParticleSystem blackFire;
    public ParticleSystem[] whiteParticleSystem;
    public Collider2D coll;
	// Use this for initialization
	void Start ()
    {
        burning = false;
        blackFire = GameObject.FindGameObjectWithTag("BlackFire").GetComponent<ParticleSystem>();
        whiteParticleSystem = transform.GetComponentsInChildren<ParticleSystem>();

        coll = GetComponent<Collider2D>();

        for (int i = 0; i < whiteParticleSystem.Length; i++)
        {
            whiteParticleSystem[i].Stop();
        }

        blackFire.Stop();
    }
	
	// Update is called once per frame
	void Update ()
    {

        if (!burning)
        {
            for (int i = 0; i < whiteParticleSystem.Length; i++)
            {
                whiteParticleSystem[i].Stop();
               ParticleSystem.MainModule settings = whiteParticleSystem[i].main;
               settings.startColor = new ParticleSystem.MinMaxGradient(new Color(BackgroundColor.cameraXPos / 512, BackgroundColor.cameraXPos / 512, BackgroundColor.cameraXPos / 512));


            }

            ParticleSystem.MainModule settingsB = blackFire.main;
            settingsB.startColor = new ParticleSystem.MinMaxGradient(Color.black);

            blackFire.Stop();
            coll.enabled = false;
        }
        else
        {
            coll.enabled = true;
        }
	}
}
