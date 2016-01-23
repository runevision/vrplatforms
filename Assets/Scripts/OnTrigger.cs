using UnityEngine;
using System.Collections;

public class OnTrigger : MonoBehaviour {

    void OnCollisionEnter()
    {
        float hoverForce = 1.0f;
        Debug.Log("Trigger enter");
        this.GetComponent<Rigidbody>().isKinematic = false;
        this.GetComponent<Rigidbody>().useGravity = true;
        Physics.gravity = new Vector3(0, -0.7f, 0);
        //GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);

        Renderer rend = transform.Find("inverted_cube").GetComponent<Renderer>();
        rend.enabled = true;
        transform.Find("inverted_cube").GetComponent<Fade>().StartFade();

        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();

        RigMover.instance.SetPlatform(null);

    }
}
