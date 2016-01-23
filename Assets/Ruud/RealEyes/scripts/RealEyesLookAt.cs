
using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class RealEyesLookAt : MonoBehaviour {
	
	public Transform target;
	public Transform leftEye;
	public Transform rightEye;
	public float maxRotationUp = 30.0f;
	public float maxRotationDown = 30.0f;
	public float maxRotationLeft = 30.0f;
	public float maxRotationRight = 30.0f;
	public float crosseyedDegrees = -3.0f;
	private Vector3 leftEyeStartRotation;
	private Vector3 rightEyeStartRotation;
	
	void Update () {	
		Vector3 lookAt;
		
		if (target){		
			lookAt = target.transform.position - transform.position;
		}
		else{
			lookAt = transform.forward;
		}
		
		Vector2 lookAtLeftRight = new Vector2(lookAt.x, lookAt.z);
		Vector2 lookAtUpDown = new Vector2(lookAt.y, lookAt.z);
		Vector2 eyeLeftRight = new Vector2(transform.right.x, transform.right.z);
		Vector2 eyeUpDown = new Vector2(transform.up.y, transform.up.z);
		
		float angleLeftRight = Mathf.Clamp( Vector2.Angle(lookAtLeftRight, eyeLeftRight), 90f - maxRotationRight, 90f + maxRotationLeft);
		float angleUpDown = Mathf.Clamp( Vector2.Angle(lookAtUpDown, eyeUpDown), 90f - maxRotationUp, 90f + maxRotationDown);

		angleLeftRight = (angleLeftRight/180f);
		angleUpDown = (angleUpDown/180f);
		
		float angleX = Mathf.Lerp(-90,90,angleUpDown);
		float angleY = Mathf.Lerp(90,-90,angleLeftRight);
		Vector3 leftAngles = new Vector3(angleX, angleY + crosseyedDegrees, 0f);
		Vector3 rightAngles = new Vector3(angleX, angleY - crosseyedDegrees, 0f);
		
		leftEye.localEulerAngles = leftAngles;
		rightEye.localEulerAngles = rightAngles;
	}
	
	void LookAt(Transform lookAtTarget){
		target = lookAtTarget;
	}
}
