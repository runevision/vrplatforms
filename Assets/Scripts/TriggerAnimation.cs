using UnityEngine;
using System.Collections;

public class TriggerAnimation : MonoBehaviour {

	void OnTriggerEnter (Collider other) {
		GetComponent<Animation> ().Play();
		GetComponent<BoxCollider> ().enabled = false;
	}
}
