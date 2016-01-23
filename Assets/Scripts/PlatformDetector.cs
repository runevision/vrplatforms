using UnityEngine;
using System.Collections;

public class PlatformDetector : MonoBehaviour {

    public GameObject playerHead;

    private GameObject oldPlatform, newPlatform;
    private bool overPlatform = false;

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
            newPlatform = hit.transform.gameObject;
            Platform platform = newPlatform.transform.GetComponentInParent<Platform>();
            if (platform == null) return;

            // if (!overPlatform || newPlatform != oldPlatform) {

                RigMover.instance.SetPlatform(platform);
                Debug.Log("New platform hit! :O");
                newPlatform.GetComponent<Renderer>().material.color = Color.red;


            //}     
        }
    }
}
