using UnityEngine;
using System.Collections;

namespace GreenLight{
	public class Gun : MonoBehaviour
	{
		[SerializeField]
		private Transform gunPoint;
		[SerializeField]
		private float shootDist = 5;
		[SerializeField]
		private float randDirection = 0.3f;// between 0 and 1
		[SerializeField]
		private int bullets = 1;
		[SerializeField]
		private LayerMask gunHitLayer;
		
		//private LineRenderer[] lines;
	
		void Update ()
		{
			if(Input.GetMouseButtonDown(0)){
				for(int i = 0;i < bullets;i++){
					Quaternion gunRotation = gunPoint.transform.rotation;
					Vector3 gunRotEulers = gunRotation.eulerAngles;
					gunRotEulers = new Vector3(gunRotEulers.x + Random.Range((-randDirection/2),(randDirection/2)),
					                           gunRotEulers.y + Random.Range((-randDirection/2),(randDirection/2)),
					                           gunRotEulers.z + Random.Range((-randDirection/2),(randDirection/2)));;
					gunRotation.eulerAngles = gunRotEulers;
					Quaternion bulletRotaion = gunRotation;
					Vector3 shootDir = (bulletRotaion * Vector3.forward * shootDist);
					
					Vector3 shootEndPoint = gunPoint.position + shootDir;
					
					//draw
					//Debug.DrawLine(gunPoint.position,shootEndPoint,Color.red,10.5f);
					//Debug.DrawLine((shootEndPoint+new Vector3(0,0.02f,0)),shootEndPoint,Color.green,10.5f);
					
					//linecast
					RaycastHit hitData;
					bool hit = Physics.Linecast(gunPoint.position,
									shootEndPoint,
					                out hitData,
									LayerMask.NameToLayer( gunHitLayer.ToString()));
									
					//linerenderes
					GameObject lineHolder = new GameObject("Line");
					
					LineRenderer lineRenderer = lineHolder.AddComponent<LineRenderer>();
					lineHolder.AddComponent<LineFade>();
					lineRenderer.SetPosition(0, gunPoint.position);
					if(hit){
						lineRenderer.SetPosition(1, hitData.point);
					}else{
						lineRenderer.SetPosition(1, shootEndPoint);
					}
					
					//call Shot in damageable objects
						
					if(hit){
						bool hitDamagable = false;	
						Damageable damageable = hitData.collider.gameObject.GetComponent<Damageable>();
						if(damageable != null){
							hitDamagable = true;
							Debug.Log("GUN HIT DAMAGEABLE");
						}else{
							GameObject parent = hitData.collider.transform.parent.gameObject;
							while(parent != null){
								damageable = parent.GetComponent<Damageable>();
								if(damageable != null){
									hitDamagable = true;
									Debug.Log("GUN HIT DAMAGEABLE");
									parent = null;
								}else{
									if(parent.transform.parent == null){
										parent = null;
									}else{
										parent = parent.transform.parent.gameObject;
									}
								}
							}
							Debug.Log("name: "+	hitData.collider.gameObject.name+" damageable: "+damageable);
						}
						if(hitDamagable){
							damageable.Shot(hitData.point,hitData.normal);
						}
					}
				}
			}
			
			
		}
	}
}
