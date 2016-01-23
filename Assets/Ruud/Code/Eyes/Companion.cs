using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Rucrede;

public class Companion : MonoBehaviour {

	public EyeStatesEnum State = EyeStatesEnum.HAPPY;	
	public Transform PlayerTransform;
	
	public Transform ContainerTransform;
	public Transform BodyTransform;
	public Transform MouthTransform;
	public Eye Eye;
	//public CollisionNotifier PersonalSpaceNotifier;
	
	public bool LookAtPlayerContinuous = false;
	public bool FacePlayerSporadically = false;
	
	[SerializeField]
	public List<ReactionTypesEnum> PersonalSpaceReactions;
	
	public float SayHelloInterval = 3.0f;
	public float SayHelloSpreadInterval = 20.0f;
	
	public float SearchInterval = 0.5f;
	public float SearchSpreadInterval = 0.5f;
	
	public float MoveInPlaceInterval = 3.0f;
	public float MoveInPlaceSpreadInterval = 20.0f;
	
	public AudioSource HelloSound;
	
	public float HoverYoyoInterval = 5.0f;
	public float HoverDistance = 1.0f;
	
	public float BreathYoyoInterval = 0.5f;
	
	protected Tween _hoverTween;
	protected Tween _breathTween;
	protected EyeStatesEnum _prevState;
	protected Quaternion _initialEyeRotation;
	protected Vector3 _initialPosition;
	
	public void SetState(EyeStatesEnum state) {
		State = state;
		InitState(State);
		Eye.SetState(State);
	}
	
	// Use this for initialization
	void Start () {
		_initialEyeRotation = Eye.transform.localRotation;
		_initialPosition = transform.position;
		
		//PersonalSpaceNotifier.TriggerEnter += HandlePersonalSpaceTriggerEnter;
		//PersonalSpaceNotifier.TriggerExit += HandlePersonalSpaceTriggerExit;
		SetState(State);
	}

	void HandlePersonalSpaceTriggerEnter (Collider collider) {
		if (collider.gameObject.layer == LayerMask.NameToLayer("Player")) {
			ExecutePersonalSpaceReactions(collider);
		}
	}
	
	void HandlePersonalSpaceTriggerExit (Collider collider) {
		if (collider.gameObject.layer == LayerMask.NameToLayer("Player")) {
			Debug.Log("trigger exit");
			//move back to original position
			MoveTo(_initialPosition);
		}
	}
	
	public void MoveTo(Vector3 position, VoidDelegate onArrival = null) {
		Tween.toWithSpeed(transform.position, position, 3f, Tween.EaseType.easeInOutSine,
			(Tween tween) => { transform.position = (Vector3)tween.Value; }, 
			(Tween tween) => {
				if (onArrival != null)
					onArrival();
			}
		);
	}
	
	protected void ExecutePersonalSpaceReactions(Collider collider) {
		foreach(ReactionTypesEnum reaction in PersonalSpaceReactions){
			InitPersonalSpaceReaction(reaction);
		}	
	}
	
	protected void InitPersonalSpaceReaction(ReactionTypesEnum reaction) {
		switch(reaction) {
			case ReactionTypesEnum.GREET:
				Greet();
				break;
			case ReactionTypesEnum.FLEE:
				Flee();
				break;
		}
	}
	
	protected void Greet() {
		Vector3 eyeHeightPosition = new Vector3(transform.position.x, PlayerTransform.position.y, transform.position.z);
		MoveTo(eyeHeightPosition, SayHello);
	}
	
	protected void Flee() {
		Vector3 fleeOffset = (transform.position - PlayerTransform.position).normalized * 5.0f;
		Vector3 movePos = transform.position + fleeOffset;
		movePos = new Vector3(movePos.x, 4.0f, movePos.z);
		MoveTo(movePos);
	}
		
	// Update is called once per frame
	void Update () {
		UpdateEyeState();
	}
	
	protected void InitState(EyeStatesEnum state) {
		switch(state){
		case EyeStatesEnum.HAPPY:
			Hover();
			Breath();
			//HelloInterval();
			break;
		case EyeStatesEnum.DRUGGED:
			Hover();
			Breath();
			StartSearchInterval();
			
			if (FacePlayerSporadically)
				StartFacePlayerInterval();
			break;
		default:
			Hover();
			break;
		}
	}
	
	protected void Hover() {
		float delay = -HoverYoyoInterval / Random.Range(1.0f, 3.0f);
		_hoverTween = Tween.to(ContainerTransform.localPosition, ContainerTransform.localPosition + new Vector3(0f, HoverDistance, 0.0f), HoverYoyoInterval, Tween.EaseType.easeInOutBack,
		                       (Tween tween) => {ContainerTransform.localPosition = (Vector3)tween.Value;},null, delay, true, -1);
	}
	
	protected void Breath() {
		float delay = -BreathYoyoInterval / Random.Range(1f, 3f);
		_breathTween = Tween.to(BodyTransform.localScale, BodyTransform.localScale + new Vector3(0.09f, 0.09f, 0.09f), BreathYoyoInterval, Tween.EaseType.easeInOutSine,
		                        (Tween tween) => {BodyTransform.localScale = (Vector3)tween.Value;}, null, delay, true, -1);
	}
	
	protected void HelloInterval() {
		float delay = SayHelloInterval + Random.Range(1.0f, SayHelloSpreadInterval);
		Tween.delayedCall(delay, SayHello);
	} 
	
	protected void StartSearchInterval() {
		float delay = SearchInterval + Random.Range(1.0f, SearchSpreadInterval);
		Tween.delayedCall(delay, RandomLook);
	}
	
	protected void RandomLook() {
	
		float axisChance = Random.Range(0f,1.0f);
		Quaternion lookDirection;
		
		if (axisChance < 0.5f) {
			lookDirection = Quaternion.Euler(Vector3.up * Random.Range(-30f,30f));
		} else {
			lookDirection = Quaternion.Euler(Vector3.forward * Random.Range(-30f,30f));
		}
			
		Tween.to(Eye.transform.localRotation, _initialEyeRotation * lookDirection, 0.5f, Tween.EaseType.easeInOutCubic,
		         (Tween tween) => {Eye.transform.localRotation = (Quaternion)tween.Value;}, 
				 (Tween tween) => {StartSearchInterval();}, 
		        0f, false, 0);
	}
	
	public void SayHello() {
		Tween.to(MouthTransform.localScale, MouthTransform.localScale + new Vector3(0.3f, 0f, 0.5f), 0.25f, Tween.EaseType.easeInOutSine,
		         (Tween tween) => {MouthTransform.localScale = (Vector3)tween.Value;}, null, 0f, true, 1);
		                           
		HelloSound.Play();
		HelloInterval();
	}
	
	protected void StartFacePlayerInterval() {
		float delay = MoveInPlaceInterval + Random.Range(1.0f, MoveInPlaceSpreadInterval);
		Tween.delayedCall(delay, FacePlayer);
	}
	
	protected void FacePlayer() {	
		Quaternion rot = Quaternion.LookRotation(PlayerTransform.position - transform.position);		
		Tween.to(transform.rotation, rot, 0.5f, Tween.EaseType.easeInOutCubic,
		         (Tween tween) => {transform.rotation = (Quaternion)tween.Value;}, null, 0f, false, 0);
		         
		StartFacePlayerInterval();
	}
	
	protected void UpdateEyeState() {
		if (LookAtPlayerContinuous) {
			Quaternion rot = Quaternion.LookRotation(PlayerTransform.position - transform.position);
			transform.rotation = rot;
		}
	}
	
}
