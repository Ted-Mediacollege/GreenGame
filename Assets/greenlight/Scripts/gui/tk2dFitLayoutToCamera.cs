using UnityEngine;
namespace GreenLight{
	[ExecuteInEditMode]
	public class tk2dFitLayoutToCamera : MonoBehaviour{
		[SerializeField]
		private tk2dUILayout layout;
		
		[SerializeField]
		private tk2dCamera cam;
		
		private void Start(){
			float camWidth = cam.ScreenExtents.width;
			float camHeight = cam.ScreenExtents.height;
			//Debug.Log(camWidth);
			//Debug.Log(camHeight);
			layout.SetBounds(new Vector3(-(camWidth/2.0f), -(camHeight/2.0f), 0),new Vector3((camWidth/2.0f), (camHeight/2.0f), 0));
			//layout.Refresh();
			Vector3 pos = new Vector3(-(camWidth/2.0f), (camHeight/2.0f), 0);
			layout.transform.localPosition = pos;
		}
	}
}

