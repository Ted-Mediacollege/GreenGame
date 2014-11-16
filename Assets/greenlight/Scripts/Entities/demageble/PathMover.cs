using UnityEngine;
using System.Collections;

namespace GreenLight{
	public class PathMover : Living {
		//////////////////////////////
		
		private Vector3[] path;
		private int ProgressInPath = 0;
		private float DistPointReached = 1.5f;
		private Vector3 target;
		private bool pathFound;
		private GreenLight.PathManager pathMngr;
		
		
		public float maxAngularVelocity = 2;
		public float drag = 0.97f;
		public float dashForce = 200;
		public float moveForce = 10;
		public float maxRotateForce = 1;
		
		
		[SerializeField]
		private float freezSlowDown = 0.5f;
		[SerializeField]
		private float deFreezSpeed = 0.005f;
		private float currentFreezSlowDown = 1;
		internal bool freezHit;
		private SpriteRenderer sprite;
		
		public GameObject test;
		
		private void Start () {
			
			GreenLight.LinkHolder linkHolder = GameObject.Find("linkHolder").GetComponent<GreenLight.LinkHolder>();
			pathMngr = linkHolder.patheManager;
			
			getTarget();
			transform.rotation =  GreenLight.Movement.RotateToPoint(transform.position,target);
			sprite = gameObject.GetComponent<SpriteRenderer>();
		}
		
		public void FreezHit(){
			currentFreezSlowDown = freezSlowDown;
			sprite.color = new Color(currentFreezSlowDown,currentFreezSlowDown,1);
		}
		
		private void FixedUpdate(){
			test.transform.position = target;
		
			/*FreezRestore();
			
			Drag();*/
			RotateToPoint();
			
			TargetMove();
		}
		
		private void FreezRestore(){
			if(currentFreezSlowDown<1){
				currentFreezSlowDown += deFreezSpeed;
				sprite.color = new Color(currentFreezSlowDown,currentFreezSlowDown,1);
			}
		}
		
		private void TargetMove(){
			float distanceToTarget = Vector3.Distance(transform.position,target);
			if(pathFound){
				
				if(DistPointReached>distanceToTarget){
					ProgressInPath++;
					if(ProgressInPath>path.Length-1){
						getTarget();
					}else{
						target = path[ProgressInPath];
					}
				}
			}else{
				if(DistPointReached>distanceToTarget){
					// enemy reached end
					/////Dead(true);
				}
			}
			
			rigidbody.AddRelativeForce(new Vector3(0,0,moveForce) );
		}
		
		private void getTarget(){
			path = pathMngr.getRandomPath(transform.position,5);
			ProgressInPath = 0;
			if(path!=null){
				pathFound = true;
				target = path[ProgressInPath];
			}else{
				pathFound = false;
				Debug.Log("no target");
				/////////target = enemyMngr.getClosestEnd(transform.position);
			}
		}
		
		private void RotateToPoint(){
			////////rigidbody.AddTorque(new Vector3());
			//////////limit force
			//////////rigidbody.angularVelocity = GreenLight.Movement.limitTorque(rigidbody.angularVelocity,maxAngularVelocity);
			
			
			Vector3 _direction = (target - transform.position).normalized;
			Quaternion _lookRotation = Quaternion.LookRotation(_direction);
			_lookRotation = Quaternion.Euler(0,_lookRotation.eulerAngles.y,0);
			transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, 0.15f);
			
		}
		
		private void oldRotateToPoint(){
			/*float thisRotation =  transform.eulerAngles.z;
			float deltaY = transform.position.y - target.y;
			float deltaX = transform.position.x - target.x;
			float rotationGoal = (Mathf.Atan2(deltaY,deltaX) * 180 / Mathf.PI)+90;
			//transform.rotation =  Quaternion.Euler(new Vector3(0, 0, angleInDegrees));
			float deltaAngel = thisRotation - rotationGoal;
			//Debug.Log(thisRotation +"-"+rotationGoal+"="+deltaAngel);
			if(deltaAngel>360){
				deltaAngel -=360;
			}
			
			if(deltaAngel>maxRotateForce){
				deltaAngel = maxRotateForce;
			}else if(deltaAngel<maxRotateForce){
				deltaAngel = -maxRotateForce;
			}
			//Debug.Log(deltaAngel);
			//Debug.Log(path[ProgressInPath]);
			//get input
			//add force
			rigidbody.AddTorque(-deltaAngel);
			//limit force
			if(rigidbody.angularVelocity > maxAngularVelocity){
				rigidbody.angularVelocity = maxAngularVelocity;
			}else if(rigidbody.angularVelocity < -maxAngularVelocity){
				rigidbody.angularVelocity = -maxAngularVelocity;
			}*/
		}
		
		private void Drag(){
			/*float newX = 0;
			float newY = 0;
			if(rigidbody.velocity.x!=0){
				newX = rigidbody.velocity.x*drag;
			}else{
				newX = rigidbody.velocity.x;
			}
			if(rigidbody.velocity.y!=0){
				newY= rigidbody.velocity.y*drag;
			}else{
				newY= rigidbody.velocity.y;
			}
			rigidbody.velocity = new Vector2(newX,newY);*/
		}
		
	}
}
