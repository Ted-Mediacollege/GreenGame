using UnityEngine;

namespace GreenLight{
	public class Damageable : MonoBehaviour{
		
		[SerializeField]
		private GameObject[] shotSpawnObjects;
		
		virtual public void Shot(Vector3 position, Vector3 normal){
			Debug.Log("shot!!!!!!!!!!");
			for(int i = 0 ; i < shotSpawnObjects.Length;i++){
				GameObject.Instantiate(shotSpawnObjects[i],position,Quaternion.LookRotation(normal));
			}
		}
	}
}

