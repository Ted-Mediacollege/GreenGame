using UnityEngine;
using System.Collections;

[System.Serializable]
public class LifeBar {
	
	[SerializeField]
	private GameObject lifeBarIcon;
	
	private MonoBehaviour creator;
	internal void init(MonoBehaviour _creator)
	{
		creator = _creator;
	}

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
