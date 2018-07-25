using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBob : MonoBehaviour {

    public float speed = 2.0f;
    public Vector3 moveTo;
    public bool loop = true;

    private Vector3 moveFrom;
    private bool moveBack;

	// Use this for initialization
	void Start () {
        moveFrom = transform.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (loop)
        {
            if(!moveBack)
            {
                transform.position = Vector3.Lerp(moveFrom, moveTo, Time.deltaTime * speed);
            }
            else
            {
                transform.position = Vector3.Lerp(moveTo, moveFrom, Time.deltaTime * speed);
            }
        }
        else
        {
            transform.position = Vector3.Lerp(moveFrom, moveTo, Time.deltaTime * speed);
        }
        	
	}
}
