using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IsoMath : MonoBehaviour {

	private static float tileW = 0.90f;
	private static float tileH = 0.50f;
	private static int i;
	private static int j;

    /*public static bool canConstructHere(int x, int y, int size) {
		for(int i = 0; i < size; i++) {
			for(int j = 0; j < size; j++) {
				if(collsionData[x + i, y + j]) {
					return false;
				}
			}
		}
		return true;
	}*/

    public static Vector3 addSizeToPosition(Vector3 vec, int width, int height,int size)
    {
        float px = vec.x - ((width - 1f) * 0.5f);
        float py = vec.y + ((height - width) * 0.25f);
        Vector2 tilepos = IsoMath.worldToTile(vec.x, vec.y);
        return new Vector3(px, py, (tilepos.y + (size - tilepos.x)) / 2.5f + 2f);
    }

	public static Vector2 tileToWorld(int tileX, int tileY){
		return new Vector2((tileY * tileW/2) + (tileX * tileW/2), -((tileX * tileH/2) - (tileY * tileH/2)));
	}
	
	public static Vector3 tileToWorld3D(float px,float py, float pz){
		return new Vector3((py * tileW/2) + (px * tileW/2), -((px * tileH/2) - (py * tileH/2)),pz);
	}
	
	public static Vector2 worldToTile(float px,float py){
		//float displacementX = 0;
		//float displacementY = 0;
		//px -= displacementX;
		//py -= displacementY;
		
		float tx = 1 / tileW * px - 1 / tileH * py + 0.5f;
		float ty = 1 / tileW * px + 1 / tileH * py - 0.5f +1;
		return new Vector2(tx, ty);
	}
	
	public static Vector2 getMouseWorldPosition(){
		Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		//Vector3 mousePosition = new Vector3(mouseRay.origin.x,mouseRay.origin.y,0);
		Vector2 mousePos2D = new Vector2(mouseRay.origin.x,mouseRay.origin.y);
		return mousePos2D;
	}

	public static Vector3 getMouseWorldPosition3D(){
		Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		Vector3 mousePosition = new Vector3(mouseRay.origin.x,mouseRay.origin.y,10);
		return mousePosition;
	}
	
	public static Vector2? getMouseTilePosition(){
		Vector2 mousePos2D = getMouseWorldPosition();
		Vector2 TilePos = IsoMath.worldToTile(mousePos2D.x,mousePos2D.y);
		//print ("world pos : "+mousePos2D+"\n");
		if (TilePos.x < LevelData.width && TilePos.x > 0 &&
		    TilePos.y < LevelData.height && TilePos.y > 0) {
			//print ("tile pos: " + TilePos + "\n");
			//LevelData.GroundVehicles [(int)Mathf.Floor (TilePos.x), (int)Mathf.Floor (TilePos.y)].GetComponent<SpriteRenderer> ().color = new Color32 (0, 255, 0, 255);
			return TilePos;
		} else {
			return null;
		}
	}

	public static VecInt[] Area(VecInt a,float width,float height,Rect? rect){
		List<VecInt> tempList = new List<VecInt> ();
		List<VecInt> tempList2 = new List<VecInt> ();
		int wSteps = (int)(width / tileW);
		int hSteps = (int)(height / tileH);
		print ("A (" + a.x + "," + a.y + ") width: "+wSteps+" height: "+hSteps);
		int up = 1;
		int right = 1;
		//wSteps += 1;
		//hSteps += 1;
		if (wSteps < 0) {
			wSteps*=-1;
			right = -1;
		}
		if (hSteps < 0) {
			hSteps*=-1;
			up = -1;
		}
		//Debug.Log("hSteps: "+hSteps);
		for (j = 0; j < hSteps; j+=1) {
			//Debug.Log("========================== j: "+j);
			for (i = 0; i < wSteps; i+=1) {
				//Debug.Log("========================== i: "+i);
				int x = a.x +(i*right)-(j*up);
				int y = a.y +(i*right)+(j*up);
				tempList.Add(new VecInt(x,y));
				if((i != wSteps-1)&&(j != hSteps-1)){
					//Debug.Log("========================== end: ");
					if(up == -1 && right == -1){
						y -=1;
					}else if(up==-1){
						x +=1;
					}else if(right==-1){
						x -=1;
					}else{
						y +=1;
					}
					tempList.Add(new VecInt(x,y));
					
				}
			}
		}
		//check if in rect
		if(rect!=null){
			print ("null");
			Rect rec = (Rect)rect;
			for (i =0;i<tempList.Count;i++){
				//print ("(" + tempList[i].x + "," + tempList[i].y + ")");
				if(tempList[i].x>rec.width-1||tempList[i].x<0){
				
				}else if(tempList[i].y>rec.height-1||tempList[i].y<0){
				
				}else{
					tempList2.Add(tempList[i]);
				}
			}
		}
		return tempList2.ToArray ();
	}
}

