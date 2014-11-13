using UnityEngine;
using System.Collections;

public class DrawGrid : MonoBehaviour
{
	[SerializeField]
	private bool draw;
	
	private float tileSize = 1f;
	private float gridSize = 100;
	
	void OnDrawGizmos()
	{
		if(draw){
			Gizmos.color = Color.magenta;
			for (float y = -gridSize; y < gridSize; y+= tileSize){
				Gizmos.DrawLine(IsoMath.tileToWorld3D(-gridSize+0.5f, y+0.5f, 0.0f),
				                IsoMath.tileToWorld3D(gridSize+0.5f, y+0.5f, 0.0f));
			}
			for (float x = -gridSize; x < gridSize; x+= tileSize){
				Gizmos.DrawLine(IsoMath.tileToWorld3D(x+0.5f, -gridSize+0.5f, 0.0f),
				                IsoMath.tileToWorld3D(x+0.5f, gridSize+0.5f, 0.0f));
			}
		}
	}
}

