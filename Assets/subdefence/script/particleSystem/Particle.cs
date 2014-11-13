using UnityEngine;
using System.Collections;

public class Particle : MonoBehaviour {
	
	private SpriteRenderer img;
	private float Scale;
	public float scaleX = 0.1f;
	public float scaleY = 0.1f;
	public float randomScale = 0.3f;
	private float alpha = 0;
	private float xFlip = 1;
	
	public float[] Delta = {0.1f,-0.11f};
	private int state = 0;
	private float deltaAlpha;
	private float speed;
	private float startSpeed;
	private float deltaSpeed = 1.5f;

	[SerializeField]
	private float waveSpeed = 0.005f;
	
	private void Start () {
		img = GetComponent<SpriteRenderer>();
		img.color = new Color(1f,1f,1f,alpha);
		float randomS = Random.Range(-(randomScale/2),(randomScale/2));
		scaleX+=randomS;
		scaleY+=randomS;
		transform.localScale = new Vector3(scaleX,scaleY,1);
		deltaAlpha = Delta[state]/Scale;
		speed = 0.02f;
		startSpeed = speed;
	}
	
	private void FixedUpdate () {
		if(speed>0){
			speed -= (0.02f/(90/deltaSpeed));
		}
		float angleDelta;
		if(transform.eulerAngles.z>90){
			angleDelta=deltaSpeed;
		}else if(transform.eulerAngles.z<90){
			angleDelta=-deltaSpeed;
		}else{
			angleDelta=0;
		}
		transform.eulerAngles = new Vector3(0,0,transform.eulerAngles.z+angleDelta);

		float angle = transform.eulerAngles.z;
		Vector2 move = movement.ForceAndAngleToDirection(speed,angle);

		transform.Translate(move.x,move.y,0);

		if(state==0 && scaleX>Scale ){
			state++;
			deltaAlpha = Delta[state]/Scale;
		}
		else if(scaleX<0){
			Destroy(gameObject);
		}
		scaleX += Delta[state];
		scaleY += Delta[state];
		alpha += deltaAlpha;
		transform.localScale = new Vector3(scaleX,scaleY,1);
		img.color = new Color(1f,1f,1f,1f);

		SinMove();
	}
	float waveAngle = 0;
	void SinMove(){
		waveAngle+=30;
		if(waveAngle>360){
			waveAngle = 0;
		}
		Vector2 sinPos = movement.AngleToDirection(waveAngle)*waveSpeed;
		transform.Translate(sinPos.x,sinPos.y,0);
	}

}
