using UnityEngine;
using System.Collections;

public class TowerMachineGun : Tower {
	[SerializeField]
	private int targetAimAstimationPresition = 20;

	public override void Rotate(){
		if(enemyMngr.enemies.Count>0){
			if(distCurrentTarget < agroDistance && targetFound){
				
				//first estimate target position
				Vector3 displace = movement.AngleToDirection(gun.transform.eulerAngles.z)*BulletSpawnDCenter;
				Vector3 thisPos = gun.transform.position;
				Vector3 spawnPoint = new Vector3(thisPos.x+(displace.x*0.2f),thisPos.y+(displace.y*0.2f),0);
				
				float bulSpeed = bullet.GetComponent<Bullet>().speed;
				float enemySpeed = currentTarget.transform.rigidbody2D.velocity.magnitude / 50; //enemy speed per fixed update
				Vector2 enemyDirection = movement.AngleToDirection( currentTarget.transform.eulerAngles.z);
				
				float distance = Vector2.Distance(currentTargetPos,spawnPoint);
				float tBullet = distance / bulSpeed;
				float sEnemy = enemySpeed * tBullet;
				
				Vector2 estimatedTargetPos = new Vector2(currentTargetPos.x + enemyDirection.x*sEnemy
				                                            ,currentTargetPos.y + enemyDirection.y*sEnemy);
				                                                                                  
				//better estimate target position
				int i;
				for(i = 0;i<targetAimAstimationPresition;i++){
					distance = Vector2.Distance(estimatedTargetPos,spawnPoint);
					tBullet = distance / bulSpeed;
					sEnemy = enemySpeed * tBullet;
					
					estimatedTargetPos = new Vector2(currentTargetPos.x + enemyDirection.x*sEnemy
					                                 ,currentTargetPos.y + enemyDirection.y*sEnemy);
				}
				                                                        
				currentTargetPos = estimatedTargetPos;

				gun.rigidbody2D.AddTorque(movement.RotateForce(gun.transform.position,
				                                               gun.transform.eulerAngles.z,
				                                               currentTargetPos,
				                                               maxRotForce,
				                                               10));
			}
		}
	}
}
