using UnityEngine;
using System.Collections;

public class RigMover : MonoBehaviour {

	public static RigMover instance;

	public Transform rig;
	public float transitionSpeed = 1;

	private float transitionDuration = 0.01f;

	private Platform oldPlatform;
	private Platform newPlatform;
	private float switchStartTime = 0;

	// Use this for initialization
	void OnEnable () {
		instance = this;
	}

	public void SetPlatform (Platform platform) {
		if (platform == newPlatform)
			return;

		oldPlatform = newPlatform;
		newPlatform = platform;
		switchStartTime = Time.time;

		if (oldPlatform == null) {
			transitionDuration = 0;
			return;
		}

		Vector3 oldPos = GetRigPositionFromPlatform (oldPlatform);
		Vector3 newPos = GetRigPositionFromPlatform (newPlatform);
		transitionDuration = Vector3.Distance (oldPos, newPos);
		transitionDuration = Mathf.Max (transitionDuration, 0.01f);
	}

	// Update is called once per frame
	void LateUpdate () {
		if (newPlatform == null)
			return;
		
		float lerp = Mathf.Clamp01 ((Time.time - switchStartTime) / transitionDuration);

		if (lerp == 1 || oldPlatform == null) {
			rig.transform.position = GetRigPositionFromPlatform (newPlatform);
		}
		else {
			Vector3 oldPos = GetRigPositionFromPlatform (oldPlatform);
			Vector3 newPos = GetRigPositionFromPlatform (newPlatform);
			rig.transform.position = Vector3.Lerp (oldPos, newPos, lerp);
		}
	}

	Vector3 GetRigPositionFromPlatform (Platform platform) {
		return platform.platform.position
			+ new Vector3 (-platform.tileX, 0, -platform.tileY) * Controller.instance.size;
	}
}
