using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundColor : MonoBehaviour
{
    public const float MAX = 255;
    public const float MIN = 0;
    public float grayScale;
    public static float cameraXPos;
    public static float CameraXPos {
        get { return cameraXPos; }
        set
        {

            cameraXPos = value;
            if(cameraXPos > MAX)
            {
                cameraXPos = MAX;
            }

            if (cameraXPos < MIN)
            {
                cameraXPos = MIN;
            }
        }
    }

    public static Color backgroundColor;
    public Material coverMaterial;
	// Use this for initialization
	void Start ()
    {
        float cameraXPos = Mathf.Abs(transform.position.x);
        coverMaterial = Resources.Load<Material>("Materials/Particle 1");
    }
	
	// Update is called once per frame
	void Update ()
    {
        CameraXPos = Mathf.Floor((255 - Mathf.Abs(transform.position.x)));
        grayScale = CameraXPos;
        backgroundColor = new Color(CameraXPos / 255, CameraXPos/ 255, CameraXPos/ 255);
        coverMaterial.color = backgroundColor;
        Camera.main.backgroundColor = backgroundColor;
	}
}
