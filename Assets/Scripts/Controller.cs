using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Controller : MonoBehaviour {

	static Controller m_Instance;
	public static Controller instance { get { return m_Instance; } }

	public float duration = 4;
	public float size = 1;

	// Use this for initialization
	void Awake () {
		m_Instance = this;
	}
}
