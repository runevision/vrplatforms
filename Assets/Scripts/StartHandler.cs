using UnityEngine;
using System.Collections;

public class StartHandler : MonoBehaviour {

	public Transform chest;
	public GameObject startMarker;
	public GameObject level;

	// Use this for initialization
	void Start () {
		level.SetActive (false);
		startMarker.SetActive (true);
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 vector = chest.position - transform.position;
		vector.y = 0;
		if (vector.magnitude < 0.3f)
			StartLevel ();
	}

	void StartLevel () {
		startMarker.SetActive (false);
		level.SetActive (true);
	}
}
