using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class pathData : MonoBehaviour 
{
	private Vector3 startPoint;
	private Vector3[] points;

	void Start () {
		int childscount = this.gameObject.transform.childCount;
		points = new Vector3[childscount];

		for(int i = 0; i < childscount; i++) {
			Vector3 point = this.gameObject.transform.GetChild(i).position;
			points[i] = point;

			if(i == 0) {
				startPoint = point;
			}
		}
	}

	public Vector3 getStart() {
		return startPoint;
	}

	public Vector3[] getPoints() {
		return points;
	}
}
