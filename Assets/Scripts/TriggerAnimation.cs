using UnityEngine;
using System.Collections;

public class TriggerAnimation : MonoBehaviour {

	void OnTriggerEnter () {
		GetComponent<Animation> ().Play();
		GetComponent<BoxCollider> ().enabled = false;
	}
}
