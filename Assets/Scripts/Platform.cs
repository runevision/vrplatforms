﻿using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Platform : MonoBehaviour {

	[Range (-1, 1)]
	public int tileX = 0;
	[Range (-1, 1)]
	public int tileY = 0;

	public Transform endA;
	public Transform endB;
	public Transform platform;

    public bool customDuration;
    [Range(2, 16)]
    public float duration = 2;
    public bool onlyOnce;
    public bool rocket;

	public Material markerMaterialA;
	public Material markerMaterialB;

    private float timer = 0;
    private int pauses = 0;
    private bool start = false;
    [Range(0, 100)]
    public float acceleration = 10;
    [Range(0, 500)]
    public float maxSpeed = 200;
    private float velocity = 0;
    private float distance = 0;
    private bool particlesSpawned = false;

    private Color origColor;

    public AudioSource stepAudio;
	public AudioSource impactAudio;
    public ParticleSystem particles;

	// Use this for initialization
	void Start () {
		if (Application.isPlaying)
        {
			platform.gameObject.SetActive(true);

            //TODO: Spawn particle system.
			if (particles != null) {
				particles = (ParticleSystem)Instantiate(particles, platform.position, Quaternion.Euler(90,0,0));
           	 	particles.transform.parent = platform;
           	 }

			origColor = GetComponentInChildren<MeshRenderer>().material.color;
        }
	}

    // Update is called once per frame
    void Update()
    {
        if (endA.position == endB.position)
            return;

        float localDuration;
        if (customDuration)
            localDuration = duration;
        else
            localDuration = Controller.instance.duration;

        if (!platform.gameObject.activeInHierarchy)
            return;

        if (onlyOnce || rocket)
        {
            if (start)
            {
                if (timer == 0)
                    timer = Time.time;

                float lerp;

                if(onlyOnce)
                { 
                    if (Time.time - timer < localDuration)
                    {
                        lerp = Mathf.Cos((Time.time - timer) * Mathf.PI * 2 / (localDuration * 2) + Mathf.PI) * 0.5f + 0.5f;
                    }
                    else
                    {
                        lerp = 1;
                        if (!particlesSpawned) {
							StartCoroutine(Impact());
							particlesSpawned = true;
                        }
                    }
                }
                else
                {
                    OnTrigger.instance.Win();
                    velocity += acceleration * Time.deltaTime;
                    if (velocity > maxSpeed)
                        velocity = maxSpeed;
                    distance += velocity * Time.deltaTime;
                    float totdistance = Vector3.Distance(endA.position, endB.position);
                    lerp = distance / totdistance;

                    if (lerp >= 1)
                    	OnTrigger.instance.FadeAndRestart();
                }

                platform.position = Vector3.Lerp(endA.position, endB.position, lerp);
            }
        }
        else
        {
            if (timer == 0)
                timer = Time.time;

            float lerp;
            if (Time.time - timer + 0.1 < localDuration)
            {
                lerp = Mathf.Cos((Time.time - timer) * Mathf.PI * 2 / (localDuration * 2)) * 0.5f + 0.5f;
                if (pauses % 2 == 1)
                    lerp = 1 - lerp;
            }
            else
            {
                if (!particlesSpawned)
                { 
                    StartCoroutine(Impact());
                    particlesSpawned = true;
                }
                lerp = pauses % 2;
                if (Time.time - timer > localDuration + Controller.instance.pause)
                {
                    timer += localDuration + Controller.instance.pause;
                    pauses++;
                    particlesSpawned = false;
                }
            }
            platform.position = Vector3.Lerp(endA.position, endB.position, lerp);
        }
    }

    IEnumerator Impact () {
        particles.Play();
        if (impactAudio)
        	impactAudio.Play();
        yield return new WaitForSeconds(0.1f);
        particles.Stop();
    }

	void OnRenderObject () {
		if (Camera.current.tag == "MainCamera")
			return;

		DrawEnd (endA, markerMaterialA);
		if (endB.position != endA.position)
			DrawEnd (endB, markerMaterialB);

		GL.PushMatrix ();
		// Draw line
		GL.Begin (GL.LINES);
		{
			GL.Vertex (endA.position);
			GL.Vertex (endB.position);
		}
		GL.End ();
		GL.PopMatrix ();
	}

	void DrawEnd (Transform tr, Material mat) {
		float halfSize = 0.5f * Controller.instance.size;

		// Apply the material
		mat.SetPass (0);

		GL.PushMatrix ();
		// Set transformation matrix for drawing to
		// match our transform
		GL.MultMatrix (tr.localToWorldMatrix);

		// Draw quad
		GL.Begin (GL.QUADS);
		{
			//GL.Color (color);

			float u1 = 1/3f * (tileX + 1);
			float u2 = 1/3f * (tileX + 2);
			float v1 = 1/3f * (tileY + 1);
			float v2 = 1/3f * (tileY + 2);

			GL.TexCoord2 (u1, v2);
			GL.Vertex (new Vector3 (-1, 0.01f,  1) * halfSize);

			GL.TexCoord2 (u2, v2);
			GL.Vertex (new Vector3 ( 1, 0.01f,  1) * halfSize);

			GL.TexCoord2 (u2, v1);
			GL.Vertex (new Vector3 ( 1, 0.01f, -1) * halfSize);

			GL.TexCoord2 (u1, v1);
			GL.Vertex (new Vector3 (-1, 0.01f, -1) * halfSize);
		}
		GL.End ();

		GL.PopMatrix ();
	}

    public void StartMoving()
    {
        start = true;
        if(rocket)
            GetComponentInChildren<AudioSource>().Play();
    }

    public void StepOnto () {
        RigMover.instance.SetPlatform (this);
		GetComponentInChildren<MeshRenderer>().material.color = new Color (origColor.r * 1.2f, origColor.g * 1.2f, origColor.b * 1.2f, origColor.a);

        if(onlyOnce || rocket)
            StartMoving();

        if (stepAudio)
            stepAudio.Play ();
    }

    public void StepOff () {
		GetComponentInChildren<MeshRenderer>().material.color = origColor;
    }
}
