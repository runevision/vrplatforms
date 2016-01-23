using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Controller : MonoBehaviour {

	static Controller m_Instance;
	public static Controller instance { get { return m_Instance; } }

	[Range (4, 32)]
	public float duration = 4;
	[Range (0.5f, 1.5f)]
	public float size = 1;

	// Use this for initialization
	void OnEnable () {
		m_Instance = this;
	}

	#if UNITY_EDITOR
	void OnValidate () {
		UnityEditor.SceneView.RepaintAll ();
	}
	#endif
}
