using UnityEngine;
using System.Collections;

public class Player2 : MonoBehaviour {
	public float maxAngularVelocity = 10;
	public float drag = 0.9f;
	public float dashForce = 300;
	public float moveForce = 25;
	public float rotateForce = 3;

	[SerializeField]
	private GameObject gun;

	private void FixedUpdate () {
		Move();
		Rotate();
	}

	private void Update(){
		//dash
		bool jump = Input.GetButtonDown("Jump");
		if(jump){
			rigidbody2D.AddForce( movement.ForceAndAngleToDirection(dashForce,transform.rotation.eulerAngles.z));
		}
		
	}

	private void Rotate(){
		//get input
		float hor = -Input.GetAxis("Horizontal");
		//add force
		rigidbody2D.AddTorque(hor*rotateForce);
		//limit force
		rigidbody2D.angularVelocity = movement.limitTorque(rigidbody2D.angularVelocity,maxAngularVelocity);
	}

	private void Move(){
		//move
		bool upKeyDown;
		if(Input.GetKey(KeyCode.UpArrow)||Input.GetKey(KeyCode.W)){
			upKeyDown = true;
		}else{
			upKeyDown = false;
		}
		Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		Vector3 mousePos = new Vector3(mouseRay.origin.x,mouseRay.origin.y,0);
		if(upKeyDown){
			float angle = movement.RotateToPoint(transform,mousePos).eulerAngles.z;
			rigidbody2D.AddForce( movement.ForceAndAngleToDirection(moveForce,angle));
		}
		//drag
		float newX = 0;
		float newY = 0;
		if(rigidbody2D.velocity.x!=0){
			newX = rigidbody2D.velocity.x*drag;
		}else{
			newX = rigidbody2D.velocity.x;
		}
		if(rigidbody2D.velocity.y!=0){
			newY= rigidbody2D.velocity.y*drag;
		}else{
			newY= rigidbody2D.velocity.y;
		}
		rigidbody2D.velocity = new Vector2(newX,newY);
	}
}
