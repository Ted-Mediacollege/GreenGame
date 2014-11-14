using UnityEngine;
using System.Collections;
using System.Collections.Generic;

enum InputState{
	normal,
	multiselect,
	buildingPlace
}
public class Game : MonoBehaviour {

	[SerializeField]
	private Camera cam;
	[SerializeField]
	private GameObject multiSelectArea;

	[SerializeField]
	private GameObject preBuildImage;

	private bool mouseDown = false;
	private Vector2 oldPos;
	private int[] selectedIDs;
	private List<MapObject> selected = new List<MapObject>();
	private int[] location;
	//private bool multiselect;
	private Vector3 startMousePos;
	private VecInt startTilePos;
	private bool releasingPreBuildImage;
	private InputState state;
	private int buildingId;
	private static Game _instance;
	private int buildingTileWidth = 2;

    private bool mouseButton1Up;

	public static Game instance{
		get{
			return _instance;
		}
	}
	private void Awake(){
		_instance = this;
		state = InputState.normal;
	}
	void OnEnable(){
		EventManager.OnGuiInput += GuiInput;
	}
	void OnDisable(){
		EventManager.OnGuiInput -= GuiInput;
	}
	private void GuiInput(string message){
		Debug.Log("[Game]: Event Message: "+message);
		bool buildSomething = true;
		if (message == "Button1") {
			fullPreBuildImage(LevelData.staticBuildings[0]);
			buildingId = 0;
		}else if (message == "Button2") {
			fullPreBuildImage(LevelData.staticBuildings[1]);
			buildingId = 1;
		}else if (message == "Button3") {
			fullPreBuildImage(LevelData.staticBuildings[2]);
			buildingId = 2;
		}else if (message == "Button4") {
			fullPreBuildImage(LevelData.staticBuildings[3]);
			buildingId = 3;
		}else if (message == "Button5") {
			fullPreBuildImage(LevelData.staticBuildings[4]);
			buildingId = 4;
		}else{
			buildSomething = false;
		}
		if(buildSomething){
			StartCoroutine(PreBuildImageFollowCursor());
			releasingPreBuildImage = true;
		}
	}
	public bool fullPreBuildImage(GameObject buildingSprite){
        state = InputState.buildingPlace;
		preBuildImage.GetComponent<SpriteRenderer>().sprite = buildingSprite.GetComponent<SpriteRenderer>().sprite;
		Color color = preBuildImage.renderer.material.color;
		color.a = 0.5f;
		preBuildImage.renderer.material.color = color;
		return true;
	}
	private IEnumerator PreBuildImageFollowCursor(){
		Vector2 currentMousePos = IsoMath.getMouseWorldPosition();
        if (mouseButton1Up && releasingPreBuildImage)
		{
			float buildingTileWidthX = 0;
			float buildingTileWidthY = 0;
			Vector2 hello = IsoMath.worldToTile(currentMousePos.x - buildingTileWidthX,currentMousePos.y + buildingTileWidthY);
			VecInt hello2 = new VecInt((int)hello.x,(int)hello.y);
			LevelData.constructBuilding(hello2.x,hello2.y,buildingId,buildingTileWidth);
			Color color = preBuildImage.renderer.material.color;
			color.a = 0;
			preBuildImage.renderer.material.color = color;
			releasingPreBuildImage = false;
            state = InputState.normal;
		}else{
			if(buildingId < 2)
			{
				buildingTileWidth = 2;
				preBuildImage.transform.position = new Vector3(currentMousePos.x-0.50f, currentMousePos.y+0.25f, 0);
			}else if(buildingId == 3)
			{
				buildingTileWidth = 1;
				preBuildImage.transform.position = new Vector3(currentMousePos.x-0.60f, currentMousePos.y+0.55f, 0);
			}else if(buildingId == 4 || buildingId == 2)
			{
				buildingTileWidth = 3;
				preBuildImage.transform.position = new Vector3(currentMousePos.x+0.00f, currentMousePos.y+0.1f, 0);
			}
			yield return new WaitForEndOfFrame();
			StartCoroutine(PreBuildImageFollowCursor());
		}

		yield return 2;
	}
	public void UpdateSelect(){
		Vector2? TilePosN = IsoMath.getMouseTilePosition();
		if (state == InputState.multiselect) {
			Vector2 currentMousePos = IsoMath.getMouseWorldPosition();
			float areaWidth = currentMousePos.x-startMousePos.x;
			float areaHeight = currentMousePos.y-startMousePos.y;
			multiSelectArea.transform.localScale = new Vector3(areaWidth,areaHeight,1);
			if (Input.GetMouseButtonUp (0)) {
				VecInt[] selectedArea = IsoMath.Area(startTilePos,areaWidth,areaHeight,new Rect?(new Rect(0,0,LevelData.width,LevelData.height)));
				List<int> tempSelectedIds = new List<int>(); 
				selected.Clear();
				for(int i = 0;i < selectedArea.Length;i++){
					//Debug.Log("selected: ("+selectedArea[i].x+","+selectedArea[i].y+")");
					//LevelData.LoadedGroundTiles[selectedArea[i].x,selectedArea[i].y].GetComponent<SpriteRenderer>().color = new Color(0,0,1,1);
                    MapObject Tempselected = LevelData.GetMapObjects((int)selectedArea[i].x, (int)selectedArea[i].y);//LevelData.mapObjects [(int)selectedArea[i].x, (int)selectedArea[i].y];
					if(Tempselected != null){
						tempSelectedIds.Add(Tempselected.gameObject.GetInstanceID());
						selected.Add(Tempselected);
					}
				}
				selectedIDs = tempSelectedIds.ToArray();
				EventManager.CallOnSelect(selectedIDs);
				state = InputState.normal;
				multiSelectArea.SetActive(false);
			}
        }
        else if (state == InputState.normal){
			if(TilePosN!= null){
				Vector2 TilePos = (Vector2)TilePosN;
				//print ("tile: " + TilePos + "\n");
				//multi select
                if (Input.GetMouseButtonDown(0) && (Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.JoystickButton2)))
                {
					StartMultiSelect();
					startTilePos = new VecInt((int)TilePos.x,(int)TilePos.y);
					Debug.Log("[Main] Multiselect: "+startTilePos);//+selectedIDs.Length);
                }
                else if (Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.JoystickButton4))
                {
				//single select
					selected.Clear();
					//MapObject Tempselected = LevelData.mapObjects [(int)TilePos.x, (int)TilePos.y];
                    MapObject Tempselected = LevelData.GetMapObjects((int)TilePos.x, (int)TilePos.y);
                    if(Tempselected != null){
                        selected.Add(Tempselected);
						selectedIDs = new int[]{selected[0].gameObject.GetInstanceID()};
						Debug.Log("[Main] selected: "+selectedIDs.Length);
					}else if(releasingPreBuildImage){
						StartCoroutine(PreBuildImageFollowCursor());
					}else{
						selectedIDs = new int[]{};
						Debug.Log("[Main] not selected: "+selectedIDs.Length);
					}
					EventManager.CallOnSelect(selectedIDs);
                    #if UNITY_PSM || UNITY_ANDROID
                }
                else if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.JoystickButton4))
                {
                    if (selectedIDs.Length > 0)
                    {
                        FindNewPath();
                    }
                }
                    #else
				}else if(Input.GetMouseButtonDown(1)){
					if(selectedIDs.Length > 0){
						FindNewPath();
					}
				}
                #endif
			}
		}
        mouseButton1Up = Input.GetMouseButtonUp(0);
	}
	private void FindNewPath(){
		Vector2 TilePos = (Vector2)IsoMath.getMouseTilePosition();
		print ("[Main] find path");
		for (int i = 0;i<selected.Count;i++){
			VecInt[] newPath = PathFind.FindPath (
				new VecInt(selected[i].pos.x,selected[i].pos.y)
				, new VecInt((int)TilePos.x,(int)TilePos.y)
				, LevelData.CollsionData
                , true);
			if(newPath != null){
				selected[i].gameObject.GetComponent<Unit>().FollowPath(newPath);
			}
		}
	}
	private void StartMultiSelect(){
		state = InputState.multiselect;
		multiSelectArea.SetActive(true);
		startMousePos = IsoMath.getMouseWorldPosition3D();
		multiSelectArea.transform.position = startMousePos;
		multiSelectArea.transform.localScale = Vector3.zero;
	}
	public void UpdateMove(){
		#if UNITY_PSM || UNITY_ANDROID
	Vector2 deltaPos = new Vector2(Input.GetAxis("RHorizontal") * 0.2f, Input.GetAxis("RHorizontal") * -0.2f);
		Camera.main.transform.Translate(new Vector3(deltaPos.x,deltaPos.y,0));
		#else
        Vector2? currentPos = null;
		//Debug.Log(mouseDown);
		if(Input.GetMouseButtonUp(0)){
			mouseDown = false;
		}
		if(Input.GetMouseButtonDown(0)||mouseDown){
			currentPos = Input.mousePosition;
			mouseDown = true;
		}
		if(Input.GetMouseButtonDown(0)){
			oldPos = (Vector2)currentPos;
		}
		if(mouseDown){
			if(currentPos != null && oldPos != null){
				Vector2 deltaPos = (Vector2)currentPos - oldPos;
				deltaPos *= -0.02f;
				Camera.main.transform.Translate(new Vector3(deltaPos.x,deltaPos.y,0));
				oldPos = (Vector2)currentPos;
			}
		}
		#endif
		
	}
}
