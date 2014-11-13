using UnityEngine;
using System.Collections;

public class SetLayerParticleSystem : MonoBehaviour {
	[SerializeField]
	private Component comp;
	public string layerName;
	public int layerOrder;
	
	[ExecuteInEditMode]
	void Start () {
		particleSystem.renderer.sortingLayerName = layerName;
		particleSystem.renderer.sortingOrder = layerOrder;
	}
}
