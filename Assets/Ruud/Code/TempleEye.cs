using UnityEngine;
using System.Collections;
using Rucrede;

//[ExecuteInEditMode]
public class TempleEye : MonoBehaviour {

	protected Hero _hero;
	protected AudioSource _hypnoAudio;
	protected LaserBeam _laser;
	protected MeshRenderer _eyeRenderer;
	protected Quaternion _initialEyeRotation;
	
	protected int _eyeRotationCount = 0;
	protected int _randomEyeRotations = 10;
	
	public Color DeactivatedColor = Color.green;
	public Color ActivatedColor = Color.red;
	
	public float SearchInterval = 0.5f;
	public float SearchSpreadInterval = 2.0f;
	
	public float FadeHeroInterval = 1.0f;
	public float FadeHeroSpreadInterval = 2.0f;
	
	public float MaxYAxisRotation = 25f;	
	public float MaxXAxisRotation = 25f;
	
	public float TimeToActivateEye = 2.0f;
	
	protected bool _followHeroGaze = false;
	protected bool _heroIsLooking = false;
	protected Crystal _crystal = null;
	private Tween currentTween = null;

	// Use this for initialization
	void Start () {
		_hero = GameObject.FindObjectOfType<Hero>();
		_laser = GetComponentInChildren<LaserBeam>();
		_eyeRenderer = GetComponent<MeshRenderer>();
		_initialEyeRotation = transform.localRotation;
		IrisColor = DeactivatedColor;
		EnableLaser(false);
		//StartSearchInterval();
	}
	
	public void HeroLooking(bool aLooking) {
		if (_heroIsLooking == aLooking)
			return;
		
		_heroIsLooking = aLooking;
		
		if (_heroIsLooking){
			Debug.Log(name + " hero is looking");
			ChargeLaser();
		}else {
			Debug.Log(name + " hero is not looking");
			CancelLaserCharge();
			StartSearchInterval();
		}
		
		CancelCurrentTween();
	}
	
	protected void CancelLaserCharge() {
		if (laserChargeTween != null) {
			laserChargeTween.Destroy();
			laserChargeTween = null;
		}
		
		if (_hypnoAudio != null)
			_hypnoAudio.Stop();
	}

	public Color IrisColor {
		get {
			if (_eyeRenderer == null){
				return Color.green;
			}
		
			return _eyeRenderer.material.GetColor("_Color");
		}
		set{
			_eyeRenderer.material.SetColor("_Color", value);
		}
	}
	
	protected void EnableLaser(bool aEnable) {
		_followHeroGaze = true;
		if (_laser != null)
			_laser.Enable(aEnable);
	}

	protected void StartSearchInterval() {
		float delay = SearchInterval + Random.Range(0.1f, 0.4f); //SearchSpreadInterval);
		currentTween = Tween.delayedCall(delay, RandomLook);
	}
	
	protected void RandomLook() {
		
		float axisChance = Random.Range(0f, 1.0f);
		Quaternion lookDirection;
		
		if (axisChance < 0.5f) {
			lookDirection = Quaternion.Euler(Vector3.up * Random.Range(-MaxXAxisRotation, MaxXAxisRotation));
		} else {
			lookDirection = Quaternion.Euler(Vector3.forward * Random.Range(-MaxYAxisRotation, MaxYAxisRotation));
		}
		
		currentTween = Tween.to(transform.localRotation, _initialEyeRotation * lookDirection, 0.3f, Tween.EaseType.easeInOutCubic,
		         (Tween tween) => {transform.localRotation = (Quaternion)tween.Value;}, 
		(Tween tween) => {StartSearchInterval();}, 
		0f, false, 0);
	}
	
	protected void StartFaceHeroInterval() {
		float delay = FadeHeroInterval + Random.Range(1.0f, FadeHeroSpreadInterval);
		currentTween = Tween.delayedCall(delay, FaceHero);
	}
	
	protected void FaceHero() {	
		Quaternion rot = Quaternion.LookRotation(_hero.LookAtTransform.position - transform.position);		
		
		currentTween = Tween.to(transform.rotation, rot, 0.2f, Tween.EaseType.easeInOutCubic,
		         (Tween tween) => {transform.rotation = (Quaternion)tween.Value;}, 
		         (Tween t) => {LockToPlayer();}  
		      	, 0f, false, 0);
		
		StartFaceHeroInterval();
	}
	
	protected void LockToPlayer() {
		_followHeroGaze  = true;
		ChargeLaser();
	}
	
	protected Tween laserChargeTween;
	protected void ChargeLaser() {
	
		if (_hypnoAudio != null)
			_hypnoAudio.Play();
				
		laserChargeTween = Tween.to(IrisColor, ActivatedColor, 2.0f, Tween.EaseType.linear,
		(Tween t) => {IrisColor = (Color)t.Value; },
		(Tween t) => { ActivateLaser();}
		);
	}
	
	protected void ActivateLaser() {
		EnableLaser(true);
	}
	
	protected void DischargeLaser() {
		currentTween = Tween.to(IrisColor, DeactivatedColor, 1.0f, Tween.EaseType.linear,
			(Tween t) => {IrisColor = (Color)t.Value; }
		);
	}

	protected void IgnorePlayer() {
		EnableLaser(false);
		_followHeroGaze = false;
		_eyeRotationCount = 0;
		_randomEyeRotations = Random.Range(3, 4);
		
		DischargeLaser();
		RandomLook();
	}
	
	void CancelCurrentTween() {
		if (currentTween != null) {
			currentTween.Destroy();
			currentTween = null;
		}
	}
	
	// Update is called once per frame
	void Update () {
		Transform _target = null;
		
		if (_followHeroGaze && _crystal == null) {
			_target = _hero.LookAtTransform;
				
			//check if you hit a crystal:
			RaycastHit hit;
			if (_laser.Enabled && _followHeroGaze && Physics.Raycast(transform.position, transform.forward, out hit, 100f, 1 << LayerMask.NameToLayer("Crystal"))) {
				_followHeroGaze = false;
				_crystal = hit.collider.GetComponent<Crystal>();
				
				if (_crystal != null) {
					_crystal.AddEye(this);
				} else {
					Debug.Log("Did not find crystal");
				}
				
				CancelCurrentTween();
			}
		} else if (_crystal != null) {
			_target = _crystal.transform;
		}
		
		if (_crystal == null && _heroIsLooking) {
			_target = _hero.transform;
		}
		
		if (_target) {
			Vector3 dir = _target.position - transform.position;
			Quaternion lookAtRotation = Quaternion.LookRotation(dir, _hero.transform.up);
			transform.rotation = Quaternion.Slerp(transform.rotation, lookAtRotation, 4.0f * Time.deltaTime);
		}
	}
	
	void OnDrawGizmos() {	
		Vector3 forward = transform.forward * 100f;
		Gizmos.color = Color.red;
		Gizmos.DrawRay(transform.position, forward);
	}
	
	
	
	
}
