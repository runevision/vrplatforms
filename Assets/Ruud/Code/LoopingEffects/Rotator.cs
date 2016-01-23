using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour, ILoopEffect {
	
	public bool StartImmediate = true;
	public float AngleSpeed = 10f;
	public float Acceleration = 2f;

	public Vector3 Axis = Vector3.forward;
	
	public bool IsLooping {
		get {return _running;}
	}
	
	public bool IsReversed {
		get {return _reverse;}
	}
	
	protected float _targetSpeed;
	protected float _speed;
	protected bool _running = false;
	protected bool _reverse;
	
	// Use this for initialization
	void Awake () {
		Init();
	}
	
	public void Init() {
		_speed = 0f;
		_targetSpeed = AngleSpeed;
		
		if (StartImmediate){
			Play();
		}
	}
	
	public void SetAcceleration(float aAccel) {
		Acceleration = aAccel;
	}
	
	public void SetSpeed(float aSpeed) {
		AngleSpeed = aSpeed;
		_targetSpeed = AngleSpeed;
	}

	public void TogglePlay() {
		if (_running)
			Stop();
		else
			Play();
	}

	public void Play() {
		_targetSpeed = AngleSpeed;
		_running = true;
	}
	
	public void SetProgress(float value) {}
	public float GetProgress() {return 0f;}
	
	public void Stop() {
		_targetSpeed = 0f;
	}
	
	public void SetReverse(bool value) {
		_reverse = value;
	}
	
	// Update is called once per frame
	void Update () {
		if (_running == false)
			return;
	
		if (_speed < _targetSpeed) {
			_speed += Acceleration * Time.deltaTime;
			_speed = Mathf.Min(_speed, _targetSpeed);
		} else if (_speed > _targetSpeed) {
			_speed -= Acceleration * Time.deltaTime;
			_speed = Mathf.Max(_speed, _targetSpeed);
		}

		transform.Rotate(Axis, _speed * Time.deltaTime, Space.Self);
		
		if (_speed == 0f)
			_running = false;
	}
}
