using UnityEngine;
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

	public Material markerMaterialA;
	public Material markerMaterialB;

    private float timer = 0;
    private float adjust = 0;
    private bool start = false;

	// Use this for initialization
	void Start () {
		if (Application.isPlaying)
			platform.gameObject.SetActive (true);
	}

    // Update is called once per frame
    void Update()
    {
        float localDuration;
        if (customDuration)
            localDuration = duration;
        else
            localDuration = Controller.instance.duration;

        if (!platform.gameObject.activeInHierarchy)
            return;

        if (onlyOnce)
        {
            if (start)
            {
                if (timer == 0)
                    timer = Time.time;

                float lerp;
                if (Time.time - timer + 0.1 < localDuration)
                {
                    lerp = Mathf.Cos((Time.time - adjust) * Mathf.PI * 2 / (localDuration * 2)) * 0.5f + 0.5f;
                }
                else
                {
                    lerp = 1;
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
                lerp = Mathf.Cos((Time.time - adjust) * Mathf.PI * 2 / (localDuration * 2)) * 0.5f + 0.5f;
            }
            else
            {
                lerp = (adjust / Controller.instance.pause) % 2; //Should be zero or one depening on where in time we are.
                if (Time.time - timer > localDuration + Controller.instance.pause)
                {
                    timer += localDuration + Controller.instance.pause;
                    adjust += Controller.instance.pause;
                }
            }
            platform.position = Vector3.Lerp(endA.position, endB.position, lerp);
        }

        if (Controller.instance.size != platform.localScale.x)
            platform.localScale = Vector3.one * Controller.instance.size;
    }

	void OnRenderObject () {
		if (Camera.current.tag == "MainCamera")
			return;

		DrawEnd (endA, markerMaterialA);
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
			GL.Vertex (new Vector3 (-1, 0,  1) * halfSize);

			GL.TexCoord2 (u2, v2);
			GL.Vertex (new Vector3 ( 1, 0,  1) * halfSize);

			GL.TexCoord2 (u2, v1);
			GL.Vertex (new Vector3 ( 1, 0, -1) * halfSize);

			GL.TexCoord2 (u1, v1);
			GL.Vertex (new Vector3 (-1, 0, -1) * halfSize);
		}
		GL.End ();

		GL.PopMatrix ();
	}

    void startMoving()
    {
        start = true;
    }
}
