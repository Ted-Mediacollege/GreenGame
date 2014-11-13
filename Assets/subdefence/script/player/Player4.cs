using UnityEngine;
using System.Collections;

public class Player4 : MonoBehaviour {
	public float maxAngularVelocity = 10;
	public float drag = 0.9f;
	public float dashForce = 300;
	public float moveForce = 25;
	public float rotateForce = 3;
	public int maxRotateAngle = 30;
	
	private bool up,down,left,right;
	
	private void FixedUpdate () {
		GetInpute();
		Move();
		Rotate();
	}
	
	private float fixPos(float p) {
		if(p != 0){
			return p * drag;
		}else{
			return p;
		}
	}
	
	private void GetInpute(){
		if(BuildTypeData.buildType == BuildType.PC){
		    if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) {
			    up = true;
		    }else{
			    up = false;
		    }
		    if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) {
			    left = true;
		    }else{
			    left = false;
		    }
		    if(Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) {
			    down = true;
		    }else{
			    down = false;
		    }
		    if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
			    right = true;
		    }else{
			    right = false;
		    }
		}else if(BuildTypeData.buildType == BuildType.VITA){
		    if(Input.GetAxisRaw("Vertical")>0.9f) {//up
			    up = true;
		    }else{
			    up = false;
		    }
		    /*if(Input.GetKey(KeyCode.JoystickButton9)) {//right
			    right = true;
		    }else{
			    right = false;
		    }*/
		    if(Input.GetAxisRaw("Vertical")<-0.9f) {//down
			    down = true;
		    }else{
			    down = false;
		    }
		    /*if(Input.GetKey(KeyCode.JoystickButton11)) {//left
			    left = true;
		    }else{
			    left = false;
		    }*/
		}
	}
	
	private void Rotate(){
		float rotation = transform.localRotation.eulerAngles.z;
		int xDirection = (int)transform.localScale.x;
		if(up) {
			if(xDirection>0){
				if(rotation>(360-maxRotateAngle)||rotation<180){
					transform.Rotate(0,0,-2*xDirection);
				}
			}else if(rotation<maxRotateAngle||rotation>180){
				transform.Rotate(0,0,-2*xDirection);
			}
		}else if(down) {
			if(xDirection>0){
				if(rotation<maxRotateAngle||rotation>180){
					transform.Rotate(0,0,2*xDirection);
				}
			}else{
				if(rotation>(360-maxRotateAngle)||rotation<180){
					transform.Rotate(0,0,2*xDirection);
				}
			}
		}else{
			if(transform.localRotation.eulerAngles.z>180&&transform.localRotation.eulerAngles.z<358){
				transform.Rotate(0,0,1.9f);
			}else if(transform.localRotation.eulerAngles.z>2&&transform.localRotation.eulerAngles.z<180) {
				transform.Rotate(0,0,-1.9f);
			}
		}
	}
	
	private void Move(){
		float newX = 0, newY = 0;
		/*if(up) {//up
			rigidbody2D.AddForce(new Vector3(0, moveForce, 0));
		}
		if(right) {//right
			rigidbody2D.AddForce(new Vector3(moveForce, 0, 0));
		}
		if(down) {//down
			rigidbody2D.AddForce(new Vector3(0, -moveForce, 0));
		}
		if(left) {//left
			rigidbody2D.AddForce(new Vector3(-moveForce, 0, 0));
		}*/
		rigidbody2D.AddForce(new Vector3(Input.GetAxisRaw("Horizontal")*moveForce, Input.GetAxisRaw("Vertical")*moveForce, 0));
		newX = fixPos(rigidbody2D.velocity.x);
		newY = fixPos(rigidbody2D.velocity.y);
		
		if(newX > 0.1 && transform.localScale.x == 1) {
			Vector3 scale = transform.localScale;
			scale.x = -1;
			transform.localScale = scale;
			float rot = transform.localEulerAngles.z;
			transform.localEulerAngles = new Vector3(0,0,(360-rot));
		} else if(newX < -0.1 && transform.localScale.x == -1) {
			Vector3 scale = transform.localScale;
			scale.x = 1;
			transform.localScale = scale;
			float rot = transform.localEulerAngles.z;
			transform.localEulerAngles = new Vector3(0,0,(360-rot));
		}
		
		rigidbody2D.velocity = new Vector2(newX, newY);
	}
	
	void OnGUI()
	{
		if(BuildTypeData.buildType == BuildType.PC){
		    GUILayout.Box("BuildType PC");
		}else if(BuildTypeData.buildType == BuildType.VITA){
            GUILayout.Box("BuildType VITA");
		}
	}
}


