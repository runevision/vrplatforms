using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class OnTrigger : MonoBehaviour {

	public static OnTrigger instance;

	bool win = false;
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

    public void Win () {
        win = true;
    }

    public void Die () {
    	if (dead || win)
    		return;

    	dead = true;

        RigMover.instance.SetPlatform(null);

        Transform rig = transform.parent.transform;
        rig.GetComponent<Fall>().StartFalling();

        FadeAndRestart();
    }

    public void FadeAndRestart () {
        Renderer rend = transform.Find("inverted_cube").GetComponent<Renderer>();
        rend.enabled = true;
        rend.material.color = Vector4.zero;
        transform.Find("inverted_cube").GetComponent<Fade>().StartFade();

        Invoke("Reload", 3);
    }

    void Reload () {
        SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
    }
}
