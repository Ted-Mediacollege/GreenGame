using UnityEngine;
using System.Collections;

public class Mine : Item {
	
	[SerializeField]private float pushForce = 5;
	[SerializeField]private float damage = 5;
	[SerializeField]private float explosionRange = 2;
	[SerializeField]private bool drawExplosionRange = false;
	[SerializeField]private GameObject[] explosion;
	
	public int sellPrice = 30;
	public int buyPrice = 50;
	
	private void Start(){
		rigidbody2D.AddForce(movement.ForceAndAngleToDirection(pushForce,transform.rotation.eulerAngles.z));
	}
	
	//private void OnTriggerEnter2D(Collider2D col){
	private void OnCollisionEnter2D(Collision2D col){
		if(col.gameObject.tag!=gameObject.tag 
		   && col.gameObject.tag!="tower"
		   && col.gameObject.name!="player" ){
			if(col.gameObject.tag=="enemy"){
				Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position,explosionRange);
				//draw range
				if(drawExplosionRange){
					Debug.DrawLine(transform.position,new Vector3(transform.position.x+explosionRange,transform.position.y,0));
				}
				
				for(int i = 0;i<hits.Length;i++){
					if(hits[i].tag=="enemy"){
						float distHit = Vector3.Distance( hits[i].transform.position,transform.position);
						float distDamage = (damage*((explosionRange-distHit)/explosionRange));
						int finalDamage = (int) Mathf.Round( distDamage);
						if(finalDamage>0){
							hits[i].GetComponent<Enemy>().Hit(finalDamage);
							//Debug.Log(finalDamage);
						}
					}
				}
				for(int i = 0 ; i < explosion.Length;i++){
					GameObject.Instantiate(explosion[i],transform.position,Quaternion.identity);
				}
				
				GameObject.Destroy(gameObject);
			}
		}
	}
}