using UnityEngine;
using System.Collections;

public class Rocket : MonoBehaviour {

	public bool IsReadyForLaunch = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void ReadyForLaunch() {
		IsReadyForLaunch = true;
	}
}
