using UnityEngine;
public class VecInt
{
	public int x;
	public int y;
	public VecInt(int _x,int _y){
		x = _x;
		y = _y;
	}
	
	public string print{
		get{
			string ret = "("+x+","+y+")";
			return ret;
		}
	}
	
	public Vector3 vec3(int z){
		return new Vector3(x,y,z);
	}
}

