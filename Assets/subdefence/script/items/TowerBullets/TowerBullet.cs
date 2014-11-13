using UnityEngine;
using System.Collections;

public class TowerBullet : Bullet {
	
	private void FixedUpdate(){
		transform.localPosition = transform.position + velocity;
	}
	
	private void OnTriggerEnter2D(Collider2D col){
		if(col.gameObject.tag!=gameObject.tag 
		   && col.gameObject.tag!="tower"
		   && col.gameObject.name!="player" ){
			if(col.gameObject.tag=="enemy"){
				col.GetComponent<Enemy>().Hit(damage);
			}
			GameObject.Destroy(gameObject);
		}
	}
}