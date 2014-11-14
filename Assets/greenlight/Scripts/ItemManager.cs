using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GreenLight;

namespace GreenLight{
	public class ItemManager : MonoBehaviour {
		
		public ItemList itemsList;
		
		internal int itemLenght;
		
		[SerializeField]
		private Camera uiCam;
		private float shootTimer = 0;
		public float shootTime = 10;
		private GameObject bulletHolder;
		
		private LinkHolder linkHolder;
		private GameManager gameMngr;
		private GuiManager guiMngr;
		
		//line
		private LineRenderer lineRenderer;
		
		//tower
		private float towerSpawnDistace = 0.3f;
		private TowerManager towerMngr;
		//tower text
		[SerializeField]
		private GameObject towerSellTextPrefab;
		private GameObject towerSellTextHolder;
		private MeshRenderer towerSellTextRender;
		private TextMesh towerSellText;
	
		//gun
		[SerializeField]
		private GameObject gun;
		[SerializeField]
		private Transform gunBulletSpawn;
		
		private bool rButtonPressed;
	
		[SerializeField]
		private GameObject noIcon;
	
		private GameObject noIconHolder;
		
		private Quaternion gunRotation;
		void Awake () {
			
			linkHolder = GameObject.Find("linkHolder").GetComponent<LinkHolder>();
			towerMngr = linkHolder.towerManger;
			gameMngr = linkHolder.gameManager;
			itemsList = linkHolder.itemList;

			bulletHolder = new GameObject("bullets");
			itemLenght = itemsList.items.Length;
			
			guiMngr = Main.instance.gui;
			for ( int i = 0; i < itemLenght; i++){
				/////guiMngr.CreateButton(i);
			}
			
			//create line
			lineRenderer = gameObject.AddComponent<LineRenderer>();
			lineRenderer.material = new Material (Shader.Find("Particles/Additive"));
			lineRenderer.enabled = false;
			lineRenderer.sortingLayerName = "player";
			lineRenderer.sortingOrder = 11;
	
			//create tower sell text 
			towerSellTextHolder = GameObject.Instantiate(towerSellTextPrefab,transform.position,Quaternion.identity) as GameObject;
			//towerSellTextHolder.name = "towerSellText";
			towerSellTextRender = towerSellTextHolder.GetComponent<MeshRenderer>();
			towerSellText = towerSellTextHolder.GetComponent<TextMesh>();
			towerSellTextRender.sortingLayerName = "ui";
			//towerSellText.font = TowerSellFont;
			towerSellText.text = "TowerSellText";
			towerSellTextHolder.SetActive(false);
	
			//create noIcon
			noIconHolder = GameObject.Instantiate(noIcon,transform.position,Quaternion.identity) as GameObject;
			noIconHolder.SetActive(false);
		}
		
		
		/*void OnGUI()
		{
			GUILayout.Box ("\n"+gunRotation.eulerAngles.z.ToString()
			               +"\n L Hor:"+Input.GetAxisRaw("Horizontal")
			               +"\n L Ver:"+Input.GetAxisRaw("Vertical")
			               +"\n R Hor:"+Input.GetAxisRaw("RHorizontal")
			               +"\n R Ver:"+Input.GetAxisRaw("RVertical")
			               +"\n Rotation:"+new Vector3(0,0, movement.DirectionToAngle(new Vector2(Input.GetAxisRaw("RHorizontal"),Input.GetAxisRaw("RVertical"))))
						);
		}*/
		
