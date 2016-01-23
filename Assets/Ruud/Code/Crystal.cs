using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class Crystal : MonoBehaviour {

	public int EyesRequired = 1;
	public List<TempleEye> _eyes;		
	public UnityEvent Activated;
	protected AudioSource _cling; 
	
	protected bool _activated = false;
	protected MeshRenderer _renderer;
	
	public Material DeactivatedMaterial;
	public Material ActivatedMaterial;
	
	// Use this for initialization
	void Start () {
		_renderer = GetComponentInChildren<MeshRenderer>();
		_cling = GetComponent<AudioSource>();
		_renderer.material = DeactivatedMaterial;
	}
	
	public bool AddEye(TempleEye eye) {		
		if (_eyes.Count < EyesRequired) {
			_eyes.Add(eye);	
		
		if (_cling != null)
			_cling.Play();
		
		} else {
			return false;
		}
		
		if (_eyes.Count >= EyesRequired) {	
			if (!_activated) { 
				_activated = true;
				_renderer.material = ActivatedMaterial;
				if (Activated != null)
					Activated.Invoke();	
			}
		}
		
		return true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
