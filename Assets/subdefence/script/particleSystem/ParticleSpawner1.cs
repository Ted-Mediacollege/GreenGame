using UnityEngine;
using System.Collections;

public class ParticleSpawner1 : MonoBehaviour {

	[SerializeField]
	private GameObject[] particles;
	[SerializeField]
	private Transform spawnPoint;
	[SerializeField]
	private float spawnTime = 3;
	[SerializeField]
	private float spawnTimeRandom = 1;
	[SerializeField]
	private float spawnPerTimer = 3;
	[SerializeField]
	private float spawnDisplacement = 1;




	private float spawnT;

	private GameObject bubbleHolder;

	void Start(){
		spawnT = (spawnTime +Random.Range(spawnTimeRandom/2.0f,-spawnTimeRandom/2.0f))*rigidbody2D.velocity.magnitude;
		bubbleHolder = GameObject.Find("temp objects");
	}

	void Update () {
		spawnT-=Time.deltaTime;
		if(rigidbody2D.velocity.magnitude>1){
			if(spawnT<0){
				spawnT = (spawnTime +Random.Range(spawnTimeRandom/2.0f,-spawnTimeRandom/2.0f));
				Spawn();
			}
		}else{
			if(spawnT<0){
				spawnT = ((spawnTime +Random.Range(spawnTimeRandom/2.0f,-spawnTimeRandom/2.0f))*20);
				Spawn();
			}
		}
	}

	void Spawn(){
		for(int i = 0; i < spawnPerTimer;i++){
			float angle;
			Quaternion partRotation = new Quaternion();
			Vector3 partSpawn = spawnPoint.position;
			partSpawn.y += Random.Range(-(spawnDisplacement/2),(spawnDisplacement/2));
			if(transform.localScale.x>0){
				angle = -45;
			}else{
				angle = 45;
			}
			partRotation.eulerAngles = new Vector3(0,0,angle);
			GameObject buble = GameObject.Instantiate(particles[0],partSpawn,partRotation) as GameObject;
			buble.transform.parent = bubbleHolder.transform;
		}
	}
}
