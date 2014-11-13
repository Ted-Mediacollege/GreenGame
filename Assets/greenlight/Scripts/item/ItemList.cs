using UnityEngine;
using System.Collections;

namespace GreenLight{
	public class ItemList : MonoBehaviour
	{
		public Item[] items;
		public int currentItem = 0;
		
		private void Awake(){
			currentItem = items.Length-1;
		}
	}
}

