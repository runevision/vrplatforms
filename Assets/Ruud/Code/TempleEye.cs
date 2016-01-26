using UnityEngine;
using System.Collections;
using Rucrede;

public class TempleEye : MonoBehaviour {

	public Hero _hero;
	public MeshRenderer _eyeRenderer;
	public float DeactivatedChargeValue = 0f;
	public float ActivatedChargeValue = 0.54f;
	public float TimeToActivateEye = 2.0f;
	
	protected Quaternion _initialEyeRotation;
	protected AreaOfEffect _areaOfEffect;
	protected int _eyeRotationCount = 0;
	protected int _randomEyeRotations = 10;
	protected bool _followHeroGaze = false;
	protected bool _heroIsLooking = false;
	protected bool _eyeActivated = false;
	protected AudioSource _chargeAudio;
	protected LaserBeam _laser;
	protected Crystal _crystal = null;
	protected Tween _laserChargeTween;
	private Tween currentTween = null;

	public float IrisCharge {
		get {
			return _eyeRenderer.material.GetFloat("_panning");
		}
		set{
			_eyeRenderer.material.SetFloat("_panning", value);
		}
	}

	// Use this for initialization
	void Start () {
		_laser = GetComponentInChildren<LaserBeam>();
		_chargeAudio = GetComponent<AudioSource>();
		_initialEyeRotation = transform.localRotation;
		_areaOfEffect = GetComponentInChildren<AreaOfEffect>();
		_areaOfEffect.Triggered += ActivateEye;
		IrisCharge = DeactivatedChargeValue;
		EnableLaser(false);
	}
	
	protected void CancelLaserCharge() {
		if (_laserChargeTween != null) {
			_laserChargeTween.Destroy();
			_laserChargeTween = null;
		}
		
		if (_chargeAudio != null)
			_chargeAudio.Stop();
			
		DischargeLaser();
	}

	protected void EnableLaser(bool aEnable) {
		_followHeroGaze = true;
		if (_laser != null)
			_laser.Enable(aEnable);
	}
	
	protected void LockToPlayer() {
		_followHeroGaze  = true;
		ChargeLaser();
	}
	

	protected void ActivateEye() {
		if (!_eyeActivated) {
			_eyeActivated = true; 
			ChargeLaser();
		}
	}
	
	protected void ChargeLaser() {
		if (_chargeAudio != null)
			_chargeAudio.Play();
				
		_laserChargeTween = Tween.to(IrisCharge, ActivatedChargeValue, 2.0f, Tween.EaseType.linear,
		(Tween t) => {IrisCharge = (float)t.Value; },
		(Tween t) => { ActivateLaser();}
		);
	}
	
	protected void ActivateLaser() {
		EnableLaser(true);
	}
	
	protected void DischargeLaser() {
		_laserChargeTween = Tween.to(IrisCharge, DeactivatedChargeValue, 1.0f, Tween.EaseType.linear,
			(Tween t) => {IrisCharge = (float)t.Value; }
		);
	}

	protected void IgnorePlayer() {
		EnableLaser(false);
		_followHeroGaze = false;
		_eyeRotationCount = 0;
		_randomEyeRotations = Random.Range(3, 4);
		
		DischargeLaser();
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
				Crystal theCristal = hit.collider.GetComponent<Crystal>();
				
				if (theCristal != null) {
					if (theCristal.AddEye(this)) {
						_followHeroGaze = false;
						_crystal = theCristal;
					}
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
			transform.rotation = Quaternion.Slerp(transform.rotation, lookAtRotation, 0.5f * Time.deltaTime);
		}
	}
	
	void OnDrawGizmos() {	
		Vector3 forward = transform.forward * 100f;
		Gizmos.color = Color.red;
		Gizmos.DrawRay(transform.position, forward);
	}
	
	
	
	
}
