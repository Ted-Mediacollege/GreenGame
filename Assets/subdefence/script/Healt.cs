//using UnityEngine;
using System.Collections;

public class Healt{
	private int startHealt;
	private int healt = 100;
	internal bool dead = false;

	public int HP {
		get { return healt;} 
	}

	public Healt (int _healt) {
		healt = _healt; 
		startHealt = HP;
	}

	public void ChangeHealt(int D){
		healt += D;
		if(healt<0){
			dead = true;
		}
	}
}


