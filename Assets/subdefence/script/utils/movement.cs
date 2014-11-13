using UnityEngine;
using System.Collections;

static class movement {

	static public float RotateForce(Vector3 selfPos, 
	                                float selfAng,
	                                Vector3 target,
	                                float maxRotateForce,
	                                float forceMultiply){
		float thisRotation =  selfAng;
		float deltaY = selfPos.y - target.y;
		float deltaX = selfPos.x - target.x;
		float rotationGoal = (Mathf.Atan2(deltaY,deltaX) * 180 / Mathf.PI)+90;
		float deltaAngel = thisRotation - rotationGoal;
		if(deltaAngel>180){
			deltaAngel -=360;
		}
		
		if(deltaAngel<-180){
			deltaAngel +=360;
		}
		float force = deltaAngel * forceMultiply;
		if(force>maxRotateForce){
			force = maxRotateForce;
		}else if(force<-maxRotateForce){
			force = -maxRotateForce;
		}else{
		}
		return -force;
	}

	static public float limitTorque(float torque, float limit){
		if(torque > limit){
			torque = limit;
		}else if(torque < -limit){
			torque = -limit;
		}
		return torque;
	}

	static public float angleToPoint(Transform selfTrans, Vector3 target){
		float deltaY = selfTrans.position.y - target.y;
		float deltaX = selfTrans.position.x - target.x;
		float angle = (Mathf.Atan2(deltaY,deltaX) * 180 / Mathf.PI)+90;
		return angle;
	}

	static public Quaternion RotateToPoint( Transform selfTrans, Vector3 point ){
		return  Quaternion.Euler(new Vector3(0, 0, movement.angleToPoint(selfTrans,point)));
	}

	static public Vector2 ForceAndAngleToDirection(float force,float angle){
		float xForce = force * Mathf.Sin(angle*Mathf.PI/180);
		float yForce = force * Mathf.Cos(angle*Mathf.PI/180);
		return new Vector2(-xForce,yForce);
	}
	static public Vector2 AngleToDirection(float angle){
		float dx = Mathf.Sin(angle*Mathf.PI/180);
		float dy = Mathf.Cos(angle*Mathf.PI/180);
		return new Vector2(-dx,dy);
	}
	static public float DirectionToAngle(Vector2 direction){
		float angle = (Mathf.Atan2(direction.x,direction.y)* 180.0f / Mathf.PI)-90.0f;
		return angle;
	}
}
