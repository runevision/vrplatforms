using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Platform : MonoBehaviour {

	public const float duration = 4;

	[Range (-1, 1)]
	public int tileX = 0;
	[Range (-1, 1)]
	public int tileY = 0;

	public Transform endA;
	public Transform endB;
	public Transform platform;

	public Material markerMaterialA;
	public Material markerMaterialB;

	// Use this for initialization
	void Start () {
		if (Application.isPlaying)
			platform.gameObject.SetActive (true);
	}
	
	// Update is called once per frame
	void Update () {
		if (!platform.gameObject.activeInHierarchy)
			return;
		float lerp = Mathf.Sin (Time.time * Mathf.PI * 2 / duration) * 0.5f + 0.5f;
		platform.position = Vector3.Lerp (endA.position, endB.position, lerp);
	}

	void OnRenderObject () {
		if (Camera.current == Camera.main)
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
		float size = 0.5f;

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
			GL.Vertex (new Vector3 (-1, 0,  1) * size);

			GL.TexCoord2 (u2, v2);
			GL.Vertex (new Vector3 ( 1, 0,  1) * size);

			GL.TexCoord2 (u2, v1);
			GL.Vertex (new Vector3 ( 1, 0, -1) * size);

			GL.TexCoord2 (u1, v1);
			GL.Vertex (new Vector3 (-1, 0, -1) * size);
		}
		GL.End ();

		GL.PopMatrix ();
	}
}
