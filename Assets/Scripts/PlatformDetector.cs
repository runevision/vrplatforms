using UnityEngine;
using System.Collections;

public class PlatformDetector : MonoBehaviour {

    public GameObject playerHead;

    private GameObject oldPlatform;
    
    // Update is called once per frame
    void Update () {
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
            if (oldPlatform)
            {
                Platform oldPlatformComp = oldPlatform.transform.GetComponentInParent<Platform>();
                if (oldPlatformComp)
                    oldPlatformComp.StepOff();
            }
            
            oldPlatform = newPlatform;
        }
        else if (oldPlatform != null) {
            OnTrigger.instance.Die ();
            enabled = false;
        }
    }
}
