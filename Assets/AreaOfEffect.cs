using UnityEngine;
using System.Collections;

public delegate void VoidDelegate();

public class AreaOfEffect : MonoBehaviour {
	public VoidDelegate Triggered; 
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	
	}
	
	void OnTriggerEnter(Collider other) {
		//Destroy(other.gameObject);
		//Debug.Log("Activate laser for: " + other);
		if (Triggered != null)
			Triggered.Invoke();
	}
}
