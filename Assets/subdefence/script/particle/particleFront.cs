using UnityEngine;
using System.Collections;

public class particleFront : MonoBehaviour {
	public string layer = "Level";

	void Start () {
		particleSystem.renderer.sortingLayerName = layer;
	}
}
