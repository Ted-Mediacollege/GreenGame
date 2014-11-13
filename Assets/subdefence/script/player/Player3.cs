using UnityEngine;
using System.Collections;

public class Player3 : MonoBehaviour {
	public float maxAngularVelocity = 10;
	public float drag = 0.9f;
	public float dashForce = 300;
	public float moveForce = 25;
	public float rotateForce = 3;
	
	private void FixedUpdate () {
		Move();
	}
	
	private void Update(){
		bool jump = Input.GetButtonDown("Jump");
		if(jump){
			rigidbody2D.AddForce( movement.ForceAndAngleToDirection(dashForce,transform.rotation.eulerAngles.z));
		}
	}

	private float fixPos(float p) {
		if(p != 0){
			return p * drag;
		}else{
			return p;
		}
	}
	
	private void Move(){
		float newX = 0, newY = 0;

		if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) {
			rigidbody2D.AddForce(new Vector3(0, moveForce, 0));
		}
		if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) {
			rigidbody2D.AddForce(new Vector3(-moveForce, 0, 0));
		}
		if(Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) {
			rigidbody2D.AddForce(new Vector3(0, -moveForce, 0));
		}
		if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
			rigidbody2D.AddForce(new Vector3(moveForce, 0, 0));
		}

		newX = fixPos(rigidbody2D.velocity.x);
		newY = fixPos(rigidbody2D.velocity.y);
		
		if(newX > 0.1) {
			Vector3 scale = transform.localScale;
			scale.x = -1;
			transform.localScale = scale;
		} else if(newX < -0.1) {
			Vector3 scale = transform.localScale;
			scale.x = 1;
			transform.localScale = scale;
		}

		rigidbody2D.velocity = new Vector2(newX, newY);
	}
}
