using UnityEngine;
using System.Collections;

public class MouseLock : MonoBehaviour {

	void Start () {
		Screen.lockCursor = true;
	}
	
	void Update(){
		if(Input.GetMouseButtonDown(0)){
			Screen.lockCursor = true;
		}
	}
}
