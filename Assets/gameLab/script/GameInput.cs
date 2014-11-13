using UnityEngine;
using System.Collections;

public class GameInput {
	public static void updateInput() {
		Game game = Game.instance;
		Main main = Main.instance;
		//main.gui
		if(Input.GetKey(KeyCode.R)){
			game.UpdateMove ();
		}else{
			game.UpdateSelect();
		}
        #if UNITY_PSM || UNITY_ANDROID
        game.UpdateMove();
        #endif
	}
}
