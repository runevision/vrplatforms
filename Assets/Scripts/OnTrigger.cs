using UnityEngine;
using System.Collections;

public class OnTrigger : MonoBehaviour {

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision enter");

        Renderer rend = transform.Find("inverted_cube").GetComponent<Renderer>();
        rend.enabled = true;
        rend.material.color = Vector4.zero;
        transform.Find("inverted_cube").GetComponent<Fade>().StartFade();

        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();

        RigMover.instance.SetPlatform(null);

        Invoke("Reload", 3);
        Transform rig = transform.parent.transform;
        rig.GetComponent<Fall>().StartFalling();
    }
}
