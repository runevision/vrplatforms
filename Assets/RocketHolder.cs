using UnityEngine;
using System.Collections;
using Rucrede;
using UnityEngine.Events;

public class RocketHolder : MonoBehaviour {

	public Transform MovePosition;
	public float MoveDownDuration = 2.0f;
	public UnityEvent WhenRocketHolderInPlace;

	protected AudioSource _moveDownAudio;

	// Use this for initialization
	void Start () {
		_moveDownAudio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void MoveDown() {
		if (MovePosition != null) {
			if (_moveDownAudio)
				_moveDownAudio.Play();
			
			Tween.to(transform.position, MovePosition.position, MoveDownDuration, Tween.EaseType.easeInOutSine,
				(Tween t) => {transform.position = (Vector3) t.Value;},
				(Tween t) => { RocketHolderInPlace();}
			);
		}
	}
	
	protected void RocketHolderInPlace() {
		//Rocket Holder in Place:
		
		if (WhenRocketHolderInPlace != null)
			WhenRocketHolderInPlace.Invoke();
	}
	
}
