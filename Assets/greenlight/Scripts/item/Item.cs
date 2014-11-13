using UnityEngine;
using System.Collections;

namespace GreenLight{
	public enum itemType{
		Bullet,
		Tower,
		Mine
	}
	
	public class Item :MonoBehaviour {
		public itemType type {
			get { return _type;} 
			private set { _type = value;}
		}
		
		[SerializeField]
		private itemType _type;
		
		public Sprite iconS;
		
		void Start(){
		}
	}
}

