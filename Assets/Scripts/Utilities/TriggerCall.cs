using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace Cluster {

public class TriggerCall : MonoBehaviour {

	public LayerMask layerMask = -1;

	[System.Serializable]
	public class TriggerEvent : UnityEvent<Collider> {}

	public TriggerEvent onTriggerEnter = new TriggerEvent ();
	public TriggerEvent onTriggerExit = new TriggerEvent ();
	
	void OnTriggerEnter (Collider col) {
		if ((layerMask.value & 1 << col.gameObject.layer) != 0)
			onTriggerEnter.Invoke (col);
	}

	void OnTriggerExit (Collider col) {
		if ((layerMask.value & 1 << col.gameObject.layer) != 0)
			onTriggerExit.Invoke (col);
	}
}

}
