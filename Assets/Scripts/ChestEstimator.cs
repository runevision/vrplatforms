using UnityEngine;
using System.Collections;

public class ChestEstimator : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void LateUpdate () {
		Transform head = transform.parent;
		Vector3 headChestDelta = -head.up * 0.4f;

		float adjustFactor = Mathf.Clamp01 (-head.forward.y);
		headChestDelta.x *= adjustFactor;
		headChestDelta.z *= adjustFactor;

		Vector3 chestPos = head.position + headChestDelta;

		transform.position = chestPos;
		transform.rotation = Quaternion.identity;
	}
}
