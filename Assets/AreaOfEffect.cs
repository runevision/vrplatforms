using UnityEngine;
using System.Collections;

public delegate void VoidDelegate();

public class AreaOfEffect : MonoBehaviour {
	public VoidDelegate Triggered; 
	
	void OnTriggerEnter(Collider other) {
		if (Triggered != null)
			Triggered.Invoke();
	}
}
