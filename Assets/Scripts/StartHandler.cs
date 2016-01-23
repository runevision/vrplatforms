using UnityEngine;
using System.Collections;

public class StartHandler : MonoBehaviour {

	public Transform chest;
	public GameObject level;

	// Use this for initialization
	void Start () {
		level.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 vector = chest.position - transform.position;
		vector.y = 0;
		if (vector.magnitude < 0.5f)
			StartLevel ();
	}

	void StartLevel () {
		gameObject.SetActive (false);
		level.SetActive (true);
	}
}
