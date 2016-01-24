using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class OnTrigger : MonoBehaviour {

	public static OnTrigger instance;

	bool dead = false;

	void OnEnable () {
		instance = this;
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Death"))
            return;
        
        Debug.Log("Collision enter");

		AudioSource audio = GetComponent<AudioSource>();
        audio.Play();

        Die ();
    }

    public void Die () {
    	if (dead)
    		return;

    	dead = true;

		Invoke("Reload", 3);

		Renderer rend = transform.Find("inverted_cube").GetComponent<Renderer>();
        rend.enabled = true;
        rend.material.color = Vector4.zero;
        transform.Find("inverted_cube").GetComponent<Fade>().StartFade();

        RigMover.instance.SetPlatform(null);

        Transform rig = transform.parent.transform;
        rig.GetComponent<Fall>().StartFalling();
    }

    void Reload () {
        SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
    }
}
