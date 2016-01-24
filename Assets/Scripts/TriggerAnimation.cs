using UnityEngine;
using System.Collections;

public class TriggerAnimation : MonoBehaviour {

	void OnTriggerEnter (Collider other) {
		GetComponent<Animation> ().Play();
		GetComponent<BoxCollider> ().enabled = false;

		AudioSource source = GetComponentInChildren<AudioSource> ();
		if (source)
			source.Play ();
	}
}
