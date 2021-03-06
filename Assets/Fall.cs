﻿using UnityEngine;
using System.Collections;

public class Fall : MonoBehaviour {

    private bool falling = false;
    private float velocity = 0;
    [Range(0, 20)]
    public float acceleration = 10f;
    private float maxVelocity = 10;
	
	// Update is called once per frame
	void Update () {
        if (falling)
        {
            Transform rig = GetComponent<Transform>();
            velocity += acceleration*Time.deltaTime;
            if(velocity >= maxVelocity)
                velocity = maxVelocity;
            Vector3 pos = rig.transform.position;
            pos.y -= velocity * Time.deltaTime;
            rig.transform.position = pos;
        }
	}

    public void StartFalling()
    {
        falling = true;
    }

}
