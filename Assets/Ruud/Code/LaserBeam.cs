using UnityEngine;
using System.Collections;

public class LaserBeam : MonoBehaviour {

	protected bool _enabled;
	protected MeshRenderer _renderer;
	protected AudioSource _audio;

	public bool Enabled {
		get {
			return _enabled;
		}
	}
	
	// Use this for initialization
	void Awake () {
		_renderer = GetComponent<MeshRenderer>(); 
		_audio = GetComponentInChildren<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void Enable(bool aEnable) {
		_enabled = aEnable;
		_renderer = GetComponent<MeshRenderer>(); 
		_renderer.enabled = _enabled;
		
		if (_audio != null) {
			if (_enabled)
				_audio.Play();
			else
				_audio.Stop();
		}
		
	}
	
}
