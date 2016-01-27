using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class Crystal : MonoBehaviour {

	public int EyesRequired = 1;
	public GameObject ExplosionParticleEffectPrefab;
	public UnityEvent Activated;

	protected AudioSource _cling; 
	protected List<TempleEye> _eyes;		
	protected bool _activated = false;
	protected MeshRenderer _renderer;

	// Use this for initialization
	void Start () {
		_renderer = GetComponentInChildren<MeshRenderer>();
		_cling = GetComponent<AudioSource>();
		_eyes = new List<TempleEye>();
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
				Explode();
				if (Activated != null) {
					Activated.Invoke();
				}	
			}
		}
		
		return true;
	}

	void Explode() {
		_renderer.enabled = false;
		if (ExplosionParticleEffectPrefab != null) {
			GameObject explosionGOInstance = (GameObject)GameObject.Instantiate(ExplosionParticleEffectPrefab, transform.position,Quaternion.identity); 
			ParticleSystem explosion = explosionGOInstance.GetComponent<ParticleSystem>();
			explosion.Play();
		}
	}
}
