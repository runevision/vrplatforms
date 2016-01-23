using UnityEngine;
using System.Collections;

public class ChestEstimator : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void LateUpdate () {
		Transform head = transform.parent;
		Vector3 chestPos = head.position - head.up * 0.5f;

		transform.position = chestPos;
		transform.rotation = Quaternion.identity;
	}
}
