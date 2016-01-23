using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[ExecuteInEditMode]
public class Controller : MonoBehaviour {

	static Controller m_Instance;
	public static Controller instance { get { return m_Instance; } }

    [Range(2, 16)]
    public float duration = 2;
	[Range (0.5f, 1.5f)]
    public float size = 1;
    [Range(0, 15)]
    public float pause = 1;

	// Use this for initialization
	void OnEnable () {
		m_Instance = this;
	}

	void Update () {
		if (Input.GetKeyDown(KeyCode.Space))
			SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
	}

	#if UNITY_EDITOR
	void OnValidate () {
		UnityEditor.SceneView.RepaintAll ();
	}
	#endif
}
