using UnityEngine;
using System.Collections;

public class Switcher : MonoBehaviour {

	private Camera c;

	// Use this for initialization
	void Start () {
	
		c = Camera.main.GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter (Collider other) {

		//switch to bird's view
		c.enabled = false;
		gameObject.GetComponent<Camera>().enabled = true;
	}

	void OnTriggerExit (Collider other) {

		//switch back to first person view
		gameObject.GetComponent<Camera>().enabled = false;
		c.enabled = true;
	}
}
