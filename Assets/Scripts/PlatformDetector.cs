using UnityEngine;
using System.Collections;

public class PlatformDetector : MonoBehaviour {

    public GameObject playerHead;

    private GameObject oldPlatform;

    void LateUpdate () {
        if (RigMover.instance.transitioning)
            return;

        RaycastHit hit;
        Vector3 rayOrigin = playerHead.transform.position;
        Vector3 rayOrientation = Vector3.down;
        Debug.DrawRay(rayOrigin, rayOrientation, Color.red);

        GameObject newPlatform = null;
        Platform newPlatformComp = null;
        if (Physics.SphereCast(rayOrigin, 0.2f, rayOrientation, out hit)) {
            newPlatform = hit.transform.gameObject;
            if (newPlatform == oldPlatform)
                return;
            
            newPlatformComp = newPlatform.transform.GetComponentInParent<Platform>();
        }

        if (newPlatformComp != null) {
            newPlatformComp.StepOnto ();
            oldPlatform = newPlatform;
        }
        else if (oldPlatform != null) {
            OnTrigger.instance.Die ();
            enabled = false;
        }
    }
}
