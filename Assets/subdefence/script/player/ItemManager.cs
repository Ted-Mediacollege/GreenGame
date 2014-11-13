using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemManager : MonoBehaviour {
	
	[SerializeField]
	private GameObject itemHolder;
	
	private float screenHeight;
	private float screenWidth;
	private Vector3 topRight;
	private Vector3 itemDisplasment;
	
	internal List<GameObject> buttonList;
	public List<GameObject> items;
	
	internal int itemLenght;
	
	[SerializeField]
	private Camera uiCam;
	private float shootTimer = 0;
	public float shootTime = 10;
	private GameObject bulletHolder;
	private int currentItem = 0;
	private GameManager gameMngr;
	
	//line
	private LineRenderer lineRenderer;
	private int lengthOfLineRenderer = 2;
	[SerializeField]
	private Color lineColor1 = Color.yellow;
	[SerializeField]
	private Color lineColor2 = Color.red;
	[SerializeField]
	private Color lineColorTower1 = Color.yellow;
	[SerializeField]
	private Color lineColorTower2 = Color.red;
	[SerializeField]
	private Color lineColorTower3 = Color.yellow;
	[SerializeField]
	private Color lineColorTower4 = Color.red;
	[SerializeField]
	private float linewidth1 = 0.1f;
	[SerializeField]
	private float linewidth2 = 0.1f;
	
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
		buttonList = new List<GameObject>();
		towerMngr = GameObject.Find("gameManager").GetComponent<TowerManager>() as TowerManager;
		gameMngr = GameObject.Find("gameManager").GetComponent<GameManager>() as GameManager;
		bulletHolder = GameObject.Find("bullets");
		itemLenght = items.Count;

		screenHeight = 2f * Camera.main.orthographicSize;
		screenWidth = screenHeight * Camera.main.aspect;
		topRight = new Vector3( screenWidth/2, screenHeight/2,10);
		itemDisplasment = new Vector3( -0.2f, -0.2f,0);
		
		for ( int i = 0; i < itemLenght; i++){
			CreateButton(i);
		}
		setItem(itemLenght-1);
		currentItem = itemLenght-1;
		
		//create line
		lineRenderer = gameObject.AddComponent<LineRenderer>();
		lineRenderer.SetColors(lineColor1, lineColor2);
		lineRenderer.SetWidth(linewidth1, linewidth2);
		lineRenderer.SetVertexCount(lengthOfLineRenderer);
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
	
	public void CreateButton( int i){
		//create empry gameobject
		GameObject button = new GameObject("itemIcon");
		
		// add renderer
		SpriteRenderer img = button.AddComponent<SpriteRenderer>();
		img.sprite = items[i].GetComponent<Item>().iconS;
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
	
	public void setItem(int item){
		for ( int i = 0; i < itemLenght; i++){
			if(item ==i){
				buttonList[i].GetComponent<ItemButton>().on = true;
			}else{
				buttonList[i].GetComponent<ItemButton>().on = false;
			}
		}
	}

	void FixedUpdate(){
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

		float scrol = BuildTypeData.itemScroll;
		if(scrol!=0){
			if(scrol>0){
				if(currentItem<itemLenght-1){
					currentItem++;
				}
			}else{
				if(currentItem>0){
					currentItem--;
				}
			}
			setItem(currentItem);
		}

		if(BuildTypeData.buildType == BuildType.PC){
		    if(Input.GetKey(KeyCode.Alpha1)) { currentItem = 4; setItem(currentItem); }
		    if(Input.GetKey(KeyCode.Alpha2)) { currentItem = 3; setItem(currentItem); }
		    if(Input.GetKey(KeyCode.Alpha3)) { currentItem = 2; setItem(currentItem); }
		    if(Input.GetKey(KeyCode.Alpha4)) { currentItem = 1; setItem(currentItem); }
		    if(Input.GetKey(KeyCode.Alpha5)) { currentItem = 0; setItem(currentItem); }
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
		
		if(itemLenght>currentItem){
			Collider2D uiHit = Physics2D.OverlapPoint(uiRay.origin
			                                          ,1 << LayerMask.NameToLayer("UI"));
			// ui click
			if(uiHit&&click){   
				for (int i = 0;i<itemLenght;i++){
					if(buttonList[i].GetComponent<Collider2D>() == uiHit){
						currentItem = i;
						setItem(currentItem);
						break;
					}
				}
			// game mouse click
			}else{
				itemType currentItemType = items[currentItem].GetComponent<Item>().type;
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
						lineRenderer.SetColors(lineColor1, lineColor2);
					}else{
						if (aimRay.collider.transform.gameObject.layer == LayerMask.NameToLayer("Towers")&&aimRay.collider.transform.gameObject.GetComponent<Tower>().buildPhase==Tower.BuildPhase.Done) {
							int towerSellPrice = aimRay.collider.transform.gameObject.GetComponent<Tower>().sellPrice;
							//remove no icon 
							noIconHolder.SetActive(false);
							//draw aim line if tower found
							lineRenderer.enabled = true;
							lineRenderer.SetPosition(0, gun.transform.position);
							lineRenderer.SetPosition(1, aimRay.point);
							lineRenderer.SetColors(lineColorTower1, lineColorTower2);
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
							lineRenderer.SetColors(lineColorTower3, lineColorTower4);

							//
							noIconHolder.SetActive(true);
							noIconHolder.transform.position = aimRay.point;

							
						}else{
							//remove no icon 
							noIconHolder.SetActive(false);

							int towerBuyPrice = items[currentItem].GetComponent<Tower>().buyPrice;
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
							lineRenderer.SetColors(lineColor1, lineColor2);
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
									towerMngr.LoadTower(aimRay.point,items[currentItem],aimRay.normal);
								}
								break;
							}
							if(mouseCircle&&gameMngr.CheckMoney(towerBuyPrice)){
								// if aim at ground close by tower
								towerSellTextHolder.SetActive(false);
								lineRenderer.enabled = true;
								lineRenderer.SetPosition(0, gun.transform.position);
								lineRenderer.SetPosition(1, aimRay.point);
								lineRenderer.SetColors(lineColorTower3, lineColorTower4);
								
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
						GameObject bul = GameObject.Instantiate( items[currentItem]
						                                        ,gunBulletSpawn.position
                                                                ,gunRotation) as GameObject;
						bul.transform.parent = bulletHolder.transform;
					}
					break;
				case itemType.Mine:
					int minePrice = items[currentItem].GetComponent<Mine>().buyPrice;
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
								GameObject.Instantiate( items[currentItem]
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
			}
		}
	}
}


