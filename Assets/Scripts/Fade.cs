using UnityEngine;
using System.Collections;

public class Fade : MonoBehaviour {

    private bool fade = false;
    [Range(0,3)]
    public float duration = 2.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if(fade)
        {
            Color color = GetComponent<Renderer>().material.color;
            color.a += Time.deltaTime/duration;
            GetComponent<Renderer>().material.color = color;
        }
	}

    public void StartFade()
    {
        fade = true;
    }
}
