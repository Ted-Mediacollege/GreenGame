using UnityEngine;
using System.Collections;

namespace GreenLight{
	public class LinkHolder : MonoBehaviour {
		
		
		private TowerManager _towerManager;
		private GuiInterface _guiInterface;
		private GameManager _gameManager;
		[SerializeField]
		private ItemList itemList_;
		
		public ItemList itemList{
			get{
				return itemList_;
			}
		}
		
		public TowerManager towerManger {
			get { 
				if(_towerManager==null){
					_towerManager = gameObject.AddComponent<TowerManager>();
				}
				return _towerManager; 
			}
			/*set {
				_towerManager = value; 
			}*/
		}
		
		public GuiInterface guiInterface {
			get { 
				if(_guiInterface==null){
					_guiInterface = new GuiInterface(this,itemList);;
				}
				return _guiInterface; 
			}
		}
		public GameManager gameManager {
			get { 
				if(_gameManager==null){
					_gameManager = gameObject.AddComponent<GameManager>();
				}
				return _gameManager; 
			}
		}
	}
}
