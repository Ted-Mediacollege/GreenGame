using UnityEngine;
using System.Collections;

public class DeleteParticleSystem : MonoBehaviour {

	private ParticleSystem ps;
	
	void Start () {
		ps = gameObject.GetComponent<ParticleSystem>();
	}
	
	void Update () {
		if(ps)
		{
			if(!ps.IsAlive())
			{
				Destroy(gameObject);
			}
		}
	}
}
