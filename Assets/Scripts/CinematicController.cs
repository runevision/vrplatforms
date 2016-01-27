using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Rucrede; 

public class CinematicController : MonoBehaviour {

	public List<TempleEye> Eyes;
	public List<Crystal> Crystals;
	public List<Camera> CinematicCameras;
	public List<Camera> PlayerCameras;
	public List<float> CameraSwitchIntervals;

	public float ActivateInterval = 0.5f;
	public float ActivateDelay = 2f;
	protected int _eyeIndex = 0;
	protected int _cameraIndex = 0;
	protected int _cameraSwitchInterval = 0;

	// Use this for initialization
	void Start () {
		Tween.delayedCall(ActivateDelay, SetTargets);
		foreach(Camera cam in PlayerCameras)
			cam.enabled = false;

		NextCamera();
	}

	void SetTargets() {
		int i = 0;

		foreach(TempleEye eye in Eyes){
			eye.Target = Crystals[i++].transform;
			Tween.delayedCall(ActivateInterval * (i + 1), ChargeNext);
		}
	}

	void ChargeNext() {
		Eyes[_eyeIndex].ChargeLaser();
		_eyeIndex++;
	}

	void NextCamera() {

		foreach(Camera cam in CinematicCameras) {
			cam.enabled = false;
		}

		CinematicCameras[_cameraIndex].enabled = true;
		_cameraIndex = Mathf.Clamp(++_cameraIndex, 0, CinematicCameras.Count - 1);

		if (_cameraSwitchInterval < CameraSwitchIntervals.Count)
			Tween.delayedCall(CameraSwitchIntervals[_cameraSwitchInterval], NextCamera);

		_cameraSwitchInterval++;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