		void Update(){
	        if(shootTimer>0){
				shootTimer-=Time.deltaTime;
			}
			
			//get input and mouse position
			bool click;
			if(Input.GetAxisRaw("RButton")<0.1f){
				rButtonPressed = false;
			}
			if(Input.GetMouseButtonDown(0)||(!rButtonPressed&&Input.GetAxisRaw("RButton")>0.9f)){
				click = true;
				rButtonPressed = true;
			}else{
				click = false;	
			}
			bool firing = Input.GetMouseButton(0)||Input.GetAxisRaw("RButton")>0.9f;
			Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			Ray uiRay = uiCam.ScreenPointToRay(Input.mousePosition);
			Vector3 mousePosition = new Vector3(mouseRay.origin.x,mouseRay.origin.y,0);
			Vector2 mousePos2D = new Vector2(mouseRay.origin.x,mouseRay.origin.y);
			Vector2 thisPos2D = new Vector2(transform.position.x,transform.position.y);
			Vector2 gunPos2D = new Vector2(gun.transform.position.x,gun.transform.position.y);
	
			//rotate gun
			//Debug.Log(gunRotation);
	        if (BuildTypeData.buildType == BuildType.PC) {
	            gunRotation = movement.RotateToPoint(gun.transform, mousePosition);
	        }else if (BuildTypeData.buildType == BuildType.VITA) {
	            if ((Input.GetAxisRaw("RHorizontal") > 0.1f || Input.GetAxis("RHorizontal") < -0.19f) ||
	                Input.GetAxisRaw("RVertical") > 0.1f || Input.GetAxis("RVertical") < -0.19f){
	                gunRotation.eulerAngles = new Vector3(0, 0, -90 + movement.DirectionToAngle(new Vector2(Input.GetAxis("RHorizontal"), Input.GetAxisRaw("RVertical"))));
	            }
	        }
	
			Quaternion tempRot = new Quaternion();
			if(transform.localScale.x>0){
				tempRot.eulerAngles = new Vector3(gunRotation.eulerAngles.x,
				                                      gunRotation.eulerAngles.y,
				                                      gunRotation.eulerAngles.z - 90-transform.localEulerAngles.z);
			}else{
				tempRot.eulerAngles = new Vector3(gunRotation.eulerAngles.x,
				                                      gunRotation.eulerAngles.y,
				                                      -(gunRotation.eulerAngles.z + 90)+transform.localEulerAngles.z);
			}
			gun.transform.localRotation = tempRot;
			
			if(itemLenght>itemsList.currentItem){
				Collider2D uiHit = Physics2D.OverlapPoint(uiRay.origin
				                                          ,1 << LayerMask.NameToLayer("UI"));
				// ui click
				/*if(uiHit&&click){   
					for (int i = 0;i<itemLenght;i++){
						if(buttonList[i].GetComponent<Collider2D>() == uiHit){
							currentItem = i;
							setItem(currentItem);
							break;
						}
					}
				// game mouse click
				}*/
				
				/*else{
					itemType currentItemType = itemsList.items[currentItem].type;
					switch(currentItemType){
					case itemType.Tower:
						
						//cast aim ray
						Vector2 mousedirection = movement.AngleToDirection(gunRotation.eulerAngles.z);
						RaycastHit2D aimRay = Physics2D.Raycast(gunPos2D
																,mousedirection
																,5
						                                        ,1 << LayerMask.NameToLayer("Level") | 1 << LayerMask.NameToLayer("Towers")
						);
						if(aimRay.collider==null){
							//remove tower sell text en noIcon
							noIconHolder.SetActive(false);
							towerSellTextHolder.SetActive(false);
							//draw aim line if no wall found
							lineRenderer.enabled = true;
							lineRenderer.SetPosition(0, gun.transform.position);
							Vector2 aim = mousedirection.normalized*5 + gunPos2D;
							lineRenderer.SetPosition(1, new Vector3(aim.x,aim.y,0));
						}else{
							if (aimRay.collider.transform.gameObject.layer == LayerMask.NameToLayer("Towers")&&aimRay.collider.transform.gameObject.GetComponent<Tower>().buildPhase==Tower.BuildPhase.Done) {
								int towerSellPrice = aimRay.collider.transform.gameObject.GetComponent<Tower>().sellPrice;
								//remove no icon 
								noIconHolder.SetActive(false);
								//draw aim line if tower found
								lineRenderer.enabled = true;
								lineRenderer.SetPosition(0, gun.transform.position);
								lineRenderer.SetPosition(1, aimRay.point);
								//tower sell text
								towerSellText.color = new Color(1F, 1F, 1F);
								towerSellTextHolder.SetActive(true);
								towerSellText.text = "sell: "+towerSellPrice.ToString();
								towerSellTextHolder.transform.position = new Vector3(aimRay.point.x,aimRay.point.y,0);
								//sell tower
								if (click){  
									aimRay.collider.transform.gameObject.GetComponent<Tower>().DeBuild();
									gameMngr.ChangeMoney(towerSellPrice);
								}
							}else if(aimRay.collider.transform.gameObject.layer == LayerMask.NameToLayer("Towers")&&aimRay.collider.transform.gameObject.GetComponent<Tower>().buildPhase!=Tower.BuildPhase.Done){
								// if aim at building tower
								towerSellTextHolder.SetActive(false);
								lineRenderer.enabled = true;
								lineRenderer.SetPosition(0, gun.transform.position);
								lineRenderer.SetPosition(1, aimRay.point);
	
								//
								noIconHolder.SetActive(true);
								noIconHolder.transform.position = aimRay.point;
	
								
							}else{
								//remove no icon 
								noIconHolder.SetActive(false);
	
								int towerBuyPrice = itemList.items[currentItem].buyPrice;
								if(gameMngr.CheckMoney(towerBuyPrice)) {
									towerSellText.color = new Color(1F, 1F, 1F);
								} else {
									towerSellText.color = new Color(1F, 0F, 0F);
								}
								//remove tower sell text
								//towerSellTextHolder.SetActive(false);
								//draw aim line if wall found
								lineRenderer.enabled = true;
								lineRenderer.SetPosition(0, gun.transform.position);
								lineRenderer.SetPosition(1, aimRay.point);
								//tower buy text
								towerSellTextHolder.SetActive(true);
								towerSellText.text = "buy: "+towerBuyPrice.ToString();
								towerSellTextHolder.transform.position = new Vector3(aimRay.point.x,aimRay.point.y,0);
							   //spawn tower
								Collider2D mouseCircle = Physics2D.OverlapCircle(aimRay.point
								                                                 ,towerSpawnDistace
								                                                 ,1 << LayerMask.NameToLayer("Towers"));
								if(mouseCircle==null&&gameMngr.CheckMoney(towerBuyPrice)){
									if (click){ 
										gameMngr.ChangeMoney(-towerBuyPrice);
										towerMngr.LoadTower(aimRay.point
															,ItemList.items[currentItem].gameObject
															,aimRay.normal);
									}
									break;
								}
								if(mouseCircle&&gameMngr.CheckMoney(towerBuyPrice)){
									// if aim at ground close by tower
									towerSellTextHolder.SetActive(false);
									lineRenderer.enabled = true;
									lineRenderer.SetPosition(0, gun.transform.position);
									lineRenderer.SetPosition(1, aimRay.point);
									
									//
									noIconHolder.SetActive(true);
									noIconHolder.transform.position = aimRay.point;
									break;
								}
							}
						}
						break;
					case itemType.Bullet:
						//fire bullet
						noIconHolder.SetActive(false);
						towerSellTextHolder.SetActive(false);
						lineRenderer.enabled = false;
						if (firing&&shootTimer<=0){
							shootTimer = shootTime;
							GameObject bul = GameObject.Instantiate( ItemList.items[currentItem].gameObject
							                                        ,gunBulletSpawn.position
	                                                                ,gunRotation) as GameObject;
							bul.transform.parent = bulletHolder.transform;
						}
						break;
					case itemType.Mine:
						int minePrice = ItemList.items[currentItem].buyPrice;
						//buy mine text
						towerSellText.color = new Color(1F, 1F, 1F);
						towerSellTextHolder.SetActive(true);
						towerSellText.text = "buy: "+minePrice.ToString();
						towerSellTextHolder.transform.position = new Vector3((transform.position.x-0.6f),(transform.position.y+0.5f),0);
						//buy mine bullet
						lineRenderer.enabled = false;
						if (firing&&shootTimer<=0){
							if(gameMngr.CheckMoney(minePrice)){
								if (click){ 
									gameMngr.ChangeMoney(-minePrice);
									GameObject.Instantiate( ItemList.items[currentItem].gameObject
									                                        ,gunBulletSpawn.position
	                                                                        ,gunRotation);
								}
								break;
							}
							shootTimer = shootTime;
							//GameObject bul = GameObject.Instantiate( items[currentItem]
							//                                        ,gunBulletSpawn.position 
							//                                        ,movement.RotateToPoint(gun.transform,mousePosition)) as GameObject;
							//bul.transform.parent = bulletHolder.transform;
						}
						break;
					}
				}*/
			}
		}
	}

}


