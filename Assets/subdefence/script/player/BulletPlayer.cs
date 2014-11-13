using UnityEngine;
using System.Collections;

public class BulletPlayer : Bullet {

	private void Update(){
		transform.localPosition = transform.position + (velocity*Time.deltaTime);
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
