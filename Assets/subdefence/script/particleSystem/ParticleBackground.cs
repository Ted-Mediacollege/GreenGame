using UnityEngine;
using System.Collections;

public class ParticleBackground : MonoBehaviour {

	private float timeLeft;
	private float speed;

	void Start () {
		timeLeft = 5F + Random.Range(0, 2);

		Vector3 pos = transform.position;
		pos.x = pos.x - 0.3F + ((float) Random.Range(0, 6) / 10F);
		pos.y = pos.y - 0.1F + ((float) Random.Range(0, 2) / 10F);
		transform.position = pos;

		speed = 0.7F + ((float) Random.Range(0, 2) / 10F);

		transform.localScale = new Vector3(0.01F * (timeLeft * 2.5F), 0.01F * (timeLeft * 2.5F),1);
	}

	void Update () {
		timeLeft -= Time.deltaTime;

		if(timeLeft < 0F) {
			Destroy(gameObject);
		} else {
			Vector3 pos = transform.position;
			pos.y += Time.deltaTime * speed;
			transform.position = pos;

			transform.localScale = new Vector3(0.01F * (timeLeft * 2.5F), 0.01F * (timeLeft * 2.5F),1);
		}
	}
}
