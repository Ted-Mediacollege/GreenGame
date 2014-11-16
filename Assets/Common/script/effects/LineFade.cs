using UnityEngine;
using System.Collections;

public class LineFade : MonoBehaviour
{
	private LineRenderer line;
	private float lineWidth = 0.05f;
	private bool lineAlive = true; 
	
	void Awake ()
	{
		line = gameObject.GetComponent<LineRenderer>();
		line.SetColors(Color.gray, Color.gray);
		line.SetWidth(lineWidth, lineWidth);
		line.SetVertexCount(2);
		line.material = new Material (Shader.Find("Particles/Additive"));
		StartCoroutine(FadeLine());
	}
	
	IEnumerator FadeLine(){
		FadeLine();
		while(lineAlive){
			yield return new WaitForSeconds(0.1f);
			lineWidth -= 0.01f;
			line.SetWidth(lineWidth, lineWidth);
			if(lineWidth<=0.0f){
				GameObject.Destroy(gameObject);
				lineAlive = false;
			}
		}
	}
}

