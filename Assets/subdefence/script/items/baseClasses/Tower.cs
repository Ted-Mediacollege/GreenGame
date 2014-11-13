using UnityEngine;
using System.Collections;

public class Tower : Item {
	protected EnemyManager enemyMngr;

	public float maxAngularVelocity = 300;
	public float maxRotForce = 10000;
	public float agroDistance = 5;
	public GameObject bullet;
	private int shootTimer = 0;
	public int shootTime = 10;
	private GameObject bulletHolder;
	private bool aimingAtTarget;

	protected float distCurrentTarget;
	protected GameObject currentTarget;
	protected Vector3 currentTargetPos;
	protected bool targetFound = false;
	
	[SerializeField]
	protected float BulletSpawnDCenter;

	public GameObject gun;

	public int sellPrice = 30;
	public int buyPrice = 50;

	private Animator gunAnimCtrl;
	private Animator animCtrl;

	internal enum BuildPhase{
		Building,
		Done,
		DeBuilding
	}

	internal BuildPhase buildPhase;

	void Start () {
		enemyMngr = GameObject.Find("gameManager").GetComponent<EnemyManager>() as EnemyManager;

		bulletHolder = GameObject.Find("bullets");

		Transform gnT = gun.transform.FindChild("gunArt");
		if(gnT!=null){
			GameObject gn = gnT.gameObject;
			gunAnimCtrl = gun.transform.FindChild("gunArt").GetComponent<Animator>();
		}
		animCtrl = transform.GetComponent<Animator>();
		if(animCtrl!=null){
			animCtrl.SetTrigger("build");
			buildPhase = BuildPhase.Building;
			gun.SetActive(false);
		}
	}

	private void FixedUpdate(){
		if(buildPhase == BuildPhase.Done){
			targetFound = false;
			GetTarget();
			Rotate();
			checkTargetAim();
			Shoot();
		}
	}

	public void BuildDone(){
		buildPhase = BuildPhase.Done;
		gun.SetActive(true);
	}

	public void DeBuild(){
		buildPhase = BuildPhase.DeBuilding;
		gun.SetActive(false);
		animCtrl.SetTrigger("deBuild");
	}

	public void DeBuildDone(){
		GameObject.Find("gameManager").GetComponent<TowerManager>().SellTower(gameObject);
	}

	private void GetTarget(){
		//get targer
		if(enemyMngr.enemies.Count>0){
			currentTargetPos = enemyMngr.enemies[0].transform.position;
			distCurrentTarget = Vector3.Distance(transform.position,currentTargetPos);
			currentTarget = enemyMngr.enemies[0];
			if(distCurrentTarget<agroDistance){
				targetFound = true;
			}else{
				for (int i = 0; i < enemyMngr.enemies.Count; i++){
					distCurrentTarget = Vector3.Distance(gun.transform.position,currentTargetPos);
					float distNewTarget = Vector3.Distance(gun.transform.position,enemyMngr.enemies[i].transform.position);
					if(distNewTarget<distCurrentTarget){
						currentTarget = enemyMngr.enemies[i];
						currentTargetPos = currentTarget.transform.position;
					}
					if(distCurrentTarget<agroDistance){
						targetFound = true;
						break;
					}
				}
			}
		}
		if(targetFound){
			if(Vector3.Distance(transform.position,currentTargetPos) > agroDistance){
				targetFound = false;
			}
		}
	}

	public void GunAnimate(){
		if(gunAnimCtrl!=null){
			gunAnimCtrl.SetTrigger("shoot");
		}
	}

	public virtual void Rotate(){
		if(enemyMngr.enemies.Count>0){
			if(distCurrentTarget < agroDistance && targetFound){
				gun.rigidbody2D.AddTorque(movement.RotateForce(gun.transform.position,
				                                               gun.transform.eulerAngles.z,
				                                               currentTargetPos,
				                                               maxRotForce,
				                                               10));
			}
		}
		//limit force
		gun.rigidbody2D.angularVelocity = movement.limitTorque(gun.rigidbody2D.angularVelocity,maxAngularVelocity);
	}

	private void checkTargetAim(){
		if(targetFound){
			//transform.rotation =  Quaternion.Euler(new Vector3(0, 0, movement.RotateToPoint(transform,playerTrance.position)));
			float rotatioGloal = movement.RotateToPoint(transform,currentTargetPos).eulerAngles.z;
			float deltaRotation = Mathf.Abs( rotatioGloal - gun.transform.rotation.eulerAngles.z);
			if(deltaRotation<5){
				aimingAtTarget = true;
			}else{
				aimingAtTarget = false;
			}
		}else{
			aimingAtTarget = false;
		}
	}

	private void Shoot(){
		if(shootTimer>0){
			shootTimer--;
		}

		//GameObject.Instantiate 
		if (shootTimer==0&&aimingAtTarget){
			shootTimer = shootTime;

			Vector3 displace = movement.AngleToDirection(gun.transform.eulerAngles.z)*BulletSpawnDCenter;
			Vector3 thisPos = gun.transform.position;
			Vector3 spawnPoint = new Vector3(thisPos.x+(displace.x*0.2f),thisPos.y+(displace.y*0.2f),0);

			GameObject bul = GameObject.Instantiate( bullet
			                                        ,spawnPoint 
			                                        ,gun.transform.rotation) as GameObject;
			bul.transform.parent = bulletHolder.transform;

			GunAnimate();
		}
	}
}



