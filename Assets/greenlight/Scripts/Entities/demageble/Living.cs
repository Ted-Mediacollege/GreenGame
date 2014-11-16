using UnityEngine;

namespace GreenLight{
	public class Living : Damageable {
		[SerializeField]
		private GameObject[] deadSpawnObjects;	
	
		private int startHealt;
		private int healt_ = 100;
		private bool dead_ = false;
		
		
		public int healt {
			get { return healt_;} 
		}
		
		public bool dead {
			get { return dead_;} 
		}
		
		virtual internal void Start () {
			startHealt = healt_; 
		}
		
		public void ChangeHealt(int D){
			healt_ += D;
			if(healt<0){
				dead_ = true;
			}
		}
		
		public void Hit(int damage){
			ChangeHealt(-damage);
			if(dead){
				///Dead();
			}
		}
		
		public override void Shot(Vector3 position, Vector3 normal){
			base.Shot(position,normal);
			Hit(1);
		}
		
		private void Dead(bool reachedEnd = false){
			for(int i = 0 ; i < deadSpawnObjects.Length;i++){
				GameObject.Instantiate(deadSpawnObjects[i],transform.position,Quaternion.identity);
			}
			////////enemyMngr.removeEnemy(transform.gameObject,reachedEnd);
		}
		/*private void OnTriggerEnter2D(Collider2D col){
			if((itemType)col.gameObject.GetComponent<Item>().type == itemType.Bullet){
				healt.ChangeHealt(-10);
				if(healt.dead){
					enemyMngr.removeEnemy(transform.gameObject);
				}
			}
		}*/
	}
}