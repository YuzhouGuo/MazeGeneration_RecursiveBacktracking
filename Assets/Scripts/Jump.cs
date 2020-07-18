using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
		// declare the controller here
		CharacterController controller;

		public float jumpSpeed = 8.0F;
		public float gravity = 20.0F;
		private Vector3 moveDirection = Vector3.zero;

		void Start ()
		{
				controller = GetComponent<CharacterController> ();
		}

		void Update ()
		{
				// update the controller(the player) every frame
				if (controller.isGrounded && Input.GetKeyDown (KeyCode.Space)) {
						// apply the speed to be two times the jumpSpeed
						moveDirection.y = jumpSpeed*2;
				}
				moveDirection.y -= gravity * Time.deltaTime;
				controller.Move (moveDirection * Time.deltaTime);
		}

}
