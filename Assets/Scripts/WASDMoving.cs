using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WASDMoving : MonoBehaviour
{
		// Start is called before the first frame update
	CharacterController controller;

	public float jumpSpeed = 8.0F;
	public float gravity = 20.0F;
	//private Vector3 moveDirection = ;

	void Start ()
	{
		
	}

		// Update is called once per frame
	void Update()
    {
		controller = GetComponent<CharacterController> ();
		if (Input.GetKey (KeyCode.A))
			controller.Move (Vector3.left * Time.deltaTime * 8);
		if (Input.GetKey (KeyCode.D))
			controller.Move (Vector3.right * Time.deltaTime * 8);
		if (Input.GetKey (KeyCode.W))
			controller.Move (Vector3.forward * Time.deltaTime * 8);
		if (Input.GetKey (KeyCode.S))
			controller.Move (Vector3.back * Time.deltaTime * 8);
	}
}
