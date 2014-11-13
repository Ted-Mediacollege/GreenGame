using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GreenLight{
	public class GuiManager {
		
		private float screenHeight;
		private float screenWidth;
		
		private Vector3 topRight;
		
		private MonoBehaviour monoBehaviour_;
		private List<GameObject> buttonList;
		private ItemList itemList_;
		
		private Vector3 itemDisplasment;
		
		[SerializeField]
		private GameObject itemHolder;
		private int itemLenght;
		
		public GuiManager(MonoBehaviour monoBehaviour,ItemList itemList){
			monoBehaviour_ = monoBehaviour;
			itemList_ = itemList;
			
			screenHeight = 2f * Camera.main.orthographicSize;
			screenWidth = screenHeight * Camera.main.aspect;
			topRight = new Vector3( screenWidth/2, screenHeight/2,10);
			
			buttonList = new List<GameObject>();
			itemLenght = itemList.items.Length;
			itemDisplasment = new Vector3( -0.2f, -0.2f,0);
			
			setItem(itemLenght-1);
		}
		
		public void setItem(int item){
			for ( int i = 0; i < itemLenght; i++){
				if(item ==i){
					buttonList[i].GetComponent<ItemButton>().on = true;
				}else{
					buttonList[i].GetComponent<ItemButton>().on = false;
				}
			}
		}
		
		void fixedUpdate(){//!!!!!!!!!!!!!!!! not yet called
			for ( int i = 0; i < itemLenght; i++){
				if(buttonList[i].GetComponent<ItemButton>().on){
					if(buttonList[i].GetComponent<ItemButton>().colorScale>0)
						buttonList[i].GetComponent<ItemButton>().colorScale-=0.1f;
				}else{
					if(buttonList[i].GetComponent<ItemButton>().colorScale<1)
						buttonList[i].GetComponent<ItemButton>().colorScale+=0.1f;
				}
				float colorScale = buttonList[i].GetComponent<ItemButton>().colorScale/8;
				buttonList[i].transform.localScale = new Vector3(1+(0.125f-colorScale),1+(0.125f-colorScale),1);
			}
			
			for ( int i = 0; i < itemLenght; i++){
				buttonList[i].GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white,Color.grey,buttonList[i].GetComponent<ItemButton>().colorScale);
			}
		}
		
		void update(){
			float scrol = BuildTypeData.itemScroll;
			if(scrol!=0){
				if(scrol>0){
					if(itemList_.currentItem<itemLenght-1){
						itemList_.currentItem++;
					}
				}else{
					if(itemList_.currentItem>0){
						itemList_.currentItem--;
					}
				}
				setItem(itemList_.currentItem);
			}
			
			if(BuildTypeData.buildType == BuildType.PC){
				if(Input.GetKey(KeyCode.Alpha1)) { itemList_.currentItem = 4; setItem(itemList_.currentItem); }
				if(Input.GetKey(KeyCode.Alpha2)) { itemList_.currentItem = 3; setItem(itemList_.currentItem); }
				if(Input.GetKey(KeyCode.Alpha3)) { itemList_.currentItem = 2; setItem(itemList_.currentItem); }
				if(Input.GetKey(KeyCode.Alpha4)) { itemList_.currentItem = 1; setItem(itemList_.currentItem); }
				if(Input.GetKey(KeyCode.Alpha5)) { itemList_.currentItem = 0; setItem(itemList_.currentItem); }
			}
		}
		
		
		public void CreateButton( int i){
			//create empry gameobject
			GameObject button = new GameObject("itemIcon");
			
			// add renderer
			SpriteRenderer img = button.AddComponent<SpriteRenderer>();
			img.sprite = itemList_.items[i].iconS;
			img.sortingLayerName = "ui";
			
			//add collider
			BoxCollider2D collider = button.AddComponent<BoxCollider2D>();
			collider.size = new Vector2(0.64f,0.64f);
			collider.isTrigger = true;
			
			//add data script 
			ItemButton buttonData = button.AddComponent<ItemButton>();
			buttonData.on = false;
			buttonData.colorScale = 0;
			
			//position button
			button.transform.parent = itemHolder.transform;
			button.transform.position = itemHolder.transform.position+topRight+itemDisplasment+new Vector3(-0.32f+i*-0.72f,-0.32f,0);
			button.layer = LayerMask.NameToLayer("UI");
			
			buttonList.Add (button);
		}
		
		
	}
}

