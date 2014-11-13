using UnityEngine;
using System.Collections;

namespace GreenLight{
	public class GameManager : MonoBehaviour {
		///////
		[SerializeField]
		private int livePoints = 10;
		
		[SerializeField]
		private int money = 25;
		
		[SerializeField]
		private TextMesh livePointDisplay;
		[SerializeField]
		private TextMesh moneyDisplay;
		[SerializeField]
		private GameObject holderMoneyDisplay;
		
		private winLoseScreen winLose;
		
		private void Start(){
			livePointDisplay.text = "life: "+livePoints.ToString();
			moneyDisplay.text = "money: "+money.ToString();
			winLose = gameObject.GetComponent<winLoseScreen>() as winLoseScreen;
		}
		
		public void ChangeLife ( int c) {
			livePoints+=c;
			if(livePoints<0){
				//livePointDisplay.text = "life: "+livePoints.ToString()+" gameOver";
				GameOver();
			}else{
				livePointDisplay.text = "life: "+livePoints.ToString();
			}
		}
		
		public void GameOver(){
			if(winLose.gameStatus == GameStatus.Playing){
				winLose.gameStatus = GameStatus.GameOver;
			}
		}
		
		public void GameWon(){
			if(winLose.gameStatus == GameStatus.Playing){
				winLose.gameStatus = GameStatus.Won;
			}
		}
		
		public void ChangeMoney ( int c) {
			money+=c;
			moneyDisplay.text = "money: "+money.ToString();
		}
		
		public bool CheckMoney ( int c) {
			if(money>=c){
				return true;
			}else{
				return false;
			}
		}
	}
}

