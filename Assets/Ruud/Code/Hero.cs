using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[ExecuteInEditMode]
public class Hero : MonoBehaviour {

	public Transform LookAtTransform;
	public float HeroEyeControlRange = 15f;

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
		
		int layermask = 1 << LayerMask.NameToLayer("LevelGeometry") | 1 << LayerMask.NameToLayer("Crystal");
		
		//geometry ray cast
		if (Physics.Raycast(transform.position, transform.forward, out hit, 1000f, layermask)) {
			LookAtTransform.position = hit.point;
		}
		
		//eyes ray cast
		if (Physics.Raycast(transform.position, transform.forward, out hit, HeroEyeControlRange, 1 << LayerMask.NameToLayer("TempleEye"))) {
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
		Vector3 forward = transform.forward * HeroEyeControlRange;
		Gizmos.color = Color.green;
		Gizmos.DrawRay(transform.position, forward);
	}
}
