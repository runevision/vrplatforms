using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Rucrede; 

public class CinematicController : MonoBehaviour {

	public bool PlayMode = false;
	public List<TempleEye> Eyes;
	public List<Crystal> Crystals;
	public List<Camera> CinematicCameras;
	public List<Camera> PlayerCameras;
	public List<float> CameraSwitchIntervals;

	public GameObject LevelGameObject;

	public float ActivateInterval = 0.5f;
	public float ActivateDelay = 2f;
	protected int _eyeIndex = 0;
	protected int _cameraIndex = 0;
	protected int _cameraSwitchInterval = 0;


	// Use this for initialization
	void Start () {
		Tween.delayedCall(0.5f, EnableLevel);
		Tween.delayedCall(ActivateDelay, SetTargets);
		foreach(Camera cam in PlayerCameras)
			cam.gameObject.SetActive(false);

		NextCamera();
	}

	void EnableLevel(bool aEnabled = true) {
		LevelGameObject.SetActive(aEnabled);
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
			cam.gameObject.SetActive(false);
		}

		CinematicCameras[_cameraIndex].gameObject.SetActive(true);

		_cameraIndex++;
		if (_cameraIndex > CinematicCameras.Count-1)
			_cameraIndex = 0;

		if (PlayMode) {
			if (_cameraSwitchInterval < CameraSwitchIntervals.Count)
				Tween.delayedCall(CameraSwitchIntervals[_cameraSwitchInterval], NextCamera);

			_cameraSwitchInterval++;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("n")) {
			NextCamera();
		}
	}
}
