﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GreenLight{
	public class PathManager : MonoBehaviour 
	{
		private GameObject[] paths;
	
		void Awake () {
			int childscount = this.gameObject.transform.childCount;
			paths = new GameObject[childscount];
			
			for(int i = 0; i < childscount; i++) {
				Transform pathTransf = this.gameObject.transform.GetChild(i);
				if(pathTransf.gameObject.name != "path"){
					Debug.LogError("only path allowed in path manager");
				}
				pathData pathData = pathTransf.gameObject.GetComponent<pathData>();
				if(pathData == null){
					Debug.LogError("path must have pathData attatched");
				}
				pathData.Init();
				GameObject path = pathTransf.gameObject;
				paths[i] = path;
			}
		}
		
		public Vector3[] getClosestPath(Vector3 enemy) {
			int closest = 0;
			float distance = Vector3.Distance(paths[closest].GetComponent<pathData>().getStart(), enemy);
	
			for(int p = 1; p < paths.Length; p++) {
				float newDistance = Vector3.Distance(paths[p].GetComponent<pathData>().getStart(), enemy);
				if(newDistance < distance) {
					distance = newDistance;
					closest = p;
				}
			}
	
			return paths[closest].GetComponent<pathData>().getPoints();
		}
	
		public Vector3[] getRandomPath(Vector3 enemy, int reqDistance) {
			Debug.Log("pathcount:"+paths.Length);
			ArrayList pathlist = new ArrayList();
			
			//return a random path within reqDistance
			for(int p = 0; p < paths.Length; p++) {
				float distance = Vector3.Distance(paths[p].GetComponent<pathData>().getStart(), enemy);
				Debug.Log("dist:"+distance);
				if(distance < reqDistance) {
					pathlist.Add(p);
				}
			}
			if(pathlist.Count > 0) {
				int randomPathID = Random.Range(0, pathlist.Count);
				return paths[(int) pathlist[randomPathID]].GetComponent<pathData>().getPoints();
			}
			//return a the closest path
			
			
			
			return null;
		}
	}
}
