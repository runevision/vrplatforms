using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class RocketLockMechanismController : MonoBehaviour {

	public int Locks = 4;
	public UnityEvent OnLocksRemoved;
	
	protected int _unlocked = 0;
	

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void LockRemoved() {
		_unlocked++;
		if (_unlocked == Locks) {
			if (OnLocksRemoved != null)
				OnLocksRemoved.Invoke();
		}
	}
}
