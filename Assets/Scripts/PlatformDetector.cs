using UnityEngine;
using System.Collections;

public class PlatformDetector : MonoBehaviour {

    public GameObject playerHead;

    private GameObject oldPlatform;

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        RaycastHit hit;
        Vector3 rayOrigin = playerHead.transform.position;
        Vector3 rayOrientation = Vector3.down;
        Debug.DrawRay(rayOrigin, rayOrientation, Color.red);
        if (Physics.Raycast(rayOrigin, rayOrientation, out hit))
        {
            // If overPlatform == false or newPlatform != oldPlatform, trigger new platform event.
            // 
            GameObject newPlatform = hit.transform.gameObject;
            Platform platform = newPlatform.transform.GetComponentInParent<Platform>();
            if (platform == null) return;

            if (newPlatform != oldPlatform) {
                RigMover.instance.SetPlatform(platform);
                newPlatform.GetComponent<Renderer>().material.color = new Color (1.2f, 1.2f, 1.2f);
                if (oldPlatform) oldPlatform.GetComponent<Renderer>().material.color = Color.white;
                oldPlatform = newPlatform;
                if(platform.onlyOnce)
                {
                    platform.StartMoving();
                }
            }
        }
    }
}
