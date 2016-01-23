using UnityEngine;
using System.Collections;

namespace Rucrede.SwipeStory {

	public class Bouncer :MonoBehaviour, ILoopEffect {
	
	public bool StartImmediate = true;
	public Tween.EaseType EaseType = Tween.EaseType.easeInOutSine;
	public float LoopDuration = 1.0f;
	public bool Yoyo = true;
	public int LoopCount = -1;
	public bool RandomStartProgress = true;
	public Vector3 BounceOffset;
	
	public bool IsLooping {
		get {return _looping;}
	}
	
	public bool IsReversed {
		get {return _reverse;}
	}
	
	protected Vector3 _startPosition;
	protected Tween _tween;
	protected bool _looping;
	protected bool _reverse;
	
	// Use this for initialization
	void Awake () {
		Init();
	}
	
	void Start() {
		if (StartImmediate){
			Play();
		}
	}
	
	public void Init() {
		_looping = false;
		_startPosition = transform.localPosition;
	}
	
	public void Play() {
		if (_tween != null) {
			_tween.Destroy();
			_tween = null;
		}
		
		_looping = true;
		
		Vector3 theStartPos = _startPosition;	
		Vector3 theEndPos =_startPosition + BounceOffset;
			
		if (_reverse) {
			theStartPos = _startPosition + BounceOffset;
			theEndPos = _startPosition;
		}
		
		//LOOP YOYO:
		_tween = Tween.to(theStartPos, theEndPos, LoopDuration, EaseType, 
			(Tween t) => { transform.localPosition = (Vector3) t.Value;},
			null,
			0f,
			Yoyo,
			LoopCount
		);
		
		if (RandomStartProgress) {
			_tween.SetProgress(Random.Range(0f, 1f));
		}
	}
	
	public void Stop() {
		_looping = false;
	
		if (_tween != null) {
			_tween.Destroy();
			_tween = null;
		}
		
		_tween = Tween.to(transform.localPosition, _startPosition, LoopDuration, Tween.EaseType.easeInOutSine, 
			(Tween t) => { transform.localPosition = (Vector3) t.Value;},
			(Tween t) => { _tween = null;}
		);	
	}
	
	public void SetReverse(bool value) {
		_reverse = value;
	}
	
	public void SetProgress(float value) {
		if (_tween != null) {
			_tween.SetProgress(value);
		}
	}
	
	public float GetProgress() {
		if (_tween != null) {
			return _tween.Progress;
		}
		
		return 0f;
	}
	
	void OnDestroy() {
		if (_tween != null)
			_tween.Destroy();
	}
	
}
}