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

	public bool transitioning {
		get { return ((Time.time - switchStartTime) / transitionDuration) < 1; }
	}

	// Use this for initialization
	void OnEnable () {
		instance = this;
	}

	public void SetPlatform (Platform platform) {
		if (platform == newPlatform)
			return;

		if (oldPlatform != null)
			oldPlatform.StepOff();

		oldPlatform = newPlatform;
		newPlatform = platform;
		switchStartTime = Time.time;

		if (oldPlatform == null || newPlatform == null) {
			transitionDuration = 0;
			return;
		}

		Vector3 oldPos = GetRigPositionFromPlatform (oldPlatform);
		Vector3 newPos = GetRigPositionFromPlatform (newPlatform);
		transitionDuration = Vector3.Distance (oldPos, newPos) / transitionSpeed;
		transitionDuration = Mathf.Clamp (transitionDuration, 0.01f, 1);
	}

	// Update is called once per frame
	void LateUpdate () {
		if (newPlatform == null)
			return;
		
		float lerp = Mathf.Clamp01 ((Time.time - switchStartTime) / transitionDuration);
		lerp = Mathf.SmoothStep (0, 1, lerp);

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
