using UnityEngine;
using System.Collections;

public class CameraSize : MonoBehaviour {
	[SerializeField]
	private int pixelHeight = 544;


	void Start () {
		SetSize ();
	}

	private void SetSize(){
		// set the camera to the correct orthographic size (so scene pixels are 1:1)
		float s_baseOrthographicSize = pixelHeight / 100.0f / 2.0f;
		gameObject.GetComponent<Camera>().camera.orthographicSize = s_baseOrthographicSize;
	}
}
