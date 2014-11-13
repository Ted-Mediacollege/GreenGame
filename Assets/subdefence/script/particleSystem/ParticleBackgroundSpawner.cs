using UnityEngine;
using System.Collections;

public class ParticleBackgroundSpawner : MonoBehaviour {

	[SerializeField]
	private GameObject[] particles;

	private GameObject bubbleHolder;
	private float spawn;
	private float spawning;

	void Start () {
		spawn = 1F;
		spawning = Random.Range(0, 1000);
	}

	void Update () {
		spawn -= Time.deltaTime;
		spawning += Time.deltaTime;
		if(spawning > 1000) { spawning -= 1000; }

		if(spawn < 0F)
		{
			if(Mathf.PerlinNoise(0, spawning / 2F) > 0.5F) {
				spawnParticle();
			}
			spawn = 0.4F;
		}
	}

	void spawnParticle() {
		GameObject buble = GameObject.Instantiate(particles[0], transform.position, new Quaternion(0,0,45,0)) as GameObject;
		buble.transform.parent = gameObject.transform;
	}
	
	/*
	void spawnParticle() {
		Quaternion partRotation = new Quaternion();
		partRotation.eulerAngles = new Vector3(0,0,45);
		GameObject buble = GameObject.Instantiate(particles[0],transform.position,partRotation) as GameObject;
		buble.transform.parent = bubbleHolder.transform;


		float angle;
		Quaternion partRotation = new Quaternion();
		Vector3 partSpawn = spawnPoint.position;
		partSpawn.y += Random.Range(-(spawnDisplacement/2),(spawnDisplacement/2));
		angle = -45;
		partRotation.eulerAngles = new Vector3(0,0,angle);
		GameObject buble = GameObject.Instantiate(particles[0],partSpawn,partRotation) as GameObject;
		buble.transform.parent = bubbleHolder.transform;
	}*/
}
