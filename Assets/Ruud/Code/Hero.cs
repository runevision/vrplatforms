using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[ExecuteInEditMode]
public class Hero : MonoBehaviour {

	public Transform LookAtTransform;

	protected TempleEye eyeFocussedAt = null;

	//get all eyes:
	protected List<TempleEye> _eyes = null;

	// Use this for initialization
	void Start () {
		_eyes = GameObject.FindObjectsOfType<TempleEye>().ToList();
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit hit;		
		
		//geometry ray cast
		if (Physics.Raycast(transform.position, transform.forward, out hit, 1000f, 1 << LayerMask.NameToLayer("LevelGeometry"))) {
			LookAtTransform.position = hit.point;
		}
		
		//eyes ray cast
		if (Physics.Raycast(transform.position, transform.forward, out hit, 1000f, 1 << LayerMask.NameToLayer("TempleEye"))) {
			eyeFocussedAt = hit.collider.GetComponent<TempleEye>();
			Debug.Log("found eye: " + eyeFocussedAt.name);	
			eyeFocussedAt.HeroLooking(true);
			
		} else{
			eyeFocussedAt = null;
		}
		
		foreach(TempleEye eye in _eyes) {
			if (eye != eyeFocussedAt) {
				eye.HeroLooking(false);
			}	
		}
	}
	
	
	
	void OnDrawGizmos() {	
		Vector3 forward = transform.forward * 100f;
		Gizmos.color = Color.green;
		Gizmos.DrawRay(transform.position, forward);
	}
}
