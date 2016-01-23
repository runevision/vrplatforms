using UnityEngine;
using System.Collections;

public class Fade : MonoBehaviour {

    private bool fade = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if(fade)
        {
            Color color = GetComponent<Renderer>().material.color;
            color.a = Mathf.MoveTowards (color.a, 1, Time.deltaTime);
            GetComponent<Renderer>().material.color = color;
            Debug.Log("Alpha is " + color.a);
        }
	}

    public void StartFade()
    {
        fade = true;
    }
}
