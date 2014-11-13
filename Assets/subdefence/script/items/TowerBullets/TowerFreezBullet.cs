using UnityEngine;
using System.Collections;

public class TowerFreezBullet : Bullet {
	
	[SerializeField]
	private float explosionRange = 2;
	
	[SerializeField]
	private bool drawExplosionRange = false;
	
	private void FixedUpdate(){
		transform.localPosition = transform.position + velocity;
	}
	
	private void OnTriggerEnter2D(Collider2D col){
		if(col.gameObject.tag!=gameObject.tag 
		   && col.gameObject.tag!="tower"
		   && col.gameObject.name!="player" ){
				Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position,explosionRange);
				//draw range
				if(drawExplosionRange){
					Debug.DrawLine(transform.position,new Vector3(transform.position.x+explosionRange,transform.position.y,0));
				}
				
				for(int i = 0;i<hits.Length;i++){
					if(hits[i].tag=="enemy"){
						hits[i].GetComponent<Enemy>().FreezHit();
					}
				}
				GameObject.Destroy(gameObject);
		}
	}
}