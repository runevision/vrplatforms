using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace Cluster {

public class CollisionCall : MonoBehaviour {
	
	public LayerMask layerMask = -1;

	[System.Serializable]
	public class CollisionEvent : UnityEvent<Collision> {}

	public CollisionEvent onCollisionEnter;
	public CollisionEvent onCollisionExit;
	
	void OnCollisionEnter (Collision col) {
		if ((layerMask.value & 1 << col.gameObject.layer) != 0)
			onCollisionEnter.Invoke (col);
	}

	void OnCollisionExit (Collision col) {
		if ((layerMask.value & 1 << col.gameObject.layer) != 0)
			onCollisionExit.Invoke (col);
	}
}

}
